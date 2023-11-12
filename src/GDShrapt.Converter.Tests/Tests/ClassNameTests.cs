using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class ClassNameTests : Test
    {
        [TestMethod]
        public void ClassNameTest1()
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
        public void ClassNameTest2()
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
        public void ClassNameTest3()
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
        public void ClassNameTest4()
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
    }
}
