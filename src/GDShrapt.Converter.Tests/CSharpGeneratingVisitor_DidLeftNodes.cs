using System;
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
        public void DidLeft(GDNode node)
        {
            var @namespace = NamespaceDeclaration(ParseName("Generated")).NormalizeWhitespace();
            var usings = new UsingDirectiveSyntax[]{UsingDirective(ParseName("Godot")),
                                                    UsingDirective(ParseName("System")),
                                                    UsingDirective(ParseName("System.Linq"))};

            _compilationUnit = CompilationUnit().AddUsings(usings).AddMembers(@namespace.AddMembers(_partsCode)).NormalizeWhitespace();
        }

        public void DidLeft(GDExpression expr)
        {

        }
    }
}