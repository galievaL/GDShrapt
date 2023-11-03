using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class ConversionTests : Test
    {
        [TestMethod]
        public void ConversionTest1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
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

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    private const string HTerrainData = ""./ hterrain_data.gd"";

    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public PoolStringArray GetRecognizedExtensions(Resource res)
        {
            if (res != null && res is HTerrainData)
            {
                return new PoolStringArray { HTerrainData.META_EXTENSION };
            }
            return new PoolStringArray();
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}

