using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    public class ExpressionSyntaxHelper
    {
        public LiteralExpressionSyntax LiteralExpressionSyntax;
        List<ArgumentSyntax> _argumentLiteralExpressionSyntax = new List<ArgumentSyntax>();

        public ExpressionSyntaxHelper()
        {
        }

        public ExpressionSyntaxHelper(LiteralExpressionSyntax literalExpressionSyntax)
        {
            LiteralExpressionSyntax = literalExpressionSyntax;
        }

        public ExpressionSyntaxHelper(List<ArgumentSyntax> literalExpressionSyntaxes)
        {
            _argumentLiteralExpressionSyntax = literalExpressionSyntaxes;
        }

        public void AddArgumentLiteralExpressionSyntax(ArgumentSyntax argument) =>
            _argumentLiteralExpressionSyntax.Add(argument);

        public List<ArgumentSyntax> GetArgumentLiteralExpressionSyntax() =>
            _argumentLiteralExpressionSyntax;

        public List<ArgumentSyntax> GetResultArgumentLiteralExpressionSyntax()
        {
            _argumentLiteralExpressionSyntax.RemoveAt(_argumentLiteralExpressionSyntax.Count - 1);
            return _argumentLiteralExpressionSyntax;
        }
    }
}
