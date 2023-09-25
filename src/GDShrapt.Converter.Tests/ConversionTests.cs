using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var visitor = new CSharpGeneratingVisitor2(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass.cs",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;

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

const HTerrainData = preload(""./ hterrain_data.gd"")

func get_recognized_extensions(res):
	if res != null and res is HTerrainData:
		return PoolStringArray([HTerrainData.META_EXTENSION])
	return PoolStringArray()
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor2(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "TestClass",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;

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

