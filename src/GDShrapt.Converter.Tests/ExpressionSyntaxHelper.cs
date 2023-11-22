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
        //public LiteralExpressionSyntax LiteralExpressionSyntax;
        public ArgumentSyntax ArgumentLiteralExpressionSyntax;
        //public List<ArgumentSyntax> ArgumentLiteralExpressionSyntaxList = new List<ArgumentSyntax>();

        public ExpressionSyntaxHelper()
        {
            //LiteralExpressionSyntax = default;
            ArgumentLiteralExpressionSyntax = default;
        }

        public ExpressionSyntaxHelper(LiteralExpressionSyntax literalExpressionSyntax)
        {
            //LiteralExpressionSyntax = literalExpressionSyntax;
        }

        public ExpressionSyntaxHelper(List<ArgumentSyntax> literalExpressionSyntaxes)
        {
            //LiteralExpressionSyntax = default;
            ArgumentLiteralExpressionSyntax = default;
            //ArgumentLiteralExpressionSyntaxList = literalExpressionSyntaxes;
        }

        //public List<ArgumentSyntax> GetResultArgumentLiteralExpressionSyntax()
        //{
        //    ArgumentLiteralExpressionSyntax.RemoveAt(ArgumentLiteralExpressionSyntax.Count - 1);
        //    return ArgumentLiteralExpressionSyntax;
        //}
    }
}
