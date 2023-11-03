using GDShrapt.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace GDShrapt.Converter.Tests.Tests
{
    [TestClass]
    public class MethodDeclarationTest : Test
    {
        [TestMethod]
        public void MethodDeclarationTest1()
        {
            var code = @"
class_name Builder

func build():
    return Home.new()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class Builder
    {
        public Home Build()
        {
            return new Home();
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest2()
        {
            var code = @"
class_name MyMath

func sum(a, b):
    return a + b;
";

            // Add будет метод расширение для Variant, который я напишу отдельно. Называем такие методы также, как называется оператор в парсере. 
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class MyMath
    {
        public Variant Sum(Variant a, Variant b)
        {
            return a.Add(b);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest3()
        {
            var code = @"
class_name MyMath

func sum(a: int, b: int):
    return a + b;
";

            // Если типы известны, сохраняем оператор
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class MyMath
    {
        public long Sum(long a, long b)
        {
            return a + b;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest4()
        {
            var code = @"
class_name MyMath

func sum(a, b) -> int:
    return a + b;
";

            // Если выходной тип метода известен, то предугадываем входящие типы, если возможно
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class MyMath
    {
        public long Sum(long a, long b)
        {
            return a + b;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest5()
        {
            var code = @"
class_name Helper

func get_element(a, i):
    return a[i];
";

            // Обращение по индексу или названию возможно у любого типа в Godot. Вписываем мой метод Indexer, который будет написан отдельно как метод расширение для Variant
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class Helper
    {
        public Variant GetElement(Variant a, Variant b)
        {
            return a.Indexer(b);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest6()
        {
            var code = @"
class_name Helper

func create_values():
    return { 1:""Masha"", 2:""Dasha"", 3:""Sasha"" }
";

            // Тут создаем Dictionary из Godot. Не путать с сишарпоским.
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Generated
{
    public class Helper
    {
        public Dictionary CreateValues()
        {
            return new Dictionary() 
            {
                { 1, ""Masha"" }, 
                { 2, ""Dasha"" },
                { 3, ""Sasha"" }
            };
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest7()
        {
            var code = @"
class_name Helper

func create_values():
    return [1, 2, 3]
";

            // Тут создаем Array из Godot. Также не путать с сишарпоским.
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Generated
{
    public class Helper
    {
        public Array CreateValues()
        {
            return new Array() { 1, 2, 3 };
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest8()
        {
            var code = @"
class_name Helper

func create_values():
    if 2 + 2 == 4:
        return { 1:""Masha"", 2:""Dasha"", 3:""Sasha"" };
    return [1, 2, 3]
";

            // Если тип трудно определить, то ставим Variant
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Generated
{
    public class Helper
    {
        public Variant CreateValues()
        {
            if (2 + 2 == 4)
                return new Dictionary() 
                {
                    { 1, ""Masha"" }, 
                    { 2, ""Dasha"" },
                    { 3, ""Sasha"" }
                };
            return new Array() { 1, 2, 3 };
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest9()
        {
            var code = @"
class_name Helper

func create_value():
    var a = 2 + 2
    return a
";

            // Если тип можно вывести по коду, то выводим
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class Helper
    {
        public long CreateValue()
        {
            var a = 2 + 2;
            return a;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclarationTest10()
        {
            var code = @"
class_name Helper

func create_value(parameter):
    var a = 2 + parameter
    return a
";

            // Если тип можно вывести по коду, то выводим
            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;
using Godot;

namespace Generated
{
    public class Helper
    {
        public Variant CreateValue(Variant parameter)
        {
            var a = 2.Add(parameter);
            return a;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
