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
        ExpressionSyntaxHelper GetLiteralExpression(GDNode node, ExpressionSyntaxHelper helper = null)
        {
            LiteralExpressionSyntax expr = null;

            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDCallExpression":
                    //"GDCallExpression 'Vector2(10, 20)'" => {"GDIdentifierExpression 'Vector2'", "GDExpressionsList '10, 20'"} => "GDExpressionsList '10, 20'"
                    var exprList = node?.Nodes?.Where(x => x.TypeName == "GDExpressionsList").FirstOrDefault();
                    //{"GDNumberExpression '10'", "GDNumberExpression '20'"}
                    var nodes = exprList?.Nodes?.ToList();
                    helper = helper ?? new ExpressionSyntaxHelper(new List<ArgumentSyntax>());

                    var ident = ((GDIdentifierExpression)node?.Nodes?.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier.ToString();
                    //var inv = GetExpressionSyntax(ident);

                    foreach (var n in nodes)
                    {
                        helper.AddArgumentLiteralExpressionSyntax(Argument(GetLiteralExpression(n, helper).LiteralExpressionSyntax));
                    }
                    var dd = GetArgumentToMethodExpressionSyntax(ident, helper.GetArgumentLiteralExpressionSyntax());
                    helper.AddArgumentLiteralExpressionSyntax(Argument(dd));

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

            expr = expr ?? throw new NotImplementedException();
            helper = helper ?? new ExpressionSyntaxHelper(expr);

            helper.LiteralExpressionSyntax = expr;

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
                //if (тип метода, которого мы нашли Variant)
                //      var cc = CreateMethodInvocationExpressionSyntax(methodName, arguments); //просто пишем исходный метод без нью
                //      literalExpression = AddArgumentToExpressionSyntax(cc, arguments);
                //else
                //literalExpression = CreateVariantCreateFromMethodInvocationExpression(arguments, "Variant", "CreateFrom");

                var expressionSyntax = CreateMethodInvocationExpressionSyntax(methodName);
                var argLiteralExpression = Argument(ReplaceArgumentsInExpressionSyntax(expressionSyntax, arguments));

                var expressionSyntax2 = CreateMethodInvocationExpressionSyntax("Variant", "CreateFrom");
                literalExpression = AddArgumentToExpressionSyntax(expressionSyntax2, argLiteralExpression);
            }
            else
            {
                var expressionSyntax = CreateMethodInvocationExpressionSyntax(methodName);
                var argLiteralExpression = Argument(ReplaceArgumentsInExpressionSyntax(expressionSyntax, arguments));

                var expressionSyntax2 = CreateMethodInvocationExpressionSyntax("Variant", "From");
                literalExpression = AddArgumentToExpressionSyntax(expressionSyntax2, argLiteralExpression);


            }

            return literalExpression;
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax2(GDIdentifier methodNameIdentifier, List<ArgumentSyntax> arguments)
        {
            var methodName = methodNameIdentifier.ToString();
            ExpressionSyntax literalExpression = default;
            var gdIdent = new GDIdentifier();

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
            {
                literalExpression = ObjectCreationExpression(IdentifierName(methodName)).WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodName == "preload")
            {
                literalExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("ResourceLoader"), IdentifierName("Load")))
                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
            {
                #region
                //if (тип метода, которого мы нашли Variant)
                //      var cc = CreateMethodInvocationExpressionSyntax(methodName, arguments); //просто пишем исходный метод без нью
                //      literalExpression = AddArgumentToExpressionSyntax(cc, arguments);
                //else
                //literalExpression = CreateVariantCreateFromMethodInvocationExpression(arguments, "Variant", "CreateFrom");

                //var argLiteralExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(methodName), IdentifierName("CreateFrom")))
                //    .WithArgumentList(ArgumentList(SeparatedList(arguments)));

                //literalExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("Variant"), IdentifierName("CreateFrom")))
                //    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(argLiteralExpression))));
                #endregion
            }
            else
            {
                var argLiteralExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(methodName), IdentifierName("CreateFrom")))
                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));

                literalExpression = InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("Variant"), IdentifierName("From")))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(argLiteralExpression))));
            }
            return literalExpression;
        }

        //string AdaptationToStandartMethods(string methodName)
        //{
        //    switch (methodName)
        //    {
        //        case "Vector2i":
        //            return "Vector2I";
        //        case "Vector3i":
        //            return "Vector3I";
        //        case "Vector4i":
        //            return "Vector4I";
        //        case "Rect2i":
        //            return "Rect2I";
        //        default:
        //            return ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName);
        //    }
        //}

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

        //FieldDeclarationSyntax GetMember(GDVariableDeclaration declaration, SyntaxKind kind)
        //{
        //    var literalExpression = GetLiteralExpression(declaration.Initializer).LiteralExpressionSyntax;
        //    var identifier = declaration.Identifier.ToString();

        //    var vv = GetTypeVariable(kind, declaration.Colon != null, declaration.Type);

        //    return GetVariableDeclaration(identifier, PredefinedType(Token(kind)), literalExpression, declaration.ConstKeyword != null);
        //}

        TypeSyntax GetTypeVariable(string methodName, bool isThereColon, GDType variableDeclarationType, bool isThereConst, SyntaxKind? kind = null)
        {
            if (methodName == "preload")
                return IdentifierName("Resource");
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else if (kind != null)
                    return PredefinedType(Token(kind.Value));
                else if (ValidateTypeAndNameHelper.IsItGodotType(methodName) || ValidateTypeAndNameHelper.IsItSharpType(methodName))
                {
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName));
                }
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
                if (methodName == "preload")
                    return TokenList(Token(accessModifier), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword));
                else
                    return TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword));
            }
            else
                return TokenList(Token(accessModifier));
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, ExpressionSyntax literalExpression, SyntaxTokenList modifiers)
        {
            var nameDeclaration = VariableDeclarator(Identifier(nameVariable)).WithInitializer(EqualsValueClause(literalExpression));

            return FieldDeclaration(VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration)))
                                        .WithModifiers(modifiers);
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, SyntaxTokenList modifiers)
        {
            //var tt = FieldDeclaration(VariableDeclaration(tt).AddVariables(VariableDeclarator("Name")))
            //                .AddModifiers(Token(SyntaxKind.PublicKeyword));

            var nameDeclaration = VariableDeclarator(Identifier(nameVariable));

            return FieldDeclaration(VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration)))
                                        .WithModifiers(modifiers);
        }

        InvocationExpressionSyntax CreateMethodInvocationExpressionSyntax(string expression = "Variant", string name = "CreateFrom")
        {
            return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(expression), IdentifierName(name)));
        }

        ObjectCreationExpressionSyntax CreateMethodObjectCreationExpressionSyntax(string methodName)
        {
            return ObjectCreationExpression(IdentifierName(methodName));
        }

        InvocationExpressionSyntax AddArgumentToExpressionSyntax(InvocationExpressionSyntax expressionSyntax, ArgumentSyntax argument)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SingletonSeparatedList(argument)));
        }

        InvocationExpressionSyntax AddArgumentToExpressionSyntax(InvocationExpressionSyntax expressionSyntax, List<ArgumentSyntax> arguments)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        InvocationExpressionSyntax ReplaceArgumentsInExpressionSyntax(InvocationExpressionSyntax expressionSyntax, ArgumentSyntax argument)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SingletonSeparatedList(argument)));
        }

        InvocationExpressionSyntax ReplaceArgumentsInExpressionSyntax(InvocationExpressionSyntax expressionSyntax, List<ArgumentSyntax> arguments)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        ObjectCreationExpressionSyntax ReplaceArgumentsInExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, ArgumentSyntax argument)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SingletonSeparatedList(argument)));
        }

        ObjectCreationExpressionSyntax ReplaceArgumentsInExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, List<ArgumentSyntax> arguments)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }
    }
}