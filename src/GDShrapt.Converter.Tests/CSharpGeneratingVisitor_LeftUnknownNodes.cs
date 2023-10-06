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
        public void LeftUnknown(GDNode node)
        {
            throw new NotImplementedException();
        }

        public void LeftUnknown(GDExpression e)
        {
            throw new NotImplementedException();
        }
    }
}