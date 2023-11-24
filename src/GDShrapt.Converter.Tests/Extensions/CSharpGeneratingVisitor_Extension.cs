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

        ArgumentSyntax GetArgumentSyntaxExpression(GDNode node, ArgumentSyntax argumentsFactory = null)
        {
            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDCallExpression":
                    var ident = GetIdentifierExpression(node);
                    var parameters = GetExpressionsList(node);
                    var args = new List<ArgumentSyntax>();

                    foreach (var p in parameters)
                        args.Add(GetArgumentSyntaxExpression(p, argumentsFactory));

                    return Argument(GetArgumentToMethodExpressionSyntax(ident, args));
                case "GDStringExpression":
                    return Argument(GetLiteralExpression((GDStringExpression)node));
                case "GDNumberExpression":
                    return Argument(GetLiteralExpression((GDNumberExpression)node));
                case "GDBoolExpression":
                    return Argument(GetLiteralExpression(bool.Parse(node.ToString())));
                case "GDIdentifierExpression":
                    var identif = ((GDIdentifierExpression)node).Identifier.ToString();
                    ValidateTypeAndNameHelper.IsItGodotFunctions(ref identif, out string type);
                    return Argument(IdentifierName(identif));
                default:
                    throw new NotImplementedException();
            }
        }

        ExpressionSyntax GetLiteralExpression(GDNode node, ArgumentSyntax argumentsFactory = null)
        {
            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDCallExpression":
                    var ident = GetIdentifierExpression(node);
                    var parameters = GetExpressionsList(node);
                    var args = new List<ExpressionSyntax>();

                    foreach (var p in parameters)
                        args.Add(GetLiteralExpression(p, argumentsFactory));

                    return GetArgumentToMethodExpressionSyntax(ident, args);
                case "GDDualOperatorExpression":
                    var operatorType = GetCSharpDualOperatorType(((GDDualOperatorExpression)node).OperatorType);
                    var nodes = node.Nodes.ToList();

                    return BinaryExpression(operatorType, GetLiteralExpression(nodes[0]), GetLiteralExpression(nodes[1]));
                case "GDStringExpression":
                    return GetLiteralExpression((GDStringExpression)node);
                case "GDNumberExpression":
                    return GetLiteralExpression((GDNumberExpression)node);
                case "GDBoolExpression":
                    return GetLiteralExpression(bool.Parse(node.ToString()));
                case "GDIdentifierExpression":
                    var identif = ((GDIdentifierExpression)node).Identifier.ToString();
                    ValidateTypeAndNameHelper.IsItGodotFunctions(ref identif, out string type);
                    return IdentifierName(identif);
                default:
                    throw new NotImplementedException();
            }
        }

        ExpressionSyntax GetBinaryExpression(GDNode node, ArgumentSyntax argumentsFactory = null)
        {
            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDDualOperatorExpression":
                    var operatorType = GetCSharpDualOperatorType(((GDDualOperatorExpression)node).OperatorType);
                    var nodes = node.Nodes.ToList();

                    return BinaryExpression(operatorType, GetBinaryExpression(nodes[0]), GetBinaryExpression(nodes[1]));
                case "GDCallExpression":
                case "GDStringExpression":
                case "GDNumberExpression":
                case "GDBoolExpression":
                case "GDIdentifierExpression":
                    return GetLiteralExpression(node);
                default:
                    throw new NotImplementedException();
            }
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, List<ExpressionSyntax> arguments)
        {
            var args = arguments.Select(x => Argument(x)).ToList();

            return GetArgumentToMethodExpressionSyntax(methodNameIdentifier, args);
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, List<ArgumentSyntax> arguments)
        {
            var methodName = methodNameIdentifier.ToString();

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
            {
                return ObjectCreationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (ValidateTypeAndNameHelper.IsItGodotFunctions(ref methodName, out string type))
            {
                return InvocationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out GDIdentifier gdIdent))
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

            if (ValidateTypeAndNameHelper.IsItGodotFunctions(ref methodName, out string type))
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else
                    return IdentifierName(type);
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else if (isThereColon && methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent) && ((GDMethodDeclaration)gdIdent.Parent).ReturnType != null)
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(((GDMethodDeclaration)gdIdent.Parent).ReturnType.ToString()));
                else if (kind != null && kind != SyntaxKind.None)
                    return PredefinedType(Token(kind.Value));
                else if (ValidateTypeAndNameHelper.IsItGodotType(methodName))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName));
                else
                    return IdentifierName("Variant");
            }
            else
                return IdentifierName("Variant");
        }

        SyntaxTokenList GetModifier(string methodName = "", bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            if (isConst)
            {
                if (ValidateTypeAndNameHelper.IsItGodotType(methodName) || ValidateTypeAndNameHelper.IsItGodotFunctions(ref methodName, out string type))
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