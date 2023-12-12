﻿using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        readonly ConversionSettings _conversionSettings;

        private ClassDeclarationSyntax _partsCode;
        private CompilationUnitSyntax _compilationUnit;
        private List<MethodDeclarationSyntax> _methodsPartsCode = new List<MethodDeclarationSyntax>();
        private List<UsingDirectiveSyntax> _codeUsings = new List<UsingDirectiveSyntax>();

        private string _className;

        public CSharpGeneratingVisitor(ConversionSettings conversionSettings)
        {
            _conversionSettings = conversionSettings;

            _codeUsings.Add(UsingDirective(ParseName("Godot")));
            _codeUsings.Add(UsingDirective(ParseName("System")));
            _codeUsings.Add(UsingDirective(ParseName("System.Linq")));
        }

        public void EnterListChild(GDNode node)
        {
        }

        public void EnterNode(GDNode node)
        {
        }

        internal string BuildCSharpNormalisedCode()
        {
            var @namespace = NamespaceDeclaration(ParseName("Generated")).NormalizeWhitespace();

            _compilationUnit = CompilationUnit().AddUsings(_codeUsings.ToArray()).AddMembers(@namespace.AddMembers(_partsCode)).NormalizeWhitespace();

            var code = _compilationUnit.ToFullString();
            return code;
        }
    }
}