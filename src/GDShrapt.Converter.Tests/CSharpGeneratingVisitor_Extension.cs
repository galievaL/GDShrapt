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
                    helper = helper ?? new ExpressionSyntaxHelper(new List<LiteralExpressionSyntax>());

                    var ident = ((GDIdentifierExpression)node?.Nodes?.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier.ToString();
                    var inv = GetExpressionSyntax(ident);

                    foreach (var n in nodes)
                    {
                        //helper.LiteralExpressionSyntaxesList.Add(GetLiteralExpression(n, helper).LiteralExpressionSyntax);
                        helper.ArgumentLiteralExpressionSyntax.Add(Argument(GetLiteralExpression(n, helper).LiteralExpressionSyntax));
                    }
                    //var dd = GetArgumentToMethodExpressionSyntax(ident, helper.ArgumentLiteralExpressionSyntax);
                    //helper.ArgumentLiteralExpressionSyntax.Add(Argument(dd));

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

        ExpressionSyntax GetExpressionSyntax(GDIdentifier methodNameIdentifier)
        {
            var methodName = methodNameIdentifier.ToString();

            var gdIdent = new GDIdentifier();

            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" ||
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
                return CreateMethodObjectCreationExpressionSyntax(methodName);
            else if (methodName == "preload")
                return CreateMethodInvocationExpressionSyntax("ResourceLoader", "Load");
            else if (methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent))
            {
                //if (тип метода, которого мы нашли Variant)
                //      var cc = CreateMethodInvocationExpressionSyntax(methodName, arguments); //просто пишем исходный метод без нью
                //      literalExpression = AddArgumentToExpressionSyntax(cc, arguments);
                //else
                //literalExpression = CreateVariantCreateFromMethodInvocationExpression(arguments, "Variant", "CreateFrom");

                return CreateMethodInvocationExpressionSyntax("Variant", "CreateFrom");
            }
            return CreateMethodInvocationExpressionSyntax("Variant", "From");
        }

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

                var expressionSyntax = CreateMethodInvocationExpressionSyntax("Variant", "CreateFrom");
                literalExpression = AddArgumentToExpressionSyntax(expressionSyntax, arguments);
            }
            else
            {
                var expressionSyntax = CreateMethodInvocationExpressionSyntax("Variant", "From");
                literalExpression = AddArgumentToExpressionSyntax(expressionSyntax, arguments);
            }

            return literalExpression;
        }

        NameSyntax GetTypeVariable(string methodName)
        {
            if (methodName == "Vector2" || methodName == "Vector3" || methodName == "Vector4" || methodName == "Rect2" || 
                methodName == "Vector2I" || methodName == "Vector3I" || methodName == "Vector4I" || methodName == "Rect2I")
                return IdentifierName(methodName);
            else if (methodName == "preload")
                return IdentifierName("GodotObject");

            return IdentifierName("Variant");
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

        FieldDeclarationSyntax GetMember(GDVariableDeclaration declaration, SyntaxKind kind)
        {
            var literalExpression = GetLiteralExpression(declaration.Initializer).LiteralExpressionSyntax;
            var identifier = declaration.Identifier.ToString();

            return GetVariableDeclaration(identifier, PredefinedType(Token(kind)), literalExpression, declaration.ConstKeyword != null);
        }

        //FieldDeclarationSyntax GetMember(GDArrayInitializerExpression expression, SyntaxKind kind)
        //{
        //    var allCollection = expression?.Values?.Nodes?.ToList();
        //    var parent = (GDVariableDeclaration)expression.Parent;
        //    var identifier = parent?.Identifier?.ToString();

        //    var literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression(value).LiteralExpressionSyntax).ToList();
        //    var initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpressions));

        //    return CreateArrayField(identifier, PredefinedType(Token(kind)), initializerExpression);
        //}

        FieldDeclarationSyntax CreateArrayField(GDArrayInitializerExpression expression, SyntaxKind kind, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var allCollection = expression?.Values?.Nodes?.ToList();
            var parent = (GDVariableDeclaration)expression.Parent;
            var identifier = parent?.Identifier?.ToString();

            var literalExpressions = allCollection.Select(value => (ExpressionSyntax)GetLiteralExpression(value).LiteralExpressionSyntax).ToList();
            var initializerExpression = InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(literalExpressions));

            return CreateArrayField(identifier, PredefinedType(Token(kind)), initializerExpression, isConst, accessModifier);

            //var arrayType = ArrayType(PredefinedType(Token(kind)))
            //        .WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));

            //return GetVariableDeclaration(identifier, arrayType, initializerExpression, isConst, accessModifier);
        }

        FieldDeclarationSyntax CreateArrayField(string name, TypeSyntax predefinedType, InitializerExpressionSyntax initializer, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var arrayType = ArrayType(predefinedType)
                    .WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))));

            return GetVariableDeclaration(name, arrayType, initializer, isConst, accessModifier);
        }

        public FieldDeclarationSyntax GetVariableDeclaration(string nameVariable, TypeSyntax typeVariable, ExpressionSyntax literalExpression, bool isConst = false, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var nameDeclaration = VariableDeclarator(Identifier(nameVariable)).WithInitializer(EqualsValueClause(literalExpression));

            var tokenList = isConst ? TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword)) : TokenList(Token(accessModifier));

            var field = FieldDeclaration(VariableDeclaration(typeVariable).WithVariables(SingletonSeparatedList(nameDeclaration)))
                                        .WithModifiers(tokenList);
            return field;
        }

        //ObjectCreationExpressionSyntax CreateMethodObjectCreationExpression(string identifierName, List<ArgumentSyntax> arguments)
        //{
        //    return ObjectCreationExpression(IdentifierName(identifierName))
        //                .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        //}

        //InvocationExpressionSyntax CreateMethodInvocationExpression(string methodName, List<ArgumentSyntax> arguments)
        //{
        //    return InvocationExpression(IdentifierName(methodName))
        //                .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        //}

        //InvocationExpressionSyntax CreateVariantCreateFromMethodInvocationExpression(ExpressionSyntax literalExpression, string expression = "Variant", string name = "CreateFrom")
        //{
        //    return CreateMethodInvocationExpressionSyntax(expression, name)
        //                .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(literalExpression))));
        //}

        //InvocationExpressionSyntax CreateVariantCreateFromMethodInvocationExpression(List<ArgumentSyntax> arguments, string expression = "Variant", string name = "CreateFrom")
        //{
        //    return CreateMethodInvocationExpressionSyntax(expression, name)
        //                .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        //}

        //InvocationExpressionSyntax CreateMethodInvocationExpressionSyntax(string methodName)
        //{
        //    return InvocationExpression(IdentifierName(methodName));
        //}

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

        ObjectCreationExpressionSyntax ReplaceArgumentsInExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, ArgumentSyntax argument)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SingletonSeparatedList(argument)));
        }

        ObjectCreationExpressionSyntax ReplaceArgumentsInExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, List<ArgumentSyntax> arguments)
        {
            return expressionSyntax.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        //ObjectCreationExpressionSyntax AddArgumentsToExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, ArgumentSyntax argument)
        //{
        //    return expressionSyntax.AddArgumentListArguments(argument);
        //}

        //ObjectCreationExpressionSyntax AddArgumentsToExpressionSyntax(ObjectCreationExpressionSyntax expressionSyntax, List<ArgumentSyntax> arguments)
        //{
        //    return expressionSyntax.AddArgumentListArguments(arguments.ToArray());
        //}
    }
}