using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests.Done
{
    [TestClass]
    public class FieldDeclarationTests_SimpleTypes : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes_Consts()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const is_visible = true
const is_active = false

const health = 100
const speed = 55

const way = 189.9
const answer = 4.2

const name = ""Player""
const greeting = ""Hello, world!""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public const bool Is_visible = true;
        public const bool Is_active = false;
        public const long Health = 100L;
        public const long Speed = 55L;
        public const double Way = 189.9;
        public const double Answer = 4.2;
        public const string Name = ""Player"";
        public const string Greeting = ""Hello, world!"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes_Consts2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

const is_visible := true
const is_active := false

const health := 100
const speed := 55

const way := 189.9
const answer := 4.2

const name := ""Player""
const greeting := ""Hello, world!""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public const bool Is_visible = true;
        public const bool Is_active = false;
        public const long Health = 100L;
        public const long Speed = 55L;
        public const double Way = 189.9;
        public const double Answer = 4.2;
        public const string Name = ""Player"";
        public const string Greeting = ""Hello, world!"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var is_visible = true
var is_active = false

var health = 100
var speed = 55

var way = 189.9
var answer = 4.2

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
        public Variant Is_visible = true;
        public Variant Is_active = false;
        public Variant Health = 100L;
        public Variant Speed = 55L;
        public Variant Way = 189.9;
        public Variant Answer = 4.2;
        public Variant Name = ""Player"";
        public Variant Greeting = ""Hello, world!"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var is_visible := true
var is_active := false

var health := 100
var speed := 55

var way := 189.9
var answer := 4.2

var name := ""Player""
var greeting := ""Hello, world!""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public bool Is_visible = true;
        public bool Is_active = false;
        public long Health = 100L;
        public long Speed = 55L;
        public double Way = 189.9;
        public double Answer = 4.2;
        public string Name = ""Player"";
        public string Greeting = ""Hello, world!"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var a = 2200000000
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
        public Variant A = 2200000000L;
        public Variant B = 555.5;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes4()
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
        public void FieldDeclarationTests_SimpleTypes5()
        {
            // Если тип не выводится из правой части, то это Variant
            var code = @"
class_name Builder

var value = ""строка""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Value = ""строка"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_SimpleTypes6()
        {
            // Двоеточие перед равно означает, что тип выводится из правой части.
            var code = @"
class_name Builder

var value := ""строка""
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public string Value = ""строка"";
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}