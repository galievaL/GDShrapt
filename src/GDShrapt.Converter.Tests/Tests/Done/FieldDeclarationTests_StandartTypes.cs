using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests.Done
{
    [TestClass]
    public class FieldDeclarationTests_StandartTypes : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_StandartTypes_Test1()
        {
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
        public void FieldDeclarationTests_StandartTypes_Test2()
        {
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
        public void FieldDeclarationTests_StandartTypes_Test3()
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
        public void FieldDeclarationTests_StandartTypes_Test4()
        {
            var code = @"
class_name Builder

const value = Vector2(0, 0)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public static readonly Vector2 Value = new Vector2(0L, 0L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}