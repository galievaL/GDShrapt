using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.DimasTests
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
        public Vector2 Value = new Vector2().Rotated(90);
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
        public Vector2 Value;

        public Builder() 
        {
            Value = new Vector2(0, Call(""random"", Call(""random""))).Updated();
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
