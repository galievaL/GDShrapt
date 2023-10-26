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
        public string GetValidClassName(string className)
        {
            if (className[0] != '_' && !char.IsLetter(className[0]))
                className = '_' + className;

            className = Regex.Replace(className, "[^a-zA-Zа-яА-Я0-9_]", string.Empty);

            return className;
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, string returnValue, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var typeVariable = SyntaxKind.StringKeyword;
            var returnValueType = SyntaxKind.StringLiteralExpression;
            var literalExpression = LiteralExpression(returnValueType, Literal(returnValue));

            return GetVariableDeclarationWithPredefinedType(nameVariable, typeVariable, literalExpression, isConst, accessModifier);
        }

        LiteralExpressionSyntax GetLiteralExpression(GDNumberExpression numberExpression, GDNumberType? type = null)
        {
            SyntaxToken literal;
            var number = numberExpression.Number;

            switch (type ?? number.ResolveNumberType())
            {
                case GDNumberType.LongDecimal:
                case GDNumberType.LongBinary:
                case GDNumberType.LongHexadecimal:
                    literal = Literal(number.ValueInt64);
                    break;
                case GDNumberType.Double:
                    literal = Literal(number.ValueDouble);
                    break;
                case GDNumberType.Undefined:
                    literal = Literal(0);
                    //TODO: add a comment output with the actual value
                    break;
                default:
                    literal = Literal(0);
                    break;
            }

            return LiteralExpression(SyntaxKind.NumericLiteralExpression, literal);
        }

        LiteralExpressionSyntax GetLiteralExpression(GDStringExpression stringExpression)
        {
            return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(stringExpression.String.Value));
        }

        LiteralExpressionSyntax GetLiteralExpression(bool boolExpression)
        {
            return LiteralExpression(boolExpression ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
        }

        SyntaxKind GetSyntaxKind(GDNumberExpression numberExpression)
        {
            var number = numberExpression.Number;

            switch (number.ResolveNumberType())
            {
                case GDNumberType.LongDecimal:
                case GDNumberType.LongBinary:
                case GDNumberType.LongHexadecimal:
                    return SyntaxKind.LongKeyword;
                case GDNumberType.Double:
                    return SyntaxKind.DoubleKeyword;
                case GDNumberType.Undefined:
                    return SyntaxKind.LongKeyword;
                //TODO: add a comment output with the actual value
                default:
                    return SyntaxKind.LongKeyword;
            }
        }

        FieldDeclarationSyntax CreateArrayField(string name, TypeSyntax predefinedType, InitializerExpressionSyntax initializer)
        {
            var arrayType = ArrayType(predefinedType)
                    .WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));

            return FieldDeclaration(VariableDeclaration(arrayType).WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name)).WithInitializer(EqualsValueClause(initializer)))))
                        .AddModifiers(Token(SyntaxKind.PublicKeyword));
        }

        public FieldDeclarationSyntax GetVariableDeclarationWithPredefinedType(string nameVariable, SyntaxKind typeVariable, ExpressionSyntax literalExpression, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            return GetVariableDeclaration(nameVariable, PredefinedType(Token(typeVariable)), literalExpression, isConst, accessModifier);
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, ExpressionSyntax literalExpression, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var nameDeclaration = VariableDeclarator(Identifier(nameVariable)).WithInitializer(EqualsValueClause(literalExpression));

            var tokenList = isConst ? TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword)) : TokenList(Token(accessModifier));

            var field = FieldDeclaration(VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration)))
                                        .WithModifiers(tokenList);
            return field;
        }

        ObjectCreationExpressionSyntax CreateMethodObjectCreationExpression(string identifierName, List<ArgumentSyntax> arguments)
        {
            return ObjectCreationExpression(IdentifierName(identifierName))
                        .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        InvocationExpressionSyntax CreateMethodInvocationExpression(string methodName, List<ArgumentSyntax> arguments)
        {
            return InvocationExpression(IdentifierName(methodName))
                        .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        InvocationExpressionSyntax CreateVariantCreateFromMethodInvocationExpression(ExpressionSyntax literalExpression, string expression = "Variant", string name = "CreateFrom")
        {
            return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(expression), IdentifierName(name)))
                        .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(literalExpression))));
        }

        InvocationExpressionSyntax CreateVariantCreateFromMethodInvocationExpression(List<ArgumentSyntax> arguments, string expression = "Variant", string name = "CreateFrom")
        {
            return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(expression), IdentifierName(name)))
                        .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }
    }
}