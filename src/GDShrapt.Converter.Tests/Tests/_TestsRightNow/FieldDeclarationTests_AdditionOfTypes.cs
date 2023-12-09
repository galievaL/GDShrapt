using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_AdditionOfTypes : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_AdditionOfTypes_Test1()
        {
            var code = @"
class_name Builder

var name := ""Hello "" + "" World""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string Name = ""Hello "" + "" World"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_AdditionOfTypes_Test2()
        {
            var code = @"
class_name Builder

var value := 2 + 0.33
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public double Value = 2L + 0.33;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_AdditionOfTypes_Test3()
        {
            var code = @"
class_name Builder

var value := get_value() + get_value2()

func get_value(): 
    return 0.0
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
            Value = GetValue().Add(Call(""get_value2""));
        }
        public Variant GetValue()
        {
            return 0;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_AdditionOfTypes_Test4()
        {
            var code = @"
class_name Builder

var value: Float = get_value() + get_value2()

func get_value() -> Float: 
    return 0.0

func get_value2() -> Int: 
    return 0
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public double Value;

        public Builder() 
        {
            Value = GetValue() + GetValue2();
        } 

        public double GetValue()
        {
            return 0;
        }

        public long GetValue2()
        {
            return 0;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
