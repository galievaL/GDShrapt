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
                        args.Add(Argument(GetLiteralExpression(p, helper).LiteralExpressionSyntax)); //сделать сохранение в ArgumentLiteralExpressionSyntax

                    var methodExprSyntax = GetArgumentToMethodExpressionSyntax(ident, args);
                    helper.ArgumentLiteralExpressionSyntaxList.Add(Argument(methodExprSyntax));//сделать сохранение в ArgumentLiteralExpressionSyntax

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

            helper.LiteralExpressionSyntax = expr;//сделать сохранение в ArgumentLiteralExpressionSyntax

            return helper;
        }

        //ExpressionSyntax GetExpressionSyntax(GDIdentifier methodNameIdentifier)
        //{
        //    var methodName = methodNameIdentifier.ToString();

        //    var gdIdent = new GDIdentifier();

        //    if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
        //        methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
        //        return CreateMethodObjectCreationExpressionSyntax(methodName);
        //    else if (methodName == "preload")
        //        return CreateMethodInvocationExpressionSyntax("ResourceLoader", "Load");
        //    else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
        //    {
        //        //if (тип метода, которого мы нашли Variant)
        //        //      var cc = CreateMethodInvocationExpressionSyntax(methodName, arguments); //просто пишем исходный метод без нью
        //        //      literalExpression = AddArgumentToExpressionSyntax(cc, arguments);
        //        //else
        //        //literalExpression = CreateVariantCreateFromMethodInvocationExpression(arguments, "Variant", "CreateFrom");

        //        return CreateMethodInvocationExpressionSyntax("Variant", "CreateFrom");
        //    }
        //    return CreateMethodInvocationExpressionSyntax("Variant", "From");
        //}

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, List<ArgumentSyntax> arguments)
        {
            var methodName = methodNameIdentifier.ToString();
            ExpressionSyntax literalExpression = default;
            var gdIdent = new GDIdentifier();

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
            {
                literalExpression = ObjectCreationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodName == "preload")
            {
                literalExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("ResourceLoader"), IdentifierName("Load")))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
            {
                var gdIdentType = ((GDMethodDeclaration)gdIdent.Parent).ReturnType;

                var argLiteralExpression = InvocationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));

                literalExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("Variant"), IdentifierName("CreateFrom")))
                                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(argLiteralExpression))));
            }
            else
            {
                //var expressionSyntax = CreateMethodInvocationExpressionSyntax("Call");
                //literalExpression = ReplaceArgumentsInExpressionSyntax(expressionSyntax, arguments);

                literalExpression = InvocationExpression(IdentifierName("Call"))
                                    .AddArgumentListArguments(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(methodName))))
                                    .AddArgumentListArguments(arguments.ToArray());
            }

            return literalExpression;
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

        TypeSyntax GetTypeVariable(string methodName, bool isThereColon, GDType variableDeclarationType, bool isThereConst, SyntaxKind? kind = null)
        {
            if (methodName == "preload")
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else
                    return IdentifierName("Resource");
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
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

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, SyntaxTokenList modifiers, ExpressionSyntax literalExpression = null)
        {
            var nameDeclaration = VariableDeclarator(Identifier(nameVariable));

            if (literalExpression != null)
                nameDeclaration = nameDeclaration.WithInitializer(EqualsValueClause(literalExpression));

            return FieldDeclaration(VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration)))
                                        .WithModifiers(modifiers);
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