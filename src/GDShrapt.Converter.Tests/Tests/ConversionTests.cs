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

        [TestMethod]
        public void ConversionTest3()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# Called when the node enters the scene tree for the first time.
func _ready():
	add(5, 6) # Prints 11 to Output window
	var sum = get_sum(2, 4) # Sets sum to 6
	var my_int = add_ints(sum, 4) # Sets my_int to 10
	my_int = times_2(my_int) # sets my_int to 20
	move_x(self, my_int) # Move this node 20 pixels along x axis
	move_x(self) # Move by the default value
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
        public void ConversionTest4()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# This function has no return value
func add(a, b):
	print(a + b)
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
        public void ConversionTest5()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# This function returns a value
func get_sum(a, b):
	return a + b
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
        public void ConversionTest6()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

func add_ints(a: int, b: int):
    var n1 = a - b
    var n2 = ""string""
	return a + b
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
        public void ConversionTest7()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# This function will only accept integer arguments
func add_ints(a: int, b: int):
	return a + b
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
        public void ConversionTest8()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# Generate an error if the return value is not an int
func times_2(n) -> int:
	return 2 * n
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
        public void ConversionTest9()//todo: написать решение для шарпа
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends Node2D

# This function modifies an object that is passed by reference
func move_x(node: Node2D, dx = 1.5):
	node.position.x += dx
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
        public void ConversionTest10()
        {
            var code = @"
class_name Builder

func _process2(_delta):
  for i in range(-20, 20, 1):
    var f : float = float(i) / 100.0
    print([f, fmod(f, 0.1), fposmod(f, 0.1), f - 0.1 * floor(f / 0.1)])
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public void _provess2(Variant delta)
        {
            foreach(var i in Enumerable.Range(-20L, 20L, 1L))
            {
                double f = ((double)i) / 100.0;

                GD.Print(new Array() { f, f % 0.1, Mathf.PosMod(f, 0.1), f - 0.1 * Mathf.Floor(f / 0.1) });
            }
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}

