using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.DimasTests
{
    [TestClass]
    public class FieldDeclarationTests : Test
    {
        [TestMethod]
        public void FieldDeclaration_Test1()
        {
            var code = @"
class_name Builder

var name := ""Hello""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string Name = ""Hello"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclaration_Test2()
        {
            var code = @"
class_name Builder

var name: String = get_name()
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
            Name = Call(""get_name"");
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclaration_Test2_2()
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
        public void FieldDeclaration_Test3()
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
            Value = GetValue();
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
        public void FieldDeclaration_Test4()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var position = Vector2(MyMethod(15), 20)
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
            Position = new Vector2(Call(""MyMethod"", 15L), 20L);    
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclaration_Test5_1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const HTerrainData = preload(""./ hterrain_data.gd"")
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public static readonly Resource HTerrainData = ResourceLoader.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclaration_Test5_2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var HTerrainData = preload(""./ hterrain_data.gd"")
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Resource HTerrainData = ResourceLoader.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclaration_Test6()
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
        public void FieldDeclaration_Test7()
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
    }
}
