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
        FieldDeclarationSyntax CreateArrayField(GDArrayInitializerExpression expression, SyntaxKind kind, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var allCollection = expression?.Values?.Nodes?.ToList();
            var parent = (GDVariableDeclaration)expression.Parent;
            var identifier = parent?.Identifier?.ToString();

            var literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression(value).LiteralExpressionSyntax).ToList();
            var initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpressions));

            var modifiers = GetModifier("", false);
            return CreateArrayField(identifier, PredefinedType(Token(kind)), initializerExpression, modifiers);
        }

        FieldDeclarationSyntax CreateArrayField(string name, TypeSyntax predefinedType, InitializerExpressionSyntax initializer, SyntaxTokenList modifiers)
        {
            var arrayType = ArrayType(predefinedType)
                    .WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));

            return GetVariableDeclaration(name, arrayType, initializer, modifiers);
        }

        ConstructorDeclarationSyntax GetConstructorDeclaration(List<ParameterSyntax> parametrs = null, SyntaxKind accessModifier = SyntaxKind.PublicKeyword, params ExpressionStatementSyntax[] expressionStatementSyntaxes)
        {
            var constrDecl = ConstructorDeclaration(_className)
                            .WithBody(Block(expressionStatementSyntaxes))
                            .AddModifiers(Token(accessModifier));

            if (parametrs != null)
                constrDecl.AddParameterListParameters(parametrs.ToArray());

            return constrDecl;
        }

        void AddConstructor(ParameterListTKey key, ExpressionStatementSyntax expressionStatement)
        {
            if (_constructorCollection.ContainsKey(key))
                _constructorCollection[key].Add(expressionStatement);
            else
                _constructorCollection.Add(key, new List<ExpressionStatementSyntax> { expressionStatement });
        }

        void AddConstructor(ParameterListTKey key, List<ExpressionStatementSyntax> expressionsStatement)
        {
            if (_constructorCollection.ContainsKey(key))
            {
                var value = _constructorCollection[key].Concat(expressionsStatement).ToList();
                _constructorCollection[key] = value;
            }
            else
                _constructorCollection.Add(key, expressionsStatement);
        }

        ExpressionStatementSyntax GetObjectCreationExpressionStatement(string identifierType, TypeSyntax initializerType, params ArgumentSyntax[] argumentsOfInitializer)
        {
            //var callInvocation = InvocationExpression(IdentifierName("Call")).AddArgumentListArguments(Argument(GetLiteralExpression(methodNameText)));
            //ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName("Name"), callInvocation))

            //identifierType = "Name";
            //initializerType = IdentifierName("Call");
            //argumentsOfInitializer = Argument(GetLiteralExpression(methodNameText));

            var objectCreationExpression = ObjectCreationExpression(initializerType).AddArgumentListArguments(argumentsOfInitializer);

            return ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifierType), objectCreationExpression));
        }
        ExpressionStatementSyntax GetInvocationExpressionStatement(string identifierType, LiteralExpressionSyntax methodName, string initializerType = "Call", params ArgumentSyntax[] argumentsOfInitializer)
        {
            var objectCreationExpression = InvocationExpression(IdentifierName(initializerType))
                        .AddArgumentListArguments(Argument(methodName))
                        .AddArgumentListArguments(argumentsOfInitializer);

            return ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(identifierType), objectCreationExpression));
        }
    }
}
