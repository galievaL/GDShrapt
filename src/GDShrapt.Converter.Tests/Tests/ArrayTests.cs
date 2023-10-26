using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class ArrayTests : Test
    {
        [TestMethod]
        public void ArrayTest1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Массивы
var numbers = [1, 2, 3, 4, 5]
var names = [""Alice"", ""Bob"", ""Charlie""]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public long[] numbers =
        {
            1L,
            2L,
            3L,
            4L,
            5L
        };
        public string[] names =
        {
            ""Alice"",
            ""Bob"",
            ""Charlie""
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void ArrayTest2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Массивы
var num = [2, 0, 0.0, 1.79769e308]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public double[] num =
        {
            2L,
            0L,
            0,
            1.79769E+308
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void ArrayTest3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Массивы
var boo = [true, false, false, true]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public bool[] boo =
        {
            true,
            false,
            false,
            true
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void ArrayTest4()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var vectors = [Vector2(0.5, 0.8), Vector2I(0, 1), Vector2(33, 22)]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Variant[] vectors =
        {
            Variant.CreateFrom(new Vector2(0.5, 0.8)),
            Variant.CreateFrom(new Vector2I(0L, 1L)),
            Variant.CreateFrom(new Vector2(33L, 22L))
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void ArrayTest5()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var variants = [Plane(1,2,3,4), Vector2(0, 1), [2, 0, 0.0, 1.79769e308]]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class TestClass
    {
        public Variant[] variants =
        {
            Variant.CreateFrom(new Plane(1, 2, 3, 4)),
            Variant.CreateFrom(new Vector2(0, 1)),
            Variant.CreateFrom(new double[]{ 2, 0, 0.0, 1.79769e308 })
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void ArrayTest6()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var variants = [""Hello"", 2, 0, 0.0, 1.79769e308, nil]
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class TestClass
    {
        public Variant[] variants =
        {
            Variant.CreateFrom(""Hello""),
            Variant.CreateFrom(2),
            Variant.CreateFrom(0),
            Variant.CreateFrom(0.0),
            Variant.CreateFrom(1.79769e308),
            Variant.From((object)null)
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
