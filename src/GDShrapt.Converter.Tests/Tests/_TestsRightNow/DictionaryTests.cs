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
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver

# Словари
var player_scores = { ""Alice"": 1000, ""Bob"": 2000, ""Charlie"": 1500}
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Dictionary = Godot.Collections.Dictionary;

namespace Generated
{
    [Tool]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
        public Dictionary Player_scores = new Dictionary
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
