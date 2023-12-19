using GDShrapt.Converter.Tests.Tests;
using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class DictionaryTests : Test
    {
        [TestMethod]
        public void DictionaryTest1()
        {
            var code = @"
class_name Builder

# Словари
var player_scores = { ""Alice"": 1000, ""Bob"": 2000, ""Charlie"": 1500}
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Dictionary = Godot.Collections.Dictionary;

namespace Generated
{
    public class Builder
    {
        public Dictionary Player_scores = new Dictionary
        {
            {
                ""Alice"",
                1000L
            },
            {
                ""Bob"",
                2000L
            },
            {
                ""Charlie"",
                1500L
            }
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
