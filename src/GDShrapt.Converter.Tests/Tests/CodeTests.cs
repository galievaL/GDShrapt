using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class CodeTests
    {
        [TestMethod]
        public void ListFillComparisonTest()
        {
            var pString = Parameter(Identifier("abc")).WithType(IdentifierName("string"));
            var pBool = Parameter(Identifier("abc")).WithType(IdentifierName("bool"));
            var pMyType = Parameter(Identifier("abc")).WithType(IdentifierName("MyType"));
            var pMyType2 = Parameter(Identifier("abc")).WithType(IdentifierName("MyType2"));
            var pMyType3 = Parameter(Identifier("abc")).WithType(IdentifierName("MyType3"));

            var list1 = new ParameterListTKey(new List<ParameterSyntax>() { pString, pBool, pMyType });
            var list2 = new ParameterListTKey(new List<ParameterSyntax>() { pMyType, pMyType2, pMyType3 });
            var list3 = new ParameterListTKey(new List<ParameterSyntax>() { pString });
            var list4 = new ParameterListTKey(new List<ParameterSyntax>() { });

            var dictioon = new Dictionary<ParameterListTKey, int>()
            {
                [list1] = 1,
                [list2] = 1,
                [list3] = 1,
                [list4] = 1
            };

            var list01 = new ParameterListTKey(new List<ParameterSyntax>() { pString, pBool, pMyType });
            var list02 = new ParameterListTKey(new List<ParameterSyntax>() { pBool, pMyType, pString });
            var list03 = new ParameterListTKey(new List<ParameterSyntax>() { pMyType, pMyType2 });
            var list04 = new ParameterListTKey(new List<ParameterSyntax>() { pBool });
            var list05 = new ParameterListTKey(new List<ParameterSyntax>() { });

            Assert.AreEqual(dictioon.ContainsKey(list01), true);
            Assert.AreEqual(dictioon.ContainsKey(list02), false);
            Assert.AreEqual(dictioon.ContainsKey(list03), false);
            Assert.AreEqual(dictioon.ContainsKey(list04), false);
            Assert.AreEqual(dictioon.ContainsKey(list05), true);
        }


        [TestMethod]
        public void ListFillComparisonTest2()
        {        
            var stringLiteral = LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("get_name"));
            var callInvocation = InvocationExpression(IdentifierName("Call")).AddArgumentListArguments(Argument(stringLiteral));
            var expressionStatement = ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName("Name"), callInvocation));
            var expressionsStatement = new List<ExpressionStatementSyntax> { expressionStatement };
            var expressionStatement2 = ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName("N22222222222222"), callInvocation));
            var expressionsStatement2 = new List<ExpressionStatementSyntax> { expressionStatement2 };

            var pString = Parameter(Identifier("abc")).WithType(IdentifierName("string"));
            var pBool = Parameter(Identifier("abc")).WithType(IdentifierName("bool"));
            var pMyType = Parameter(Identifier("abc")).WithType(IdentifierName("MyType"));

            var constructorCollection = new Dictionary<ParameterListTKey, List<ExpressionStatementSyntax>>()
            {
                [new ParameterListTKey()] = new List<ExpressionStatementSyntax> { expressionStatement },
                [new ParameterListTKey(new List<ParameterSyntax>() { pString, pBool, pMyType })] = new List<ExpressionStatementSyntax> { expressionStatement }
            };

            var isContainsKey = constructorCollection.ContainsKey(new ParameterListTKey());

            if (isContainsKey)
                constructorCollection[new ParameterListTKey()].Add(expressionStatement2);
            else
                constructorCollection.Add(new ParameterListTKey(), expressionsStatement2);

            Assert.AreEqual(isContainsKey, true);
            Assert.AreEqual(constructorCollection[new ParameterListTKey()].Count, 2);
            Assert.AreEqual(constructorCollection.Count, 2);
    //----------------------------------------------------------------------------------------------------------
            var rr = new ParameterListTKey(new List<ParameterSyntax>(new List<ParameterSyntax>() { pString }));
            isContainsKey = constructorCollection.ContainsKey(rr);

            if (isContainsKey)
                constructorCollection[rr].Add(expressionStatement);
            else
                constructorCollection.Add(rr, expressionsStatement);

            Assert.AreEqual(isContainsKey, false);
            Assert.AreEqual(constructorCollection[rr].Count, 1);
            Assert.AreEqual(constructorCollection.Count, 3);
            //----------------------------------------------------------------------------------------------------------
            var rr2 = new ParameterListTKey(new List<ParameterSyntax>(new List<ParameterSyntax>() { pString }));
            var expressionsStatement3 = new List<ExpressionStatementSyntax> { expressionStatement, expressionStatement2 };

            if (constructorCollection.ContainsKey(rr2))
            {
                var value = constructorCollection[rr2].Concat(expressionsStatement3).ToList();
                constructorCollection[rr2] = value;
            }
            else
                constructorCollection.Add(rr2, expressionsStatement3);

            Assert.AreEqual(constructorCollection.ContainsKey(rr2), true);
            Assert.AreEqual(constructorCollection[rr2].Count, 3);
            Assert.AreEqual(constructorCollection.Count, 3);
        }
    }
}
