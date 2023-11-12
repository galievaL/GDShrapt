using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.DimasTests
{
    [TestClass]
    public class NonVariantFieldDeclarationTest : Test
    {
        [TestMethod]
        public void NonVariantFieldDeclaration_Test1()
        {
            // Двоеточие перед равно означает, что тип выводится из правой части.
            var code = @"
class_name Builder

var value := Vector2(0, 0)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value = new Vector2(0L, 0L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NonVariantFieldDeclaration_Test2()
        {
            // Если тип не выводится из правой части, то это Variant
            var code = @"
class_name Builder

var value = Vector2(0, 0)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Value = new Vector2(0L, 0L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NonVariantFieldDeclaration_Test3()
        {
            // Если в коде присутствует указание типа, то он сохраняется в C#
            var code = @"
class_name Builder

var value: Vector2i = Vector2(0, 0)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2I Value = new Vector2(0L, 0L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NonVariantFieldDeclaration_Test4()
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
        public void NonVariantFieldDeclaration_Test5()
        {
            // Двоеточие перед равно означает, что тип выводится из правой части.
            var code = @"
class_name Builder

var value := ""строка""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string Value = ""строка"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void NonVariantFieldDeclaration_Test6()
        {
            // Если тип не выводится из правой части, то это Variant
            var code = @"
class_name Builder

var value = ""строка""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Value = ""строка"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
