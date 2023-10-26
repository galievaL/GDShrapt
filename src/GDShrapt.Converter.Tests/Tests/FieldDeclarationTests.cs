using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class FieldDeclarationTests : Test
    {
        [TestMethod]
        public void FieldDeclarationTest1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Константы
const ANSWER = 42
const THE_NAME = ""Charly""

# Булевы переменные
var is_visible = true
var is_active = false

# Числовые переменные
var health = 100
var speed = 55

# Строковые переменные
var name = ""Player""
var greeting = ""Hello, world!""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public const long ANSWER = 42L;
        public const string THE_NAME = ""Charly"";
        public bool is_visible = true;
        public bool is_active = false;
        public long health = 100L;
        public long speed = 55L;
        public string name = ""Player"";
        public string greeting = ""Hello, world!"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTest2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var a = 100000
var b = 555.5
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public long a = 100000L;
        public double b = 555.5;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTest3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Векторы
var position = Vector2(10, 20)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Vector2 position = new Vector2(10L, 20L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTest4()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Векторы
var position = Vector2(10, 20)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Vector2 position = new Vector2(10L, 20L);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTest5()
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
        public GodotObject HTerrainData = ResourceLoader.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
