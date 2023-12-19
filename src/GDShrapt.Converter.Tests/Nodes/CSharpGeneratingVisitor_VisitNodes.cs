using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
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

        public void Visit(GDVariableDeclaration d)
        {
            var identifier = ValidateTypeAndNameHelper.GetValidateFieldName(d.Identifier.ToString(), ScopeType.Class);
            var initializer = d.Initializer;
            var isConst = d.ConstKeyword != null;
            var isThereColon = d.Colon != null;

            if (initializer.TypeName == "GDArrayInitializerExpression" || initializer.TypeName == "GDDictionaryInitializerExpression")
            {
                _codeUsings.Add(UsingDirective(ParseName("Godot.Collections")));
                return;
            }

            var member = default(MemberDeclarationSyntax);
            var leftPartType = default(GeneralType);
            var leftPartTypeIdentifier = default(TypeSyntax);
            var kind = default(GeneralType);
            var rightPart = default(ExpressionSyntax);
            var modifiers = new SyntaxTokenList();

            kind = GetAverageType(d.AllNodes.ToList());

            if (initializer.TypeName == "GDCallExpression")
            {
                var methodNameIdentifier = GetIdentifier((GDCallExpression)initializer);
                var methodNameText = methodNameIdentifier.ToString();
                modifiers = GetModifier(methodNameText, isConst, kind);

                leftPartType = GetTypeVariable(isThereColon, d.Type, isConst, methodNameIdentifier);
                leftPartTypeIdentifier = IdentifierName(leftPartType.ToString());
                var isVariantLeftPartType = (leftPartTypeIdentifier is IdentifierNameSyntax identSyntax) ? identSyntax.Identifier.Text == CustomSyntaxKind.Variant.ToString() : false;
                rightPart = GetLiteralExpression(initializer, isConst, isVariantLeftPartType);

                var containCallExpr = GetExpressionsList(initializer).Any(x => x.TypeName == "GDCallExpression");
                var isItStandartGodotType = ValidateTypeAndNameHelper.IsStandartGodotType(methodNameText);
                var isGodotFunctions = ValidateTypeAndNameHelper.IsGodotFunctions(ref methodNameText, out GeneralType type);

                if (isConst || !containCallExpr && (isItStandartGodotType || isGodotFunctions))
                    member = GetVariableFieldDeclaration(identifier, leftPartTypeIdentifier, modifiers, rightPart);
                else
                {
                    member = GetVariableFieldDeclaration(identifier, leftPartTypeIdentifier, modifiers);

                    var expr = ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifier), rightPart));
                    AddToAllExistingConstructors(expr);
                    AddConstructor(new ParameterListTKey(), expr);
                }
            }
            else if (initializer.TypeName == "GDDualOperatorExpression")
            {
                leftPartType = GetTypeVariable(isThereColon, d.Type, isConst, averageType: kind);
                leftPartTypeIdentifier = IdentifierName(leftPartType.ToString());
                modifiers = GetModifier(isConst: isConst, averageType: kind);

                var node = initializer.Nodes.Where(x => x.TypeName == "GDCallExpression" && GetIdentifier((GDCallExpression)x).TryExtractLocalScopeVisibleDeclarationFromParents(out GDIdentifier gdIdent)).FirstOrDefault();

                if (node != null)
                {
                    member = GetVariableFieldDeclaration(identifier, leftPartTypeIdentifier, modifiers);

                    var isVariantLeftPartType = (leftPartTypeIdentifier is IdentifierNameSyntax identSyntax) ? identSyntax.Identifier.Text == CustomSyntaxKind.Variant.ToString() : false;
                    rightPart = GetLiteralExpression(initializer, isConst, isVariantLeftPartType, ScopeType.Class);
                    var expr = ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifier), rightPart));

                    AddToAllExistingConstructors(expr);
                    AddConstructor(new ParameterListTKey(), expr);
                }
                else
                {
                    rightPart = GetLiteralExpression(initializer, isConst, scopeType: ScopeType.Class);
                    member = GetVariableFieldDeclaration(identifier, leftPartTypeIdentifier, modifiers, rightPart);
                }
            }
            else
            {
                leftPartType = GetTypeVariable(isThereColon, d.Type, isConst, averageType: kind);
                leftPartTypeIdentifier = IdentifierName(leftPartType.ToString());
                modifiers = GetModifier(isConst: isConst, averageType: kind);
                rightPart = GetLiteralExpression(initializer, isConst, scopeType: ScopeType.Class);
                
                member = GetVariableFieldDeclaration(identifier, leftPartTypeIdentifier, modifiers, rightPart);
            }

            if (member != null)
                _partsCode = _partsCode.AddMembers(member);

            _classVariableList.Add(identifier, leftPartType);
        }

        public void Visit(GDArrayInitializerExpression e)
        {
            _codeUsings.Add(UsingDirective(NameEquals("Array"), ParseName("Godot.Collections.Array")));

            var member = GetCollectionMember(e, "Array", ScopeType.Class);

            _partsCode = _partsCode.AddMembers(member);
        }

        public void Visit(GDDictionaryInitializerExpression e)
        {
            _codeUsings.Add(UsingDirective(NameEquals("Dictionary"), ParseName("Godot.Collections.Dictionary")));

            var member = GetCollectionMember(e, "Dictionary", ScopeType.Class);

            _partsCode = _partsCode.AddMembers(member);
        }

        public void Visit(GDMethodDeclaration d)
        {
            var methodName = ValidateTypeAndNameHelper.GetValidateFieldName(d.Identifier.ToString(), ScopeType.MethodName);

            var member = default(MethodDeclarationSyntax);

            var parameterSyntax = new List<ParameterSyntax>();
            var args = new List<StatementSyntax>();

            foreach (var methodDeclarationNode in d.Nodes.ToList())
            {
                if (methodDeclarationNode.TypeName == "GDParametersList")
                {
                    var parametersList = ((GDParametersList)methodDeclarationNode).Nodes.ToList();

                    foreach (var par in parametersList)
                    {
                        var parameter = (GDParameterDeclaration)par;
                        var ident = parameter.Identifier.ToString();

                        //var type = parameter.Type?.ToString() ?? CustomSyntaxKind.Variant.ToString();
                        var type = GetTypeVariable(parameter.Type?.ToString());
                        parameterSyntax.Add(Parameter(Identifier(ident)).WithType(IdentifierName(type.ToString())));

                        _classVariableList.Add(ident, GeneralTypeHelper.GetGeneralType(type.ToString()));
                    }
                }
                else if (methodDeclarationNode.TypeName == "GDExpressionsList")
                {

                }
                else if (methodDeclarationNode.TypeName == "GDStatementsList")
                {
                    var exprStatementList = methodDeclarationNode.Nodes.ToList();

                    foreach(var exprStatem in exprStatementList)
                    {
                        if (exprStatem.TypeName == "GDExpressionStatement")
                        {
                            foreach (var expr in exprStatem.Nodes.ToList())
                            {
                                if (expr.TypeName == "GDReturnExpression")
                                {
                                    var returnExpressions = expr.Nodes.ToList();

                                    foreach (var returnExpression in returnExpressions)
                                    {
                                        var type = IsClassVariable("");

                                        //if (type != null)
                                        //{
                                            
                                        //}

                                        //var averageType = ConvertMyTypeToTuple(GetAverageType(returnExpressions));
                                        var averageType = GetAverageType(returnExpressions);

                                        var isVariantLeftPartType = averageType != null && averageType.CustomType == CustomSyntaxKind.Variant;

                                        args.Add(ReturnStatement(GetLiteralExpression(returnExpression, isVariantLeftPartType: isVariantLeftPartType, scopeType:ScopeType.LocalVariable))); 
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    throw new NotImplementedException();
            }

            member = GetMethodDeclaration(d.ReturnType, methodName, parameterSyntax.ToArray(), args);

            _methodsPartsCode.Add(member);
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