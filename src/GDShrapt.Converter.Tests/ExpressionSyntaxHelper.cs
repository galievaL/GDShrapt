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
        //public List<LiteralExpressionSyntax> LiteralExpressionSyntaxesList;
        //public CountExpressionSyntax? CountExprSyntax;

        //public List<ArgumentSyntax> ArgumentLiteralExpressionSyntax = new List<ArgumentSyntax>();

        List<ArgumentSyntax> _argumentLiteralExpressionSyntax = new List<ArgumentSyntax>();

        public List<ArgumentSyntax> GetArgumentLiteralExpressionSyntax()
        {
            return _argumentLiteralExpressionSyntax;
        }

        public List<ArgumentSyntax> GetResultArgumentLiteralExpressionSyntax()
        {
            _argumentLiteralExpressionSyntax.RemoveAt(_argumentLiteralExpressionSyntax.Count - 1);
            return _argumentLiteralExpressionSyntax;
        }

        public void AddArgumentLiteralExpressionSyntax(ArgumentSyntax argument)
        {
            _argumentLiteralExpressionSyntax.Add(argument);
        }

        public ExpressionSyntaxHelper()
        {
        }

        public ExpressionSyntaxHelper(LiteralExpressionSyntax literalExpressionSyntax)
        {
            LiteralExpressionSyntax = literalExpressionSyntax;
            //CountExprSyntax = CountExpressionSyntax.Singular;
        }

        public ExpressionSyntaxHelper(List<ArgumentSyntax> literalExpressionSyntaxes)
        {
            _argumentLiteralExpressionSyntax = literalExpressionSyntaxes;
            //LiteralExpressionSyntaxesList = literalExpressionSyntaxes;
            //CountExprSyntax = CountExpressionSyntax.Multiple;
        }
    }

    public enum CountExpressionSyntax
    {
        Singular,
        Multiple
    }
}
