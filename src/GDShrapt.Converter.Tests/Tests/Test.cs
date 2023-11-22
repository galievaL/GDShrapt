using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;

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

        internal static void CompareCodeStrings(string s1, string s2)
        {
            s1 = s1.Replace("\r\n", "\n").Replace("    ", "\t");
            s2 = s2.Replace("\r\n", "\n").Replace("    ", "\t");

#if DEBUG
            bool diffFound = false;
            var original = new StringBuilder();
            var other = new StringBuilder();

            if (s1.Length == s2.Length)
            {
                for (int i = 0; i < s1.Length; i++)
                {
                    var ch1 = s1[i];
                    var ch2 = s2[i];

                    if (ch1 != ch2)
                        diffFound = true;

                    if (diffFound)
                    {
                        original.Append(ch1);
                        other.Append(ch2);
                    }
                }
            }
#endif
            Assert.AreEqual(s1, s2);
        }
    }
}
