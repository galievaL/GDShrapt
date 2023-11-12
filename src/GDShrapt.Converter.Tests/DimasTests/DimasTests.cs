using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.DimasTests
{
    [TestClass]
    public class DimasTests : Test
    {
        [TestMethod]
        public void ArrayDeclaration_Test1()
        {
            var code = @"
class_name Builder

var value := [0, 1, 2]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot.Collections;

namespace Generated
{
    public class Builder
    {
        public Array Value = new Array() { 0, 1, 2 };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

    }
}
