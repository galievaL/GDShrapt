using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class Test
    {
        public string GetCSharpCodeConvertedFromGdScript(string gdScriptCode, string nameSpace = "Generated", string className = "TestClass")
        {
            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(gdScriptCode);

            var visitor = new CSharpGeneratingVisitor(new ConversionSettings()
            {
                Namespace = nameSpace,
                ClassName = className,
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            return visitor.BuildCSharpNormalisedCode();
        }
    }
}
