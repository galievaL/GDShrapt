﻿using System;
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
            _className = d.ClassName?.Identifier?.ToString() ?? _conversionSettings.ClassName;

            _partsCode = ClassDeclaration(ValidateTypeAndNameHelper.GetValidateClassName(_className)).AddModifiers(Token(SyntaxKind.PublicKeyword));
        }

        //ConstructorDeclarationSyntax GetConstructorDeclaration(string constructorsName, SyntaxKind accessModifier = SyntaxKind.PublicKeyword, params ExpressionStatementSyntax[] statementSyntaxes)
        //{
        //    return ConstructorDeclaration("Builder").WithBody(Block(statementSyntaxes)).AddModifiers(Token(accessModifier));
        //}

        //ExpressionStatementSyntax GetExpressionStatement(string identifierType, TypeSyntax initializerType, params ArgumentSyntax[] argumentsOfInitializer)
        //{
        //    var objectCreationExpression = ObjectCreationExpression(initializerType).AddArgumentListArguments(argumentsOfInitializer);
        //    return ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifierType), objectCreationExpression));
        //}

        void AAAaaa(GDIdentifier methodNameIdentifier)
        {
            var methodName = methodNameIdentifier.ToString();
            var gdIdent = new GDIdentifier();
            ExpressionStatementSyntax expressionStatement = default;

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
            {
                var expressionSyntax = CreateMethodObjectCreationExpressionSyntax(methodName);
                literalExpression = ReplaceArgumentsInExpressionSyntax(expressionSyntax, arguments);
            }
            else if (methodName == "preload")
            {
                var expressionSyntax = CreateMethodInvocationExpressionSyntax("ResourceLoader", "Load");
                literalExpression = AddArgumentToExpressionSyntax(expressionSyntax, arguments);

            }
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
            {
                ///ToDo: дописать
            }
            else
            {
                //var expressionSyntax = CreateMethodInvocationExpressionSyntax("Call");
                //literalExpression = AddArgumentToExpressionSyntax(expressionSyntax, arguments);

                expressionStatement = GetInvocationExpressionStatement(identifier, stringLiteral, "Call", arguments.ToArray());
            }
        }

        public void Visit(GDVariableDeclaration d)
        {
            var kk = d.Type;
            var identifier = ValidateTypeAndNameHelper.GetValidateFieldName(d.Identifier.ToString());
            var initializer = d.Initializer;
            var isConst = d.ConstKeyword != null;
            var isThereColon = d.Colon != null;

            MemberDeclarationSyntax member = default;
            ExpressionSyntax literalExpression = default;

            SyntaxToken predefinedType = new SyntaxToken();
            SyntaxTokenList modifiers = new SyntaxTokenList();
            SyntaxKind kind = default;
            TypeSyntax type = default;

            if (initializer.TypeName == "GDCallExpression")
            {
                var arguments = GetLiteralExpression(initializer).GetResultArgumentLiteralExpressionSyntax();

                var initializerNodes = initializer.Nodes.ToList();
                var methodNameIdentifier = ((GDIdentifierExpression)initializerNodes.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier;
                var methodNameText = methodNameIdentifier.ToString();//get_name

                if (methodNameText == "Vector2" || methodNameText == "Vector3" || methodNameText == "Vector4" || methodNameText == "Rect2" ||
                    methodNameText == "Vector2I" || methodNameText == "Vector3I" || methodNameText == "Vector4I" || methodNameText == "Rect2I" ||
                    methodNameText == "preload")
                {
                    type = GetTypeVariable(methodNameText, isThereColon, d.Type, isConst);
                    literalExpression = GetArgumentToMethodExpressionSyntax(methodNameIdentifier, arguments);
                    modifiers = GetModifier(methodNameText, isConst);
                    member = GetVariableDeclaration(identifier, type, literalExpression, modifiers);
                }
                else
                {
                    type = GetTypeVariable(methodNameText, isThereColon, d.Type, isConst);
                    modifiers = GetModifier(methodNameText, isConst);
                    member = GetVariableDeclaration(identifier, type, modifiers);

                    var stringLiteral = GetLiteralExpression(methodNameText);//Literal("get_name"));

                    //var callInvocation = InvocationExpression(IdentifierName("Call")).AddArgumentListArguments(Argument(stringLiteral2));
                    //var expressionStatement = ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifier), callInvocation));

                    AAAaaa(methodNameIdentifier);

                    var expressionStatement = GetInvocationExpressionStatement(identifier, stringLiteral, "Call", arguments.ToArray());

                    foreach (var c in _constructorCollection)
                        AddConstructor(c.Key, expressionStatement);

                    if (!_constructorCollection.ContainsKey(new ParameterListTKey()))
                        AddConstructor(new ParameterListTKey(), expressionStatement);
                }
            }
            else
            {
                switch (initializer.TypeName)
                {
                    case "GDStringExpression":
                        kind = SyntaxKind.StringKeyword;
                        break;
                    case "GDNumberExpression":
                        kind = GetSyntaxKind((GDNumberExpression)initializer);
                        break;
                    case "GDBoolExpression":
                        kind = SyntaxKind.BoolKeyword;
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }
                literalExpression = GetLiteralExpression(initializer).LiteralExpressionSyntax;
                type = GetTypeVariable("", isThereColon, d.Type, isConst, kind);
                modifiers = GetModifier("", isConst);
                member = GetVariableDeclaration(identifier, type, literalExpression, modifiers);
            }

            #region
            //switch (initializer.TypeName)
            //{
            //    case "GDStringExpression":
            //        type = GetTypeVariable(SyntaxKind.StringKeyword, d.Colon != null, d.Type);
            //        literalExpression = GetLiteralExpression(initializer).LiteralExpressionSyntax;
            //        member = GetVariableDeclaration(identifier, type, literalExpression, d.ConstKeyword != null);
            //        //member = GetMember(d, SyntaxKind.StringKeyword);
            //        break;
            //    case "GDNumberExpression":
            //        type = GetTypeVariable(GetSyntaxKind((GDNumberExpression)initializer), d.Colon != null, d.Type);
            //        literalExpression = GetLiteralExpression(initializer).LiteralExpressionSyntax;
            //        member = GetVariableDeclaration(identifier, type, literalExpression, d.ConstKeyword != null);
            //        //member = GetMember(d, GetSyntaxKind((GDNumberExpression)initializer));
            //        break;
            //    case "GDBoolExpression":
            //        type = GetTypeVariable(SyntaxKind.BoolKeyword, d.Colon != null, d.Type);
            //        literalExpression = GetLiteralExpression(initializer).LiteralExpressionSyntax;
            //        member = GetVariableDeclaration(identifier, type, literalExpression, d.ConstKeyword != null);
            //        //member = GetMember(d, SyntaxKind.BoolKeyword);
            //        break;
            //    case "GDCallExpression":
            //        var arguments = GetLiteralExpression(initializer).GetResultArgumentLiteralExpressionSyntax();

            //        var initializerNodes = initializer.Nodes.ToList();
            //        var methodNameIdentifier = ((GDIdentifierExpression)initializerNodes.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier;
            //        literalExpression = GetArgumentToMethodExpressionSyntax(methodNameIdentifier, arguments);

            //        var methodName = methodNameIdentifier.ToString();
            //        type = GetTypeVariable(methodName, d.Colon != null, d.Type);
            //        member = GetVariableDeclaration(identifier, type, literalExpression, d.ConstKeyword != null);
            //        break;
            //    default:
            //        break;
            //}
            #endregion

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

            List<ExpressionSyntax> literalExpressions = null;
            InitializerExpressionSyntax initializerExpression;
            SyntaxToken predefinedType = new SyntaxToken();

            FieldDeclarationSyntax collection = null;

            if (isDifferentTypes)
            {
                predefinedType = ParseToken("Godot.Variant"); //ToDo: выяснить, нужно ли убирать слово годот, так как есть юзинг годота в коде

                //collection = CreateField(identifier, predefinedType, CreateArrayInitializerString(allCollection));
                //ToDo дописать наполнение для collection в случае, если элементы коллекции имеют разные типы
                object b = new List<string>();

                foreach (var coll in allCollection)
                {
                    switch (coll.TypeName)
                    {
                        case "GDStringExpression":
                            literalExpressions.Add(GetLiteralExpression((GDStringExpression)coll));
                            break;
                        case "GDNumberExpression":
                            literalExpressions.Add(GetLiteralExpression((GDNumberExpression)coll));
                            break;
                        case "GDBoolExpression":
                            literalExpressions.Add(GetLiteralExpression(bool.Parse(coll.ToString())));
                            break;
                        case "GDCallExpression":
                            throw new NotImplementedException();
                            break;
                        default:
                            break;
                    }
                    initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpressions));
                }
            }
            else
            {
                //literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression(value).LiteralExpressionSyntax).ToList();

                switch (type)
                {
                    case "GDStringExpression":
                        collection = CreateArrayField(e, SyntaxKind.StringKeyword);
                        break;
                    case "GDNumberExpression":
                        var isDouble = allCollection.Any(x => GetSyntaxKind((GDNumberExpression)x) == SyntaxKind.DoubleKeyword);
                        var kind = isDouble ? SyntaxKind.DoubleKeyword : GetSyntaxKind((GDNumberExpression)allCollection[0]);
                        collection = CreateArrayField(e, kind);
                        break;
                    case "GDBoolExpression":
                        collection = CreateArrayField(e, SyntaxKind.BoolKeyword);
                        break;
                    case "GDCallExpression":
                        var expList = e.Nodes.ToList()[0];
                        var callExprList = expList.Nodes.ToList();

                        List<InvocationExpressionSyntax> invocationExpression = new List<InvocationExpressionSyntax>();

                        foreach (var callExprItem in callExprList)
                        {
                            //literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression(value).LiteralExpressionSyntax).ToList();

                            var arguments = new List<ArgumentSyntax>();
                            var ident = "";

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
                            #region
                            ////////
                            //var uuu = new GDIdentifier();
                            //var tt = p.Identifier.TryExtractLocalScopeVisibleDeclarationFromParents(out uuu);//Поиск соответствий по названию во всем коде
                            ////////

                            //var invocationExpression2 = CreateMethodInvocationExpression("Variant.CreateFrom", arguments);
                            #endregion

                            if (ident != null && arguments != null)
                            {
                                //var exp = CreateMethodObjectCreationExpression(ident, arguments);
                                //invocationExpression.Add(CreateVariantCreateFromMethodInvocationExpression(exp));

                                var ee = CreateMethodObjectCreationExpressionSyntax(ident);
                                var eee = ReplaceArgumentsInExpressionSyntax(ee, arguments);

                                var cc = CreateMethodInvocationExpressionSyntax("Variant", "CreateFrom");
                                var ll = AddArgumentToExpressionSyntax(cc, Argument(eee));
                                invocationExpression.Add(ll);
                            }
                        }
                        var expr = invocationExpression.Select(value => (ExpressionSyntax)value);
                        initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(expr));

                        var modifiers = GetModifier("", false);
                        collection = CreateArrayField(identifier, IdentifierName("Variant"), initializerExpression, modifiers);
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
        }

        public void Visit(GDMethodDeclaration d)
        {
        }

        public void Visit(GDExpressionStatement s)
        {
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