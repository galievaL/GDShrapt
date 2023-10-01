using System;
using System.Linq;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        public void DidLeft(GDNode node)
        {
            ////////
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Generated")).NormalizeWhitespace();
            var usings = new UsingDirectiveSyntax[]{SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Godot")),
                                                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                                                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Linq"))};

            _compilationUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(usings)
                .AddMembers(@namespace.AddMembers(_partsCode))
                .NormalizeWhitespace();
        }

        public void DidLeft(GDExpression expr)
        {
            throw new System.NotImplementedException();
        }
    }
}