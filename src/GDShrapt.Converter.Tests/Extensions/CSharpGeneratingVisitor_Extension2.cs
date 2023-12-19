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
        MethodDeclarationSyntax GetMethodDeclaration(GDType returnType, string methodName, ParameterSyntax[] parameters, List<StatementSyntax> statements, SyntaxKind accessModifier = SyntaxKind.PublicKeyword)
        {
            var methodType = returnType != null ? ValidateTypeAndNameHelper.GetTypeAdaptationToStandartMethodsType(returnType.ToString()).ToString() : CustomSyntaxKind.Variant.ToString();

            return MethodDeclaration(IdentifierName(methodType), methodName)
                        .WithModifiers(TokenList(Token(accessModifier)))
                        .WithParameterList(ParameterList(SeparatedList(parameters)))
                        .WithBody(Block(statements));
        }
    }
}
