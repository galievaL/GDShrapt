using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_OtherTypes : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test1()
        {
            var code = @"
class_name Builder

var name = my_method()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Name;
        public Builder()
        {
            Name = Call(""my_method"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test2()
        {
            var code = @"
class_name Builder

var name := my_method()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Name;
        public Builder()
        {
            Name = Call(""my_method"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test3()
        {
            // Если тип не выводится из правой части, то это Variant
            var code = @"
class_name Builder

var value := some_func()
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
            Value = Call(""some_func"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test4()
        {
            var code = @"
class_name Builder

var name: String = my_method()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string Name;
        public Builder()
        {
            Name = Call(""my_method"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test5()
        {
            var code = @"
class_name Builder

var value := get_value()

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
        public double Value;
        public Builder() 
        {
            Value = Get_value();
        }
        public double Get_value()
        {
            return 0.0;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test6()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var position = Hhhh(11.5, 20)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Variant Position;
        public HTerrainDataSaver()
        {
            Position = Call(""Hhhh"", 11.5, 20L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test7()
        {
            var code = @"
    tool
    class_name HTerrainDataSaver
    extends ResourceFormatSaver

    var position := Hhhh(11.5, 20)
    ";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Variant Position;
        public HTerrainDataSaver()
        {
            Position = Call(""Hhhh"", 11.5, 20L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_OtherTypes_Test8____________________()//ToDo: Дождаться того, когда Дима продумает решение для этого случая
        {
            var code = @"
class_name Builder

const name = get_name()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public static readonly Variant Name;
        public Builder()
        {
            Name = Call(""get_name"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
