using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class DictionaryTests : Test
    {
        [TestMethod]
        public void DictionaryTest1()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Словари
var player_scores = { ""Alice"": 1000, ""Bob"": 2000, ""Charlie"": 1500}
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Dictionary<string, int> playerScores = new Dictionary<string, int>
        {
            {""Alice"", 1000},
            {""Bob"", 2000},
            {""Charlie"", 1500}
        };
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
