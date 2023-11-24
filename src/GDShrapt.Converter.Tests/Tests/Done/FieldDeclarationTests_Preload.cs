using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests.Done
{
    [TestClass]
    public class FieldDeclarationTests_Preload : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_Preload_Test1()
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
        public static readonly Resource HTerrainData = GD.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Preload_Test2()
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
        public Resource HTerrainData = GD.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Preload_Test3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var HTerrainData := preload(""./ hterrain_data.gd"")
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Resource HTerrainData = GD.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Preload_Test4()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var HTerrainData: MyType = preload(""./ hterrain_data.gd"")
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public MyType HTerrainData = GD.Load(""./ hterrain_data.gd"");
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
