using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_Array : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_Array_Test1()
        {
            var code = @"
class_name Builder
var array := [1.23, 0, ""Hello""]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Array = Godot.Collections.Array;

namespace Generated
{
    public class Builder
    {
        public Array Array = new Array() { 1.23, 0, ""Hello"" };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Array_Test2()
        {
            var code = @"
class_name Builder
var array = [987, 98]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Array = Godot.Collections.Array;

namespace Generated
{
    public class Builder
    {
        public Array Array = new Array() { 987, 98 };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

    }
}
