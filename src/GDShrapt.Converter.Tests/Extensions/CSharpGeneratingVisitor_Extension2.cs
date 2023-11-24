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
        MethodDeclarationSyntax GetMethodDeclaration(string methodType, string methodName, SyntaxTokenList modifiers, params StatementSyntax[] statements)
        {
            return MethodDeclaration(IdentifierName(methodType), methodName)
                        .WithModifiers(modifiers)
                        .WithBody(Block(statements));
        }

        FieldDeclarationSyntax CreateArrayField(GDArrayInitializerExpression expression, SyntaxKind kind, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var allCollection = expression?.Values?.Nodes?.ToList();
            var parent = (GDVariableDeclaration)expression.Parent;
            var identifier = parent?.Identifier?.ToString();

            var literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetArgumentSyntaxExpression(value).Expression).ToList();
            var initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpressions));

            var modifiers = GetModifier();
            return CreateArrayField(identifier, PredefinedType(Token(kind)), initializerExpression, modifiers);
        }

        FieldDeclarationSyntax CreateArrayField(string name, TypeSyntax predefinedType, InitializerExpressionSyntax initializer, SyntaxTokenList modifiers)
        {
            var arrayType = ArrayType(predefinedType)
                    .WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));

            return GetVariableFieldDeclaration(name, arrayType, modifiers, initializer);
        }
    }
}
