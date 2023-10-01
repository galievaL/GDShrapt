using System;
using System.Linq;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        public void Visit(GDClassDeclaration d)
        {
            ////////

            var name = d.ClassName?.Identifier?.ToString() ?? _conversionSettings.ClassName;

            _partsCode = SyntaxFactory.ClassDeclaration(GetValidClassName(name)).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        }

        public string GetValidClassName(string className)
        {
            if (className[0] != '_' && !char.IsLetter(className[0]))
                className = '_' + className;

            className = Regex.Replace(className, "[^a-zA-Zа-яА-Я0-9_]", string.Empty);

            return className;
        }

        public void Visit(GDDictionaryKeyValueDeclaration d)
        {
            throw new System.NotImplementedException(); 
        }

        public void Visit(GDEnumDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDEnumValueDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExportDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDInnerClassDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCaseDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDParameterDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDSignalDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDVariableDeclaration d) //объявление всех полей
        {
            var constKeyword = SyntaxFactory.Token(SyntaxKind.ConstKeyword);
            var stringType = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
            var variableName = SyntaxFactory.Identifier("HTerrainData");
            var equalsToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
            var initializerValue = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("./ hterrain_data.gd"));
            var variableDeclarator = SyntaxFactory.VariableDeclarator(variableName).WithInitializer(SyntaxFactory.EqualsValueClause(equalsToken, initializerValue));
            var variableDeclaration = SyntaxFactory.VariableDeclaration(stringType).AddVariables(variableDeclarator);
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration).AddModifiers(constKeyword);
        }

        public void Visit(GDIfBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElseBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElifBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDClassAtributesList list)
        {
            ////////
        }

        public void Visit(GDClassMembersList list)
        {
            ////////
        }

        public void Visit(GDDictionaryKeyValueDeclarationList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElifBranchesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDEnumValuesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExportParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExpressionsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCasesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDPathList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDLayersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDStatementsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMethodDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDToolAtribute a)
        {
            ////////
            var toolAtributeName = "Tool";
            _partsCode = _partsCode.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName(toolAtributeName)))));
        }

        public void Visit(GDClassNameAtribute a)
        {
            ////////
        }

        public void Visit(GDExtendsAtribute a)
        {
            ////////
            var extendsAtribute = a.ToString().Split(' ');
            _partsCode = _partsCode.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(extendsAtribute[1])));
        }

        public void Visit(GDExpressionStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIfStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDForStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDVariableDeclarationStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDWhileStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDArrayInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBoolExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBracketExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBreakExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBreakPointExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDCallExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDContinueExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDDictionaryInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDDualOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDGetNodeExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIdentifierExpression e)//
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIfExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIndexerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCaseVariableExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchDefaultOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDNodePathExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDNumberExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMemberOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDPassExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDReturnExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDSingleOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDStringExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDYieldExpression e)
        {
            throw new System.NotImplementedException();
        }
    }
}