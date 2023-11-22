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
        GDIdentifier GetIdentifierExpression(GDNode node) 
            => ((GDIdentifierExpression)node?.Nodes?.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier;
        
        List<GDNode> GetExpressionsList(GDNode node) 
            => node?.Nodes?.Where(x => x.TypeName == "GDExpressionsList").FirstOrDefault()?.Nodes?.ToList();
        
        ExpressionSyntaxHelper GetLiteralExpression(GDNode node, ExpressionSyntaxHelper helper = null)
        {
            LiteralExpressionSyntax expr = null;

            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDCallExpression":
                    var ident = GetIdentifierExpression(node);
                    var parameters = GetExpressionsList(node);
                    helper = helper ?? new ExpressionSyntaxHelper(new List<ArgumentSyntax>());

                    List<ArgumentSyntax> args = new List<ArgumentSyntax>();

                    foreach (var p in parameters)
                        args.Add(GetLiteralExpression(p, helper).ArgumentLiteralExpressionSyntax);

                    helper.ArgumentLiteralExpressionSyntax = Argument(GetArgumentToMethodExpressionSyntax(ident, args));

                    return helper;
                case "GDStringExpression":
                    expr = GetLiteralExpression((GDStringExpression)node);
                    break;
                case "GDNumberExpression":
                    expr = GetLiteralExpression((GDNumberExpression)node);
                    break;
                case "GDBoolExpression":
                    expr = GetLiteralExpression(bool.Parse(node.ToString()));
                    break;
                default:
                    throw new NotImplementedException();
            }
            helper = helper ?? new ExpressionSyntaxHelper();

            helper.ArgumentLiteralExpressionSyntax = Argument(expr);

            return helper;
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, List<ArgumentSyntax> arguments)
        {
            var methodName = methodNameIdentifier.ToString();
            var gdIdent = new GDIdentifier();

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
            {
                return ObjectCreationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodName == "preload")
            {
                return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("ResourceLoader"), IdentifierName("Load")))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
            {
                methodName = ValidateTypeAndNameHelper.GetValidateFieldName(methodName);

                return InvocationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else
            {
                methodName = ValidateTypeAndNameHelper.GetValidateFieldName(methodName);

                return InvocationExpression(IdentifierName("Call"))
                                    .AddArgumentListArguments(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(methodName))))
                                    .AddArgumentListArguments(arguments.ToArray());
            }
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

        LiteralExpressionSyntax GetLiteralExpression(string stringName)
        {
            return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(stringName));
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

        TypeSyntax GetTypeVariable(GDIdentifier methodNameIdentifier, bool isThereColon, GDType variableDeclarationType, bool isThereConst, SyntaxKind? kind = null)
        {
            var methodName = methodNameIdentifier.ToString();
            var gdIdent = new GDIdentifier();

            if (methodName == "preload")
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else
                    return IdentifierName("Resource");
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else if (isThereColon && methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent) && ((GDMethodDeclaration)gdIdent.Parent).ReturnType != null)
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(((GDMethodDeclaration)gdIdent.Parent).ReturnType.ToString()));
                else if (kind != null)
                    return PredefinedType(Token(kind.Value));
                else if (ValidateTypeAndNameHelper.IsItGodotType(methodName))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName));
                else
                    return IdentifierName("Variant");
            }
            else
                return IdentifierName("Variant");
        }

        SyntaxTokenList GetModifier(string methodName, bool isConst, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            if (isConst)
            {
                if (ValidateTypeAndNameHelper.IsItGodotType(methodName) || methodName == "preload")
                    return TokenList(Token(accessModifier), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword));
                else
                    return TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword));
            }
            else
                return TokenList(Token(accessModifier));
        }

        VariableDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, ExpressionSyntax literalExpression = null)
        {
            var nameDeclaration = VariableDeclarator(Identifier(nameVariable));

            if (literalExpression != null)
                nameDeclaration = nameDeclaration.WithInitializer(EqualsValueClause(literalExpression));

            return VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration));
        }

        public FieldDeclarationSyntax GetVariableFieldDeclaration(string nameVariable, TypeSyntax typeVariable, SyntaxTokenList modifiers, ExpressionSyntax literalExpression = null)
        {
            return FieldDeclaration(GetVariableDeclaration(nameVariable, typeVariable, literalExpression))
                                        .WithModifiers(modifiers);
        }

        LocalDeclarationStatementSyntax GetLocalDeclarationStatement(string nameVariable, TypeSyntax typeVariable, ExpressionSyntax literalExpression = null)
        {
            return LocalDeclarationStatement(GetVariableDeclaration(nameVariable, typeVariable, literalExpression));
        }

        ExpressionSyntax AddArgumentToExpressionSyntax2(ExpressionSyntax expressionSyntax, params ArgumentSyntax[] arguments)
        {
            if (expressionSyntax is ObjectCreationExpressionSyntax ob)
                return ob.AddArgumentListArguments(arguments);
            else if (expressionSyntax is InvocationExpressionSyntax inv)
                return inv.AddArgumentListArguments(arguments);
            else
                throw new NotImplementedException();
        }

        ExpressionSyntax ReplaceArgumentsInExpressionSyntax2(ExpressionSyntax expressionSyntax, params ArgumentSyntax[] arguments)
        {
            var argList = (arguments.Length == 0) ? SingletonSeparatedList(arguments.FirstOrDefault()) : SeparatedList(arguments.ToList());

            if (expressionSyntax is ObjectCreationExpressionSyntax ob)
                return ob.WithArgumentList(ArgumentList(argList));
            else if (expressionSyntax is InvocationExpressionSyntax inv)
                return inv.WithArgumentList(ArgumentList(argList));
            else
                throw new NotImplementedException();
        }
    }
}