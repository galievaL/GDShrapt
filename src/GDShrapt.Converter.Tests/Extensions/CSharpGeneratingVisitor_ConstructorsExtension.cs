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
        Dictionary<ParameterListTKey, List<ExpressionStatementSyntax>> _constructorCollection = new Dictionary<ParameterListTKey, List<ExpressionStatementSyntax>>();

        void AddToAllExistingConstructors(ExpressionStatementSyntax expressionStatement)
        {
            foreach (var c in _constructorCollection)
                _constructorCollection[c.Key].Add(expressionStatement);
        }

        void AddToConstructors(ParameterListTKey key, ExpressionStatementSyntax value)
        {
            if (_constructorCollection.ContainsKey(key))
                _constructorCollection[key].Add(value);
            else
                _constructorCollection.Add(key, new List<ExpressionStatementSyntax> { value });
        }

        void AddConstructor(ParameterListTKey key, List<ExpressionStatementSyntax> expressionsStatement)
        {
            if (!_constructorCollection.ContainsKey(key))
                _constructorCollection.Add(key, expressionsStatement);
        }

        void AddConstructor(ParameterListTKey key, ExpressionStatementSyntax expressionStatement)
        {
            if (!_constructorCollection.ContainsKey(key))
                _constructorCollection.Add(key, new List<ExpressionStatementSyntax> { expressionStatement });
        }

        ConstructorDeclarationSyntax GetConstructorDeclaration(List<ParameterSyntax> parametrs = null, SyntaxKind accessModifier = SyntaxKind.PublicKeyword, params ExpressionStatementSyntax[] expressionStatementSyntaxes)
        {
            var constrDecl = ConstructorDeclaration(_className)
                            .WithBody(Block(expressionStatementSyntaxes))
                            .AddModifiers(Token(accessModifier));

            if (parametrs != null)
                constrDecl.AddParameterListParameters(parametrs.ToArray());

            return constrDecl;
        }
    }
}
