﻿using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_Array : Test
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
using Array = Godot.Collections.Array;

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

        [TestMethod]
        public void FieldDeclarationTests_Array_Test3()
        {
            var code = @"
class_name Builder

var a = [10, 20, 30]
var b = str(a)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Array = Godot.Collections.Array;

namespace Generated
{
    public class Builder
    {
        public double a = new Array[10, 20, 30];
        public string b = a.ToString();
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

    }
}
