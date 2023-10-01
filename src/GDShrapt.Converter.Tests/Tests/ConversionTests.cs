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
        public const int ANSWER = ""42"";
        public const string THE_NAME = ""Charly"";
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
        public void ConversionTest4()
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

