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
        public void DidLeft(GDNode node)
        {
        }

        public void DidLeft(GDExpression expr)
        {
        }
    }
}