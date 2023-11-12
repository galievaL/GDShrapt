using System;
using System.Collections.Generic;
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
        readonly ConversionSettings _conversionSettings;

        private ClassDeclarationSyntax _partsCode;
        private CompilationUnitSyntax _compilationUnit;

        private string _className;
        private Dictionary<ParameterListTKey, List<ExpressionStatementSyntax>> _constructorCollection = new Dictionary<ParameterListTKey, List<ExpressionStatementSyntax>>();

        public CSharpGeneratingVisitor(ConversionSettings conversionSettings)
        {
            _conversionSettings = conversionSettings;
        }

        public void EnterListChild(GDNode node)
        {
            ////////
        }

        public void EnterNode(GDNode node)
        {
            ////////
        }

        internal string BuildCSharpNormalisedCode()
        {
            ////////
            var code = _compilationUnit.ToFullString();
            return code;
        }
    }
}