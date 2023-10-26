using System;
using System.Collections.Generic;
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

        public void Visit(GDVariableDeclaration d)
        {
            var @var = d.VarKeyword;
            var identifier = d.Identifier.ToString();
            var ttype = d.Type;
            var initializer = d.Initializer;
            var initializerType = d.Initializer.TypeName;
            var @const = d.ConstKeyword;

            FieldDeclarationSyntax member = default;

            switch (initializerType)
            {
                case "GDStringExpression":
                    member = GetVariableDeclaration(identifier, ((GDStringExpression)initializer).String.Value, d.ConstKeyword != null);
                    break;
                case "GDNumberExpression":
                    var typeV = GetSyntaxKind((GDNumberExpression)initializer);
                    var literalExpression = GetLiteralExpression((GDNumberExpression)initializer);

                    member = GetVariableDeclarationWithPredefinedType(identifier, typeV, literalExpression, d.ConstKeyword != null);
                    break;
                case "GDBoolExpression":
                    member = GetVariableDeclarationWithPredefinedType(identifier, SyntaxKind.BoolKeyword, GetLiteralExpression(bool.Parse(initializer.ToString())));
                    break;
                case "GDCallExpression":
                    var arguments = new List<ArgumentSyntax>();
                    var nodeArguments = initializer.Nodes.ToList()[1].Nodes.ToList();

                    foreach (var arg in nodeArguments)
                    {
                        switch (arg.TypeName)
                        {
                            case "GDStringExpression":
                                arguments.Add(Argument(GetLiteralExpression((GDStringExpression)arg)));
                                break;
                            case "GDNumberExpression":
                                arguments.Add(Argument(GetLiteralExpression((GDNumberExpression)arg)));
                                break;
                            case "GDBoolExpression":
                                arguments.Add(Argument(GetLiteralExpression(bool.Parse(arg.ToString()))));
                                break;
                            case "GDCallExpression":
                                throw new NotImplementedException();
                                break;
                            default:
                                break;
                        }
                    }

                    var methodName = initializer.Nodes.ToList()[0].ToString();
                    NameSyntax typeVariable = default;
                    ExpressionSyntax expression = default;

                    switch (methodName)
                    {
                        case "Vector2":
                        case "Vector3":
                        case "Vector4":
                        case "Rect2":
                        case "Vector2I":
                        case "Vector3I":
                        case "Vector4I":
                        case "Rect2I":
                            typeVariable = IdentifierName(methodName);
                            expression = CreateMethodObjectCreationExpression(methodName, arguments);
                            break;
                        case "preload":
                            typeVariable = IdentifierName("GodotObject");
                            expression = CreateMethodInvocationExpression("ResourceLoader.Load", arguments);
                            break;
                        default:
                            typeVariable = IdentifierName("Variant");
                            expression = CreateMethodObjectCreationExpression(methodName, arguments);
                            break;
                    }
                    member = GetVariableDeclaration(identifier, typeVariable, expression);
                    break;
                default:
                    break;
            }

            if (member != null)
                _partsCode = _partsCode.AddMembers(member);
        }


        public void Visit(GDArrayInitializerExpression e)
        {
            var allCollection = e.Values.Nodes.ToList();
            var p = (GDVariableDeclaration)e.Parent;
            var identifier = p.Identifier.ToString();
            var initializerType = p.Initializer.TypeName;

            var type = allCollection[0].TypeName;
            var isDifferentTypes = allCollection.Any(c => c.TypeName != type);

            IEnumerable<ExpressionSyntax> literalExpression = null;
            InitializerExpressionSyntax initializerExpression;
            SyntaxToken predefinedType = new SyntaxToken();

            FieldDeclarationSyntax collection = null;

            if (isDifferentTypes)
            {
                predefinedType = ParseToken("Godot.Variant"); //ToDo: выяснить, нужно ли убирать слово годот, так как есть юзинг годота в коде

                //collection = CreateField(identifier, predefinedType, CreateArrayInitializerString(allCollection));
                //ToDo дописать наполнение для collection в случае, если элементы коллекции имеют разные типы
            }
            else
            {
                switch (type)
                {
                    case "GDStringExpression":
                        literalExpression = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression((GDStringExpression)value));
                        initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpression));

                        predefinedType = Token(SyntaxKind.StringKeyword);
                        collection = CreateArrayField(identifier, PredefinedType(predefinedType), initializerExpression);
                        break;
                    case "GDNumberExpression":
                        literalExpression = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression((GDNumberExpression)value));
                        initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpression));

                        var isDouble = allCollection.Any(x => GetSyntaxKind((GDNumberExpression)x) == SyntaxKind.DoubleKeyword);
                        predefinedType = Token(isDouble ? SyntaxKind.DoubleKeyword : GetSyntaxKind((GDNumberExpression)allCollection[0]));
                        collection = CreateArrayField(identifier, PredefinedType(predefinedType), initializerExpression);
                        break;
                    case "GDBoolExpression":
                        literalExpression = allCollection.Select(value => GetLiteralExpression(bool.Parse(value.ToString())));
                        initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpression));

                        predefinedType = Token(SyntaxKind.BoolKeyword);
                        collection = CreateArrayField(identifier, PredefinedType(predefinedType), initializerExpression);
                        break;
                    case "GDCallExpression":
                        var expList = e.Nodes.ToList()[0];
                        var callExprList = expList.Nodes.ToList();
                        var callExpr = callExprList[1];

                        var identifierExpr = ((GDIdentifierExpression)callExpr.Nodes.ToList()[0]).Identifier.ToString();
                        var exprList = (callExpr.Nodes.ToList()[1]).Nodes;//.ToList()[0];

                        //var h = (expList as GDNumberExpression).Number.ResolveNumberType();
                        //List<InvocationExpressionSyntax> invocationExpressionSyntax = new List<InvocationExpressionSyntax>();
                        InvocationExpressionSyntax[] invocationExpression = new InvocationExpressionSyntax[callExprList.Count];
                        int i = 0;

                        foreach (var callExprItem in callExprList)
                        {
                            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                            string ident = "";

                            foreach (var item in callExprItem.Nodes.ToList())
                            {
                                if (item.TypeName == "GDIdentifierExpression")
                                    ident = ((GDIdentifierExpression)item).Identifier.ToString();
                                else if (item.TypeName == "GDExpressionsList")
                                {
                                    var expressions = ((GDExpressionsList)item).Nodes.ToList();

                                    foreach (var arg in expressions)
                                    {
                                        switch (arg.TypeName)
                                        {
                                            case "GDStringExpression":
                                                arguments.Add(Argument(GetLiteralExpression((GDStringExpression)arg)));
                                                break;
                                            case "GDNumberExpression":
                                                arguments.Add(Argument(GetLiteralExpression((GDNumberExpression)arg)));
                                                break;
                                            case "GDBoolExpression":
                                                arguments.Add(Argument(GetLiteralExpression(bool.Parse(arg.ToString()))));
                                                break;
                                            case "GDCallExpression":
                                                throw new NotImplementedException();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                else
                                    throw new NotImplementedException();
                            }
                            /////
                            var uuu = new GDIdentifier();
                            var tt = p.Identifier.TryExtractLocalScopeVisibleDeclarationFromParents(out uuu);//Поиск соответствий по названию во всем коде
                            //////

                            //var invocationExpression2 = CreateMethodInvocationExpression("Variant.CreateFrom", arguments);

                            if (ident != null && arguments != null)
                            {
                                var exp = CreateMethodObjectCreationExpression(ident, arguments);
                                invocationExpression[i] = CreateVariantCreateFromMethodInvocationExpression(exp);
                                ++i;
                            }
                        }

                        var gg = invocationExpression.Select(value => (ExpressionSyntax)value);
                        initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(gg));

                        //initializerExpression = CreateArrayInitializer2(invocationExpression);

                        collection = CreateArrayField(identifier, IdentifierName("Variant"), initializerExpression);
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }
            }

            if (collection != null)
                _partsCode = _partsCode.AddMembers(new MemberDeclarationSyntax[] { collection });
        }
        public void Visit(GDExpressionsList list)
        {
        }

        public void Visit(GDClassAtributesList list)
        {
        }

        public void Visit(GDClassMembersList list)
        {
        }

        public void Visit(GDToolAtribute a)
        {
            var toolAtributeName = "Tool";
            _partsCode = _partsCode.AddAttributeLists(AttributeList(SingletonSeparatedList(Attribute(ParseName(toolAtributeName)))));
        }

        public void Visit(GDClassNameAtribute a)
        {
        }

        public void Visit(GDExtendsAtribute a)
        {
            var extendsAtribute = a.ToString().Split(' ');
            _partsCode = _partsCode.AddBaseListTypes(SimpleBaseType(ParseTypeName(extendsAtribute[1])));
        }

        public void Visit(GDIdentifierExpression e)
        {
        }

        public void Visit(GDNumberExpression e)
        {
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