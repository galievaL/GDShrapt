using GDShrapt.Converter.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GDShrapt.Converter.Tests.Tests2
{
    [TestClass]
    public class MethodDeclarationTest : Test
    {
        [TestMethod]
        public void MethodDeclaration_Test1()
        {
            var code = @"
class_name Builder

func build():
    return Home.new()
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Build()
        {
            return new Home();
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclaration_Test1_1()
        {
            var code = @"
class_name Builder

func build():
    return Home.new(1, 2, 3)
";

            var csharpCodeExpectedResult = @"using Godot;
using System;
using System.Linq;

namespace Generated
{
    public class Builder
    {
        public Variant Build()
        {
            return new Home(1L, 2L, 3L);
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclaration_Test2()
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
        public void MethodDeclaration_Test3()
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

namespace Generated
{
    public class MyMath
    {
        public Variant Sum(long a, long b)
        {
            return a + b;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }

        [TestMethod]
        public void MethodDeclaration_Test4()
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
        public void MethodDeclaration_Test5()
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
        public void MethodDeclaration_Test6()
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
using Godot.Collections;
using Dictionary = Godot.Collections.Dictionary;

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
        public void MethodDeclaration_Test7()
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
using Godot.Collections;
using Array = Godot.Collections.Array;

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
        public void MethodDeclaration_Test8()
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
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;

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
        public void MethodDeclaration_Test9()
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

namespace Generated
{
    public class Helper
    {
        public Variant CreateValue()
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
        public void MethodDeclaration_Test10()
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

namespace Generated
{
    public class Helper
    {
        public Variant CreateValue(Variant parameter)
        {
            var a = ((Variant)2).Add(parameter);
            return a;
        }
    }
}";
            var csharpCode = GetCSharpCodeConvertedFromGdScript(code);

            Assert.AreEqual(csharpCodeExpectedResult, csharpCode);
        }
    }
}
