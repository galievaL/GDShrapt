using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class FieldDeclarationTests_Const : Test
    {
        [TestMethod]
        public void FieldDeclarationTests_Const_Test1()
        {
            var code = @"
class_name Builder
const DOUBLE_PI = PI * 2.0 + 1 / 6 * Inf;
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double DOUBLE_PI = Mathf.Pi * 2 + 1L / 6L * Mathf.Inf;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test1_1()
        {
            var code = @"
class_name Builder
const DOUBLE_TAU = TAU * 2.0
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double DOUBLE_TAU = Mathf.Tau * 2.0;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test1_2()
        {
            var code = @"
class_name Builder
const DOUBLE_INF = INF * 2.0
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double DOUBLE_INF = Mathf.Inf * 2.0;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test1_3()
        {
            var code = @"
class_name Builder
const DOUBLE_NAN = NAN
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double DOUBLE_NAN = Mathf.NaN * 2.0;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test2()
        {
            var code = @"
class_name Builder
const NUMBER = 1.234
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double NUMBER = 1.234;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test3()
        {
            var code = @"
class_name Builder
const NUMBER = Sin(20)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double NUMBER = Mathf.Sin(20);
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test4()
        {
            var code = @"
class_name Builder
const NUMBER = NAN
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public const double NUMBER = Mathf.NaN;
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test5()
        {
            var code = @"
class_name Builder
const NUMBER = 1.234 + some_f()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public static readonly double NUMBER = 1.234 + Some_f();
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void FieldDeclarationTests_Const_Test6()
        {
            var code = @"
class_name Builder
const Name = get_name()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public static readonly Variant Name = Get_name();
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
