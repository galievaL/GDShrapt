using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void ConversionTest()
        {
            var code = @"
tool
class_name HTerrainDataSaver
extends ResourceFormatSaver
";

            var parser = new GDScriptReader();
            var declaration = parser.ParseFileContent(code);

            var visitor = new CSharpGeneratingVisitor2(new ConversionSettings()
            {
                Namespace = "Generated",
                FileName = "TestClass.cs",
                ConvertGDScriptNamingStyleToSharp = true
            });

            var treeWalker = new GDTreeWalker(visitor);
            treeWalker.WalkInNode(declaration);

            var csharpCode = visitor.BuildCSharpNormalisedCode();
            var cshC = @"using Godot;

namespace Generated
{
    [Tool]
    [ClassName(""HTerrainDataSaver"")]
    public class HTerrainDataSaver : ResourceFormatSaver
    {
    }
}";

            Assert.AreEqual(cshC, csharpCode);
        }
    }
}

/*
 using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CodeGenerator
{
    public void GenerateClass()
    {
        var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Generated")).NormalizeWhitespace();

        var @class = SyntaxFactory.ClassDeclaration("HTerrainDataSaver")
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ResourceFormatSaver")))
            .AddAttributeLists(
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("Tool")))),
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("ClassName"),
                        SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("HTerrainDataSaver")))))))));

        var compilationUnit = SyntaxFactory.CompilationUnit()
            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Godot")))
            .AddMembers(@namespace.AddMembers(@class))
            .NormalizeWhitespace();

        var code = compilationUnit.ToFullString();
    }
}
Этот код создает новый класс HTerrainDataSaver, который наследуется от ResourceFormatSaver и имеет атрибуты Tool и ClassName. 
Класс находится в пространстве имен Generated.
 */
