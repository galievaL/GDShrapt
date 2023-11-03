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
        public List<LiteralExpressionSyntax> LiteralExpressionSyntaxesList;
        public CountExpressionSyntax? CountExprSyntax;

        public List<ArgumentSyntax> ArgumentLiteralExpressionSyntax = new List<ArgumentSyntax>();

        public ExpressionSyntaxHelper(LiteralExpressionSyntax literalExpressionSyntax)
        {
            LiteralExpressionSyntax = literalExpressionSyntax;
            CountExprSyntax = CountExpressionSyntax.Singular;
        }

        public ExpressionSyntaxHelper(List<LiteralExpressionSyntax> literalExpressionSyntaxes)
        {
            LiteralExpressionSyntaxesList = literalExpressionSyntaxes;
            CountExprSyntax = CountExpressionSyntax.Multiple;
        }
    }

    public enum CountExpressionSyntax
    {
        Singular,
        Multiple
    }
}
