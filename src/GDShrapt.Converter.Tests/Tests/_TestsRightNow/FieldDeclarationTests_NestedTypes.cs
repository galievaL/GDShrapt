using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_NestedTypes : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test1()
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

            CompareCodeStrings(csharpCodeExpectedResult, csharpCode);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test2()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var position = MyVector2(17, MyMethod(15), 20)
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
            Position = Call(""MyVector2"", 17L, Call(""MyMethod"", 15L), 20L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test3()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

var position = MyMethod(""string"", MyMethod(15), 20)
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
            Position = Call(""MyMethod"", ""string"", Call(""MyMethod"", 15L), 20L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test4()
        {
            var code = @"
class_name Builder

var value := Vector2(get_x(), get_y())
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value;
        public Builder()
        {
            Value = new Vector2(Call(""Get_x""), Call(""Get_y""));
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test5()
        {
            var code = @"
class_name Builder

var value := Vector2(make(5), random())
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value;
        public Builder()
        {
            Value = new Vector2(Call(""Make"", 5L), Call(""Random""));
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_NestedTypes_Test6()
        {
            var code = @"
class_name Builder

var value := Vector2(0, random())

func random():
    return 10
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Vector2 Value;
        public Builder()
        {
            Value = new Vector2(0L, Random());
        }

        public Variant Random()
        {
            return 10L;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
