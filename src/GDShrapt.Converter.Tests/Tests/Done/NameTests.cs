using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests.Done
{
    [TestClass]
    public class NameTests : Test
    {
        [TestMethod]
        public void NameTest1()
        {
            var code = @"
tool
extends ResourceFormatSaver
";
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class TestClass : ResourceFormatSaver
    {
    }
}";
            var @namespace = "Generated";
            var className = "TestClass";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code, @namespace, className);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NameTest2()
        {
            var code = @"
tool
class_name 123H+=Ter^5r3_-ain-DataSaver
extends ResourceFormatSaver
";
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class TestClass : ResourceFormatSaver
    {
    }
}";
            var @namespace = "Generated";
            var className = "TestClass";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code, @namespace, className);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NameTest3()
        {
            var code = @"
tool
extends ResourceFormatSaver
";
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class _123Test_Class : ResourceFormatSaver
    {
    }
}";
            var @namespace = "Generated";
            var className = "123Te@&s-t @_Class";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code, @namespace, className);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NameTest4()
        {
            var code = @"
tool
extends ResourceFormatSaver
";
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class Test_class : ResourceFormatSaver
    {
    }
}";
            var @namespace = "Generated";
            var className = "test_class";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code, @namespace, className);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NameTest5()
        {
            var code = @"
class_name Builder

var const = Vector2(0, 0)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant @const = new Vector2(0L, 0L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NameTest6()
        {
            var code = @"
class_name Builder

var class := ""строка""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string @class = ""строка"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
