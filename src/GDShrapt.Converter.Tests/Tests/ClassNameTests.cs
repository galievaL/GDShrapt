using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class ClassNameTests : Test
    {
        [TestMethod]
        public void ClassNameTest1()
        {
            var code = @"
tool
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
    public class TestClass : ResourceFormatSaver
    {
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ClassNameTest2()
        {
            var code = @"
tool
class_name 123H+=Ter^5r3_-ain-DataSaver
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
    public class TestClass : ResourceFormatSaver
    {
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }

        [TestMethod]
        public void ClassNameTest3()
        {
            var code = @"
tool
extends ResourceFormatSaver
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = "Generated",
                ClassName = "123Te@&s-t @_Class",
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
    public class _123Test_Class : ResourceFormatSaver
    {
    }
}";

            Debug.WriteLine("Сгенерированный код:\n" + csharpCode + "\n");

            Assert.AreEqual(cshC, csharpCode);
        }
    }
}
