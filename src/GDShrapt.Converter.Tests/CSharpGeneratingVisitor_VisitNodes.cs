using System;
using System.Linq;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        public void Visit(GDClassDeclaration d)
        {
            var name = d.ClassName?.Identifier?.ToString() ?? _conversionSettings.ClassName;

            _partsCode = ClassDeclaration(GetValidClassName(name)).AddModifiers(Token(SyntaxKind.PublicKeyword));
        }

        public string GetValidClassName(string className)
        {
            if (className[0] != '_' && !char.IsLetter(className[0]))
                className = '_' + className;

            className = Regex.Replace(className, "[^a-zA-Zа-яА-Я0-9_]", string.Empty);

            return className;
        }

        public void Visit(GDVariableDeclaration d)
        {
            ///GDVariableDeclaration 'const ANSWER = 42'
            ///
            var @var = d.VarKeyword;
            var identifier = d.Identifier;
            var ttype = d.Type;
            var initializer = d.Initializer;
            var initializerType = d.Initializer.TypeName;
            var @const = d.ConstKeyword;

            //var f1 = GetStringVariableDeclaration("THE_NAME", "Charly");
            //var f2 = GetBoolVariableDeclaration("BBBB", true);
            //var f3 = GetIntVariableDeclaration("ANSWER", 42);

            FieldDeclarationSyntax member = default;

            switch (initializerType)
            {
                case "GDStringExpression":
                    member = GetVariableDeclaration(identifier.ToString(), ((GDStringExpression)initializer).String.Value, d.ConstKeyword != null);
                    break;
                case "GDNumberExpression":
                    var number = ((GDNumberExpression)initializer).Number;
                    SyntaxKind typeVariable;
                    SyntaxToken literal;

                    switch (number.ResolveNumberType())
                    {
                        case GDNumberType.LongDecimal:
                        case GDNumberType.LongBinary:
                        case GDNumberType.LongHexadecimal:
                            typeVariable = SyntaxKind.LongKeyword;
                            literal = Literal(number.ValueInt64);
                            break;
                        case GDNumberType.Double:
                            typeVariable = SyntaxKind.DoubleKeyword;
                            literal = Literal(number.ValueDouble);
                            break;
                        case GDNumberType.Undefined:
                            typeVariable = SyntaxKind.LongKeyword;
                            literal = Literal(0);
                            //TODO: add a comment output with the actual value
                            break;
                        default:
                            typeVariable = SyntaxKind.LongKeyword;
                            literal = Literal(0);
                            break;
                    }

                    var literalExpression = LiteralExpression(SyntaxKind.NumericLiteralExpression, literal);
                    member = GetVariableDeclaration(identifier.ToString(), typeVariable, literalExpression, d.ConstKeyword != null);

                    break;
                case "GDBoolExpression":
                    member = GetVariableDeclaration(identifier.ToString(), bool.Parse(initializer.ToString()));
                    break;
                default:
                    break;
            }

            if (member != null)
                _partsCode = _partsCode.AddMembers(member);
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, bool value, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var typeVariable = SyntaxKind.BoolKeyword;

            var returnValueType = value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression;
            var literalExpression = LiteralExpression(returnValueType);

            return GetVariableDeclaration(nameVariable, typeVariable, literalExpression, isConst, accessModifier);
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, string returnValue, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var typeVariable = SyntaxKind.StringKeyword;
            var returnValueType = SyntaxKind.StringLiteralExpression;
            var literalExpression = LiteralExpression(returnValueType, Literal(returnValue));

            return GetVariableDeclaration(nameVariable, typeVariable, literalExpression, isConst, accessModifier);
        }

        //public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, int returnValue, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        //{
        //    var typeVariable = SyntaxKind.IntKeyword;
        //    var returnValueType = SyntaxKind.NumericLiteralExpression;
        //    var literalExpression = LiteralExpression(returnValueType, Literal(returnValue));

        //    return GetVariableDeclaration(nameVariable, typeVariable, literalExpression, isConst, accessModifier);
        //}

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, SyntaxKind typeVariable, LiteralExpressionSyntax literalExpression, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var nameDeclaration1 = VariableDeclarator(Identifier(nameVariable)).WithInitializer(EqualsValueClause(literalExpression));

            var tokenList = isConst ? TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword)) : TokenList(Token(accessModifier));

            var field = FieldDeclaration(VariableDeclaration(PredefinedType(Token(typeVariable))).WithVariables(SingletonSeparatedList(nameDeclaration1)))
                                        .WithModifiers(tokenList);

            return field;
        }

        public void Visit(GDClassAtributesList list)
        {
            ///// GDClassAtributesList 'tool || class_name HTerrainDataSaver || extends ResourceFormatSaver
        }

        public void Visit(GDClassMembersList list)
        {
            ///// GDClassMembersList 'const ANSWER = 42\nconst THE_NAME = \"Charly\"\n'
        }

        public void Visit(GDToolAtribute a)
        {
            var toolAtributeName = "Tool";
            _partsCode = _partsCode.AddAttributeLists(AttributeList(SingletonSeparatedList(Attribute(ParseName(toolAtributeName)))));
        }

        public void Visit(GDClassNameAtribute a)
        {
            //// GDClassNameAtribute 'class_name HTerrainDataSaver'
        }

        public void Visit(GDExtendsAtribute a)
        {
            var extendsAtribute = a.ToString().Split(' ');
            _partsCode = _partsCode.AddBaseListTypes(SimpleBaseType(ParseTypeName(extendsAtribute[1])));
        }

        public void Visit(GDIdentifierExpression e)//
        {
            throw new NotImplementedException();
        }

        public void Visit(GDNumberExpression e)//
        {
            // 42
        }

        #region
        public void Visit(GDDictionaryKeyValueDeclaration d)
        {
            throw new NotImplementedException(); 
        }

        public void Visit(GDEnumDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDEnumValueDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDExportDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDInnerClassDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMatchCaseDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDParameterDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDSignalDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDIfBranch b)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDElseBranch b)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDElifBranch b)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDDictionaryKeyValueDeclarationList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDElifBranchesList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDEnumValuesList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDExportParametersList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDExpressionsList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMatchCasesList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDParametersList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDPathList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDLayersList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDStatementsList list)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMethodDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDExpressionStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDIfStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDForStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMatchStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDVariableDeclarationStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDWhileStatement s)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDArrayInitializerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDBoolExpression e)
        {
            ////
        }

        public void Visit(GDBracketExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDBreakExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDBreakPointExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDCallExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDContinueExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDDictionaryInitializerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDDualOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDGetNodeExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDIfExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDIndexerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMatchCaseVariableExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMatchDefaultOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDNodePathExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDMemberOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDPassExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDReturnExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDSingleOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Visit(GDStringExpression e)
        {
            ////
        }

        public void Visit(GDYieldExpression e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}