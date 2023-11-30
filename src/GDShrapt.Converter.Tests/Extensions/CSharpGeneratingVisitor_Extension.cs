using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using static GDShrapt.Converter.Tests.MyType;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        GDIdentifier GetIdentifierExpression(GDNode node) 
            => ((GDIdentifierExpression)node?.Nodes?.Where(x => x.TypeName == "GDIdentifierExpression").FirstOrDefault()).Identifier;
        
        List<GDNode> GetExpressionsList(GDNode node) 
            => node?.Nodes?.Where(x => x.TypeName == "GDExpressionsList").FirstOrDefault()?.Nodes?.ToList();

        ExpressionSyntax GetLiteralExpression(GDNode node, bool isConst = false, ArgumentSyntax argumentsFactory = null)
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
                        args.Add(GetLiteralExpression(p, isConst, argumentsFactory));

                    return GetArgumentToMethodExpressionSyntax(ident, isConst, args.Select(x => Argument(x)).ToList());
                case "GDDualOperatorExpression":
                    var operatorType = GetCSharpDualOperatorType(((GDDualOperatorExpression)node).OperatorType);
                    var nodes = node.Nodes.ToList();

                    return BinaryExpression(operatorType, GetLiteralExpression(nodes[0], isConst), GetLiteralExpression(nodes[1], isConst));
                case "GDStringExpression":
                    return GetLiteralExpression((GDStringExpression)node);
                case "GDNumberExpression":
                    return GetLiteralExpression((GDNumberExpression)node);
                case "GDBoolExpression":
                    return GetLiteralExpression(bool.Parse(node.ToString()));
                case "GDIdentifierExpression":
                    var identif = ((GDIdentifierExpression)node).Identifier.ToString();
                    ValidateTypeAndNameHelper.IsGodotFunctions(ref identif, out MyType type);
                    return IdentifierName(identif);
                default:
                    throw new NotImplementedException();
            }
        }
         
        //ArgumentSyntax GetArgumentSyntaxExpression(GDNode node, ArgumentSyntax argumentsFactory = null)
        //{
        //    if (node.TypeName == "")
        //        throw new NotImplementedException();

        //    switch (node.TypeName)
        //    {
        //        case "GDCallExpression":
        //            var ident = GetIdentifierExpression(node);
        //            var parameters = GetExpressionsList(node);
        //            var args = new List<ArgumentSyntax>();

        //            foreach (var p in parameters)
        //                args.Add(GetArgumentSyntaxExpression(p, argumentsFactory));

        //            return Argument(GetArgumentToMethodExpressionSyntax(ident, args));
        //        case "GDStringExpression":
        //            return Argument(GetLiteralExpression((GDStringExpression)node));
        //        case "GDNumberExpression":
        //            return Argument(GetLiteralExpression((GDNumberExpression)node));
        //        case "GDBoolExpression":
        //            return Argument(GetLiteralExpression(bool.Parse(node.ToString())));
        //        case "GDIdentifierExpression":
        //            var identif = ((GDIdentifierExpression)node).Identifier.ToString();
        //            ValidateTypeAndNameHelper.IsItGodotFunctions(ref identif, out MyType type);
        //            return Argument(IdentifierName(identif));
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //ExpressionSyntax GetBinaryExpression(GDNode node, ArgumentSyntax argumentsFactory = null)
        //{
        //    if (node.TypeName == "")
        //        throw new NotImplementedException();

        //    switch (node.TypeName)
        //    {
        //        case "GDDualOperatorExpression":
        //            var operatorType = GetCSharpDualOperatorType(((GDDualOperatorExpression)node).OperatorType);
        //            var nodes = node.Nodes.ToList();

        //            return BinaryExpression(operatorType, GetBinaryExpression(nodes[0]), GetBinaryExpression(nodes[1]));
        //        case "GDCallExpression":
        //        case "GDStringExpression":
        //        case "GDNumberExpression":
        //        case "GDBoolExpression":
        //        case "GDIdentifierExpression":
        //            return GetLiteralExpression(node);
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, List<ExpressionSyntax> arguments)
        //{
        //    var args = arguments.Select(x => Argument(x)).ToList();

        //    return GetArgumentToMethodExpressionSyntax(methodNameIdentifier, args);
        //}

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, bool isConst, List<ArgumentSyntax> arguments)
        {
            var methodName = methodNameIdentifier.ToString();
            var validMethodName = ValidateTypeAndNameHelper.GetValidateFieldName(methodName);

            if (ValidateTypeAndNameHelper.IsGodotFunctions(ref methodName, out MyType type))
            {
                return InvocationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(ref methodName))
            {
                return ObjectCreationExpression(IdentifierName(methodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else if (isConst || methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out GDIdentifier gdIdent))
            {
                return InvocationExpression(IdentifierName(validMethodName))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
            else
            {
                return InvocationExpression(IdentifierName("Call"))
                                    .AddArgumentListArguments(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(validMethodName))))
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
            var numberType = numberExpression.Number.ResolveNumberType();

            if (numberType == GDNumberType.LongDecimal || numberType == GDNumberType.LongBinary || numberType == GDNumberType.LongHexadecimal)
                return SyntaxKind.LongKeyword;
            else if (numberType == GDNumberType.Double)
                return SyntaxKind.DoubleKeyword;

            return SyntaxKind.None;
        }

        MyType GetAverageType(List<GDNode> allNodes)
        {
            int stringExpr = 0, longExpr = 0, doubleExpr = 0, boolExpr = 0, identExpr = 0;


            foreach (var i in allNodes)
            {
                if (i.TypeName == "GDCallExpression" || i.TypeName == "GDIdentifierExpression")
                {
                    var name = i.TypeName == "GDCallExpression" ? GetIdentifierExpression(i).ToString() : ((GDIdentifierExpression)i).Identifier.ToString();

                    if (ValidateTypeAndNameHelper.IsGDScriptConsts(ref name, out SyntaxKind? type2) && type2.Value == SyntaxKind.DoubleKeyword)
                    {
                        ++doubleExpr;
                    }
                    else if (ValidateTypeAndNameHelper.IsGodotFunctions(ref name, out MyType type))
                    {
                        SyntaxKind? kind = null;
                        AnotherType? anotherType = null;
                        ArrayTypes? arrayTypes = null;

                        type?.Match<MyType>(
                                    syntaxKind => kind = syntaxKind.Kind,
                                    another => anotherType = another.Type,
                                    arrays => arrayTypes = arrays.Array);

                        if (anotherType != null)
                            return anotherType;

                        if (arrayTypes != null)
                            return arrayTypes;

                        if (kind != null)
                        {
                            if (kind == SyntaxKind.StringKeyword) ++stringExpr;
                            else if (kind == SyntaxKind.LongKeyword) ++longExpr;
                            else if (kind == SyntaxKind.DoubleKeyword) ++doubleExpr;
                            else if (kind == SyntaxKind.BoolKeyword) ++boolExpr;
                            else throw new NotImplementedException();
                        }
                    }
                    else
                        return AnotherType.Variant;
                }
                else
                {

                    switch (i.TypeName)
                    {
                        case "GDExpressionsList":
                        case "GDDualOperatorExpression":
                            break;
                        case "GDStringExpression":
                            ++stringExpr;
                            break;
                        case "GDBoolExpression":
                            ++boolExpr;
                            break;
                        case "GDNumberExpression":
                            var sKind = GetSyntaxKind((GDNumberExpression)i);

                            if (sKind == SyntaxKind.LongKeyword) ++longExpr;
                            else if (sKind == SyntaxKind.DoubleKeyword) ++doubleExpr;

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            if (stringExpr > 0 && longExpr + doubleExpr + boolExpr + identExpr == 0)
            {
                return SyntaxKind.StringKeyword;
            }
            else if (boolExpr > 0 && longExpr + doubleExpr + stringExpr + identExpr == 0)
            {
                return SyntaxKind.BoolKeyword;
            }
            else if (longExpr + doubleExpr > 0 && stringExpr + boolExpr + identExpr == 0)
            {
                if (doubleExpr > 0)
                    return SyntaxKind.DoubleKeyword;
                else
                    return SyntaxKind.LongKeyword;
            }
            else
                return AnotherType.Variant;
        }

        TypeSyntax GetTypeVariable(bool isThereColon, GDType variableDeclarationType, bool isThereConst, GDIdentifier methodNameIdentifier = null, MyType? averageType = null)
        {
            methodNameIdentifier = methodNameIdentifier ?? new GDIdentifier();
            var methodName = methodNameIdentifier?.ToString() ?? "";
            var gdIdent = new GDIdentifier();

            SyntaxKind? kind = null;
            AnotherType? anotherType = null;
            ArrayTypes? arrayType = null;

            averageType?.Match<MyType>(
                            syntaxKind => kind = syntaxKind.Kind,
                            another => anotherType = another.Type,
                            arrays => arrayType = arrays.Array);

            if (ValidateTypeAndNameHelper.IsGodotFunctions(ref methodName, out MyType returnType))
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else
                {
                    return returnType.Match<TypeSyntax>(
                                syntaxKind => PredefinedType(Token(syntaxKind.Kind)),
                                another => IdentifierName(another.Type.ToString()),
                                arrays => IdentifierName(arrays.Array.ToString()));
                }
            }
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                {
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                }
                else if (isThereColon && methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent) && ((GDMethodDeclaration)gdIdent.Parent).ReturnType != null)
                {
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(((GDMethodDeclaration)gdIdent.Parent).ReturnType.ToString()));
                }
                else if (kind != null && kind != SyntaxKind.None)
                {
                    return PredefinedType(Token(kind.Value));
                }
                else if (anotherType != null)
                {
                    return IdentifierName(anotherType.Value.ToString());
                }
                else if (arrayType != null)
                {
                    return IdentifierName(arrayType.Value.ToString());
                }
                else if (ValidateTypeAndNameHelper.IsStandartGodotType(methodName))
                {
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName));
                }
                else
                    return IdentifierName(AnotherType.Variant.ToString());
            }
            else
                return IdentifierName(AnotherType.Variant.ToString());
        }

        SyntaxTokenList GetModifier(string methodName = "", bool isConst = false, MyType? averageType = null, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            SyntaxKind? kind = null;
            AnotherType? anotherType = null;
            ArrayTypes? arrayType = null;

            averageType?.Match<MyType>(
                            syntaxKind => kind = syntaxKind.Kind,
                            another => anotherType = another.Type,
                            array => arrayType = array.Array);

            if (isConst)
            {
                if (kind != null && kind != SyntaxKind.None && (kind == SyntaxKind.IntKeyword || kind == SyntaxKind.LongKeyword || kind == SyntaxKind.FloatKeyword 
                    || kind == SyntaxKind.DoubleKeyword || kind == SyntaxKind.StringKeyword || kind == SyntaxKind.BoolKeyword))
                    return TokenList(Token(accessModifier), Token(SyntaxKind.ConstKeyword));
                else
                    return TokenList(Token(accessModifier), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword));
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

        SyntaxKind GetCSharpDualOperatorType(GDDualOperatorType gdType)
        {
            switch (gdType)
            {
                case GDDualOperatorType.Null:
                    throw new NotImplementedException();
                case GDDualOperatorType.MoreThan:
                    return SyntaxKind.GreaterThanExpression;
                case GDDualOperatorType.LessThan:
                    return SyntaxKind.LessThanExpression;
                case GDDualOperatorType.Assignment:
                    return SyntaxKind.EqualsValueClause;
                case GDDualOperatorType.Subtraction:
                    return SyntaxKind.SubtractExpression;
                case GDDualOperatorType.Division:
                    return SyntaxKind.DivideExpression;
                case GDDualOperatorType.Multiply:
                    return SyntaxKind.MultiplyExpression;
                case GDDualOperatorType.Addition:
                    return SyntaxKind.AddExpression;
                case GDDualOperatorType.AddAndAssign:
                    return SyntaxKind.AddAssignmentExpression;
                case GDDualOperatorType.NotEqual:
                    return SyntaxKind.NotEqualsExpression;
                case GDDualOperatorType.MultiplyAndAssign:
                    return SyntaxKind.MultiplyExpression;
                case GDDualOperatorType.SubtractAndAssign:
                    return SyntaxKind.SubtractAssignmentExpression;
                case GDDualOperatorType.LessThanOrEqual:
                    return SyntaxKind.LessThanOrEqualExpression;
                case GDDualOperatorType.MoreThanOrEqual:
                    return SyntaxKind.GreaterThanOrEqualExpression;
                case GDDualOperatorType.Equal:
                    return SyntaxKind.EqualsExpression;
                case GDDualOperatorType.DivideAndAssign:
                    return SyntaxKind.DivideAssignmentExpression;
                case GDDualOperatorType.Or:
                    return SyntaxKind.LogicalOrExpression;
                case GDDualOperatorType.Or2:
                    return SyntaxKind.LogicalOrExpression;
                case GDDualOperatorType.And:
                    return SyntaxKind.LogicalAndExpression;
                case GDDualOperatorType.And2:
                    return SyntaxKind.LogicalAndExpression;
                case GDDualOperatorType.Is:
                    return SyntaxKind.IsExpression;
                case GDDualOperatorType.As:
                    return SyntaxKind.AsExpression;
                case GDDualOperatorType.ModAndAssign:
                    return SyntaxKind.ModuloAssignmentExpression;
                case GDDualOperatorType.BitShiftLeft:
                    return SyntaxKind.LeftShiftExpression;
                case GDDualOperatorType.BitShiftRight:
                    return SyntaxKind.RightShiftExpression;
                case GDDualOperatorType.Mod:
                    return SyntaxKind.ModuloExpression;
                case GDDualOperatorType.Xor:
                    return SyntaxKind.ExclusiveOrExpression;
                case GDDualOperatorType.BitwiseOr:
                    return SyntaxKind.BitwiseOrExpression;
                case GDDualOperatorType.BitwiseAnd:
                    return SyntaxKind.BitwiseAndExpression;
                case GDDualOperatorType.In:
                    throw new NotImplementedException(); //ToDo: написать конкретику
                case GDDualOperatorType.BitwiseAndAndAssign:
                    throw new NotImplementedException(); //ToDo: написать конкретику
                case GDDualOperatorType.BitwiseOrAndAssign:
                    throw new NotImplementedException(); //ToDo: написать конкретику
                default:
                    throw new NotImplementedException();
            }
        }
    }
}