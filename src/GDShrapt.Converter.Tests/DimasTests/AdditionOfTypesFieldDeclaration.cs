using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.DimasTests
{
    [TestClass]
    public class AdditionOfTypesFieldDeclaration : Test
    {
        [TestMethod]
        public void AdditionOfTypesFieldDeclaration_Test1()
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
        public void AdditionOfTypesFieldDeclaration_Test2()
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
        public double Value = 2 + 0.33;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void AdditionOfTypesFieldDeclaration_Test3()
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

        public double GetValue()
        {
            return 0.0;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void AdditionOfTypesFieldDeclaration_Test4()
        {
            var code = @"
class_name Builder

var value: Float = get_value() + get_value2()

func get_value(): 
    return 0.0

func get_value2(): 
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
            return 0.0;
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
