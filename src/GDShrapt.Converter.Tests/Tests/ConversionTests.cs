using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void ConversionTest()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ConversionTest2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const ANSWER = 42
const THE_NAME = ""Charly""

# Булевы переменные
var is_visible = true
var is_active = false

# Числовые переменные
var health = 100
var speed = 5.5

# Строковые переменные
var name = ""Player""
var greeting = ""Hello, world!""
";
            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
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
        public double speed = 5.5;
        public string name = ""Player"";
        public string greeting = ""Hello, world!"";
    }
}";
            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ConversionTest3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const ANSWER = 42
const THE_NAME = ""Charly""

# Булевы переменные
var is_visible = true
var is_active = false

# Числовые переменные
var health = 100
var speed = 5.5

# Строковые переменные
var name = ""Player""
var greeting = ""Hello, world!""
";
            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
using System;

public class HTerrainDataSaver : ResourceFormatSaver
{
    public const long ANSWER = 42;
    public const string THE_NAME = ""Charly"";
    public bool is_visible = true;
    public bool is_active = false;
    public long health = 100;
    public double speed = 5.5;
    public string name = ""Player"";
    public string greeting = ""Hello, world!"";
}";
            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ConversionTest4()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Булевы переменные
var is_visible = true
var is_active = false

# Числовые переменные
var health = 100
var speed = 5.5

# Строковые переменные
var name = ""Player""
var greeting = ""Hello, world!""

# Массивы
var numbers = [1, 2, 3, 4, 5]
var names = [""Alice"", ""Bob"", ""Charlie""]

# Словари
var player_scores = { ""Alice"": 1000, ""Bob"": 2000, ""Charlie"": 1500}

# Векторы
var position = Vector2(10, 20)

const ANSWER = 42
const THE_NAME = ""Charly""
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });
            
            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
using System;

public class HTerrainDataSaver : ResourceFormatSaver
{
    // Булевы переменные
    public bool is_visible = true;
    public bool is_active = false;

    // Числовые переменные
    public int health = 100;
    public double speed = 5.5;

    // Строковые переменные
    public string name = ""Player"";
    public string greeting = ""Hello, world!"";

    // Массивы
    public int[] numbers = {1, 2, 3, 4, 5};
    public string[] names = {""Alice"", ""Bob"", ""Charlie""};

    // Словари
    public Dictionary<string, int> playerScores = new Dictionary<string, int>
    {
        {""Alice"", 1000},
        {""Bob"", 2000},
        {""Charlie"", 1500}
    };

    // Векторы
    public Vector2 position = new Vector2(10, 20);

    // Константы
    public const int ANSWER = 42;
    public const string THE_NAME = ""Charly"";
}";

            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ConversionTest5()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const HTerrainData = preload(""./ hterrain_data.gd"")
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public const string HTerrainData = ""./ hterrain_data.gd"";
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ConversionTest6()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const HTerrainData = preload(""./ hterrain_data.gd"")

func get_recognized_extensions(res):
	if res != null and res is HTerrainData:
		return PoolStringArray([HTerrainData.META_EXTENSION])
	return PoolStringArray()
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    private const string HTerrainData = ""./ hterrain_data.gd"";

    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public override PoolStringArray GetRecognizedExtensions(Resource res)
        {
            if (res != null && res is HTerrainData)
            {
                return new PoolStringArray { HTerrainData.META_EXTENSION };
            }
            return new PoolStringArray();
        }
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }
    }
}

