using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        GDIdentifier GetIdentifier(GDNode node)
        {
            var ident = (GDIdentifierExpression)node?.Nodes?.Where(x => x.TypeName == "GDIdentifierExpression")?.FirstOrDefault();

            if (ident != null)
                return ident.Identifier;

            var expr = node?.Nodes?.Where(x => x.TypeName == "GDMemberOperatorExpression" || x.TypeName == "GDCallExpression")?.FirstOrDefault();

            if (expr != null)
                return GetIdentifier(expr);
            else
                throw new NotImplementedException();
        }

        List<GDNode> GetExpressionsList(GDNode node) 
            => node?.Nodes?.Where(x => x.TypeName == "GDExpressionsList")?.FirstOrDefault()?.Nodes?.ToList();

        ExpressionSyntax GetLiteralExpression(GDNode node, bool isConst = false, bool isVariantLeftPartType = false, ScopeType scopeType = ScopeType.Class)
        {
            var nodes = new List<GDNode>();
            var memNode = default(GDMemberOperatorExpression);
            var arguments = new List<ExpressionSyntax>();

            if (node.TypeName == "")
                throw new NotImplementedException();

            switch (node.TypeName)
            {
                case "GDStringExpression":
                    return GetLiteralExpression((GDStringExpression)node);
                case "GDNumberExpression":
                    return GetLiteralExpression((GDNumberExpression)node);
                case "GDBoolExpression":
                    return GetLiteralExpression(bool.Parse(node.ToString()));
                case "GDIdentifierExpression":
                    return GetLiteralExpression((GDIdentifierExpression)node, scopeType);
                case "GDCallExpression":
                    nodes = node?.Nodes.ToList();

                    var ident = ((GDIdentifierExpression)nodes?.Where(x => x.TypeName == "GDIdentifierExpression")?.FirstOrDefault())?.Identifier;
                    var parameters = ((GDExpressionsList)nodes?.Where(x => x.TypeName == "GDExpressionsList")?.FirstOrDefault()).Nodes?.ToList();

                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                            arguments.Add(GetLiteralExpression(p, isConst, isVariantLeftPartType));
                    }

                    memNode = (GDMemberOperatorExpression)nodes?.Where(x => x.TypeName == "GDMemberOperatorExpression")?.FirstOrDefault();
                    if (memNode != null)
                    {
                        var methodName2 = memNode.Identifier.ToString();

                        if (methodName2 == "new")
                        {
                            var methodName1 = GetLiteralExpression(memNode.Nodes.FirstOrDefault(), isConst, isVariantLeftPartType).ToString();
                            return ObjectCreationExpression(IdentifierName(methodName1))
                                    .WithArgumentList(ArgumentList(SeparatedList(arguments.Select(x => Argument(x)).ToList())));
                        }

                        var memb = GetLiteralExpression(memNode, isConst, isVariantLeftPartType);
                        var validMethodN = ValidateTypeAndNameHelper.GetValidateFieldName(methodName2, scopeType);

                        return GetArgumentToMethodExpressionSyntax(memb, arguments, isVariantLeftPartType, validMethodN);
                    }
                    return GetArgumentToMethodExpressionSyntax(ident, isConst, arguments, isVariantLeftPartType, scopeType);
                case "GDDualOperatorExpression":
                    var operatorType = GetCSharpDualOperatorType(((GDDualOperatorExpression)node).OperatorType);
                    nodes = node.Nodes.ToList();

                    var ex1 = GetLiteralExpression(nodes[0], isConst, isVariantLeftPartType, scopeType);
                    var ex2 = GetLiteralExpression(nodes[1], isConst, isVariantLeftPartType, scopeType);

                    if (isVariantLeftPartType)
                        return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ex1, IdentifierName("Add")))
                                .AddArgumentListArguments(Argument(ex2));
                    else
                        return BinaryExpression(operatorType, ex1, ex2);
                case "GDMemberOperatorExpression":
                    memNode = (GDMemberOperatorExpression)node;
                    var methodName = memNode.Identifier.ToString();

                    var validMethodName = ValidateTypeAndNameHelper.GetValidateFieldName(methodName, scopeType);

                    var expr1 = GetLiteralExpression(memNode.Nodes.FirstOrDefault(), isConst, isVariantLeftPartType);
                    var expr2 = isVariantLeftPartType ? IdentifierName("Call") : IdentifierName(validMethodName);

                    return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expr1, expr2);
                case "GDArrayInitializerExpression":
                    var arrayElements = ((GDArrayInitializerExpression)node).Values.Nodes.ToList();

                    foreach (var p in arrayElements)
                        arguments.Add(GetLiteralExpression(p, isConst, isVariantLeftPartType));

                    return ArrayCreationExpression(ArrayType(IdentifierName("Array")), InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(arguments)));
                case "GDDictionaryInitializerExpression":
                    var dictionaryElements = ((GDDictionaryInitializerExpression)node).KeyValues.Nodes.ToList();

                    foreach (var p in dictionaryElements)
                        arguments.Add(GetLiteralExpression(p, isConst, isVariantLeftPartType));

                    return ObjectCreationExpression(IdentifierName("Dictionary"))
                                    .WithInitializer(InitializerExpression(SyntaxKind.CollectionInitializerExpression, SeparatedList(arguments)));
                case "GDDictionaryKeyValueDeclaration":
                    var exprS = new ExpressionSyntax[] 
                    {
                        GetLiteralExpression(((GDDictionaryKeyValueDeclaration)node).Key, isConst, isVariantLeftPartType),
                        GetLiteralExpression(((GDDictionaryKeyValueDeclaration)node).Value, isConst, isVariantLeftPartType)
                    };

                    return InitializerExpression(SyntaxKind.ComplexElementInitializerExpression, SeparatedList(exprS));
                default:
                    throw new NotImplementedException();
            }
        }

        FieldDeclarationSyntax GetCollectionMember(GDNode node, string collectionType, ScopeType scopeType = ScopeType.Class)
        {
            var parents = (GDVariableDeclaration)node.Parent;
            var identifier = ValidateTypeAndNameHelper.GetValidateFieldName(parents.Identifier.ToString(), scopeType);
            var leftPartType = IdentifierName(collectionType);
            var modifiers = GetModifier();
            var rightPart = GetLiteralExpression(node);

            return GetVariableFieldDeclaration(identifier, leftPartType, modifiers, rightPart);
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(ExpressionSyntax methodExpression, List<ExpressionSyntax> expressions, bool isVariantLeftPartType, string methodName = "")
        {
            var arguments = expressions.Select(x => Argument(x)).ToList();

            if (isVariantLeftPartType)
            {
                return InvocationExpression(methodExpression)
                            .AddArgumentListArguments(Argument(GetLiteralExpression(methodName)))
                            .AddArgumentListArguments(arguments.ToArray());
            }
            else
            {
                return InvocationExpression(methodExpression)
                            .WithArgumentList(ArgumentList(SeparatedList(arguments)));
            }
        }

        ExpressionSyntax GetArgumentToMethodExpressionSyntax(GDIdentifier methodNameIdentifier, bool isConst, List<ExpressionSyntax> expressions, bool isVariantLeftPartType, ScopeType scopeType)
        {
            var methodName = methodNameIdentifier.ToString();
            var validMethodName = ValidateTypeAndNameHelper.GetValidateFieldName(methodName, scopeType);
            var arguments = expressions.Select(x => Argument(x)).ToList();

            if (methodName == "fmod")
            {
                return BinaryExpression(SyntaxKind.ModuloExpression, expressions[0], expressions[1]);
            }
            else if (methodName == "str")
            {
                return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressions[0], IdentifierName("ToString")));
            }
            else if (ValidateTypeAndNameHelper.IsGodotFunctions(ref methodName, out GeneralType type))
            {
                if (methodName == "char" || methodName == "float")
                    return ParenthesizedExpression(CastExpression(IdentifierName(type.ToString()), expressions[0]));
                else 
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

        LiteralExpressionSyntax GetLiteralExpression(GDNumberExpression numberExpression)
        {
            SyntaxToken literal;
            var number = numberExpression.Number;

            switch (number.ResolveNumberType())
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

        ExpressionSyntax GetLiteralExpression(GDIdentifierExpression identExpression, ScopeType scopeType)
        {
            var identif = identExpression.Identifier.ToString();

            if (ValidateTypeAndNameHelper.IsGodotFunctions(ref identif, out GeneralType type))
                return IdentifierName(identif);
            else
                return IdentifierName(ValidateTypeAndNameHelper.GetValidateFieldName(identExpression.Identifier.ToString(), scopeType));
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

        /*
        (SyntaxKind? kind, AnotherType? anotherType, ArrayTypes? arrayTypes) ConvertMyTypeToTuple(MyType type)
        {
            SyntaxKind? kind = null;
            AnotherType? anotherType = null;
            ArrayTypes? arrayType = null;

            type?.Match<MyType>(
                        syntaxKind => kind = syntaxKind.Kind,
                        another => anotherType = another.Type,
                        arrays => arrayType = arrays.Array);

            return (kind, anotherType, arrayType);
        }

        TypeSyntax GetTypeSyntax(MyType type)
        {
            return type.Match<TypeSyntax>(
                        syntaxKind => PredefinedType(Token(syntaxKind.Kind)),
                        another => IdentifierName(another.Type.ToString()),
                        arrays => IdentifierName(arrays.Array.ToString()));
        }*/

        GeneralType GetAverageType2(GDNode node)
        {
            var ident = "";

            switch (node.TypeName)
            {
                case "GDDualOperatorExpression":
                    var nodes = node.Nodes.ToList();

                    var a = GetAverageType2(nodes[0]);
                    var b = GetAverageType2(nodes[1]);
                   
                    break;
                case "GDIdentifierExpression":
                    ident = ((GDIdentifierExpression)node).Identifier.ToString();
                    var type = IsClassVariable(ident);

                    if (type.CustomType != CustomSyntaxKind.None)
                        return new GeneralType(type.CustomType);
                    else if (type.UserType != "")
                        return new GeneralType(type.UserType);
                    else
                        return GetAverageType(new List<GDNode>() { node });
                default:
                    break;
            }
            return default;
        }

        bool IsClassVariable(string name, out GeneralType type)
        {
            type = new GeneralType();

            if (!_classVariableList.ContainsKey(name))
                return false;
            
            type = _classVariableList[name];
            return true;
        }

        GeneralType IsClassVariable(string name)
        {
            var isContain = _classVariableList.ContainsKey(name);

            if (isContain)
                return _classVariableList[name];

            return null;
        }

        GeneralType GetAverageType(List<GDNode> allNodes)
        {
            int stringExpr = 0, longExpr = 0, doubleExpr = 0, boolExpr = 0, identExpr = 0;

            foreach (var i in allNodes)
            {
                if (i.TypeName == "GDCallExpression" || i.TypeName == "GDIdentifierExpression")
                {
                    var name = (i.TypeName == "GDCallExpression") ? GetIdentifier((GDCallExpression)i).ToString() : ((GDIdentifierExpression)i).Identifier.ToString();

                    if (IsClassVariable(name, out GeneralType variableType))
                        return variableType;
                    else
                    if (ValidateTypeAndNameHelper.IsGDScriptConsts(ref name, out GeneralType type2) && type2.CustomType == CustomSyntaxKind.DoubleKeyword)
                    {
                        ++doubleExpr;
                    }
                    else if (ValidateTypeAndNameHelper.IsGodotFunctions(ref name, out GeneralType type))
                    {
                        if (type != null)
                        {
                            if (type.CustomType == CustomSyntaxKind.StringKeyword) ++stringExpr;
                            else if (type.CustomType == CustomSyntaxKind.LongKeyword) ++longExpr;
                            else if (type.CustomType == CustomSyntaxKind.DoubleKeyword) ++doubleExpr;
                            else if (type.CustomType == CustomSyntaxKind.BoolKeyword) ++boolExpr;
                            else return type;
                        }
                    }
                    else
                        return new GeneralType(CustomSyntaxKind.Variant);
                }
                else
                {
                    switch (i.TypeName)
                    {
                        case "GDExpressionsList":
                        case "GDDualOperatorExpression":
                            return GetAverageType(i.Nodes.ToList());
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

            var kind = default(CustomSyntaxKind);

            if (stringExpr > 0 && longExpr + doubleExpr + boolExpr + identExpr == 0)
            {
                kind = CustomSyntaxKind.StringKeyword;
            }
            else if (boolExpr > 0 && longExpr + doubleExpr + stringExpr + identExpr == 0)
            {
                kind = CustomSyntaxKind.BoolKeyword;
            }
            else if (longExpr + doubleExpr > 0 && stringExpr + boolExpr + identExpr == 0)
            {
                if (doubleExpr > 0)
                    kind = CustomSyntaxKind.DoubleKeyword;
                else
                    kind = CustomSyntaxKind.LongKeyword;
            }
            else
                kind = CustomSyntaxKind.Variant;

            return new GeneralType(kind);
        }

        GeneralType GetTypeVariable(string typeName)
        {
            if (typeName == null)
            {
                return new GeneralType(CustomSyntaxKind.Variant);
            }
            else if (ValidateTypeAndNameHelper.IsStandartGodotType(typeName))
            {
                return ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(typeName);
            }
            else
                return GeneralTypeHelper.GetGeneralType(typeName);
        }

        GeneralType GetTypeVariable(bool isThereColon, GDType variableDeclarationType, bool isThereConst, GDIdentifier methodNameIdentifier = null, GeneralType averageType = null)
        {
            methodNameIdentifier = methodNameIdentifier ?? new GDIdentifier();
            var methodName = methodNameIdentifier?.ToString() ?? "";
            var gdIdent = new GDIdentifier();

            if (ValidateTypeAndNameHelper.IsGodotFunctions(ref methodName, out GeneralType returnType))
            {
                if (isThereColon && (variableDeclarationType != null))
                {
                    var t = ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString());
                    return t == null ? new GeneralType(variableDeclarationType.ToString()) : t;
                }
                else
                    return returnType;
            }
            else if (isThereConst || isThereColon)
            {
                if (isThereColon && (variableDeclarationType != null))
                {
                    return ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString());
                }
                else if (isThereColon && methodNameIdentifier.TryExtractLocalScopeVisibleDeclarationFromParents(out gdIdent) && ((GDMethodDeclaration)gdIdent.Parent).ReturnType != null)
                {
                    return ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(((GDMethodDeclaration)gdIdent.Parent).ReturnType.ToString());
                }
                else if (averageType != null && averageType.CustomType != CustomSyntaxKind.None)
                {
                    return averageType;
                }
                else if (ValidateTypeAndNameHelper.IsStandartGodotType(methodName))
                {
                    return ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName);
                }
                else
                    return new GeneralType(CustomSyntaxKind.Variant);
            }
            else
                return new GeneralType(CustomSyntaxKind.Variant);
        }
        /*
        TypeSyntax GetTypeVariable(bool isThereColon, GDType variableDeclarationType, bool isThereConst, GDIdentifier methodNameIdentifier = null, CustomSyntaxKind? averageType = null)
        {
            methodNameIdentifier = methodNameIdentifier ?? new GDIdentifier();
            var methodName = methodNameIdentifier?.ToString() ?? "";
            var gdIdent = new GDIdentifier();

            //(SyntaxKind? kind, AnotherType? anotherType, ArrayTypes? arrayTypes) = ConvertMyTypeToTuple(averageType);

            if (ValidateTypeAndNameHelper.IsGodotFunctions(ref methodName, out CustomSyntaxKind? returnType))
            {
                if (isThereColon && (variableDeclarationType != null))
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(variableDeclarationType.ToString()));
                else
                    return IdentifierName(returnType.Value.ToString());
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
                else if (averageType != null && averageType.Value != CustomSyntaxKind.None)
                {
                    if (CustomSyntaxKindConverter2.ContainsKey(averageType.Value))
                        return PredefinedType(Token(CustomSyntaxKindConverter2[averageType.Value]));
                    else
                        return IdentifierName(averageType.Value.ToString());
                }
                else if (ValidateTypeAndNameHelper.IsStandartGodotType(methodName))
                {
                    return IdentifierName(ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(methodName));
                }
                else
                    return IdentifierName(CustomSyntaxKind.Variant.ToString());
            }
            else
                return IdentifierName(CustomSyntaxKind.Variant.ToString());
        }*/

        SyntaxTokenList GetModifier(string methodName = "", bool isConst = false, GeneralType averageType = null, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            //(SyntaxKind? kind, AnotherType? anotherType, ArrayTypes? arrayTypes) = ConvertMyTypeToTuple(averageType);

            if (isConst)
            {
                var customType = averageType?.CustomType;

                if (averageType != null && customType != CustomSyntaxKind.None && (customType == CustomSyntaxKind.IntKeyword || customType == CustomSyntaxKind.LongKeyword 
                    || customType == CustomSyntaxKind.FloatKeyword || customType == CustomSyntaxKind.DoubleKeyword || customType == CustomSyntaxKind.StringKeyword || customType == CustomSyntaxKind.BoolKeyword))
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