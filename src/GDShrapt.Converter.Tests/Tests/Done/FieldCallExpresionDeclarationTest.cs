using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.Tests.Done
{
    [TestClass]
    public class FieldCallExpresionDeclarationTest : Test
    {
        [TestMethod]
        public void FieldCallExpresionDeclarationTest1()
        {
            var code = @"
class_name Builder

var value := Vector2().rotated(90)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value = new Vector2().Rotated(90L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldCallExpresionDeclarationTest2()
        {
            //если первый метод Call, то остальные тоже Call
            var code = @"
class_name Builder

var value := myMethod(""vector"").rotated(90).getPosition(1, 2, 3)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Value;
        public Builder()
        {
            Value = Call(""MyMethod"", ""vector"").Call(""Rotated"", 90L).Call(""GetPosition"", 1L, 2L, 3L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldCallExpresionDeclarationTest3()
        {
            //если первый метод Call, то остальные тоже Call
            var code = @"
class_name Builder

var value := myMethod(""vector"").getPosition(1, 2, 3)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Value;
        public Builder()
        {
            Value = Call(""MyMethod"", ""vector"").Call(""GetPosition"", 1L, 2L, 3L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldCallExpresionDeclarationTest4()
        {
            var code = @"
class_name Builder

var value := Vector2(0, random(random())).updated()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value = new Vector2(0L, Call(""Random"", Call(""Random""))).Updated();
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
