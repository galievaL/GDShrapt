using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    public static class ValidateTypeAndNameHelper
    {
        public static List<string> SharpTypesList = new List<string>()
        {
            "bool", "byte", "sbyte", "char", "decimal", "double",
            "float", "int", "uint", "long", "ulong", "short",
            "ushort", "object", "string", "void"
        };

        public static List<string> GodotTypesList = new List<string>()
        {
            "Vector2", "Vector3", "Vector4", "Rect2", 
            "Vector2I", "Vector3I", "Vector4I", "Rect2I"
        };

        public static string GetValidateClassName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название поля не может быть пустым.");

            name = ValidateAllLetter(name);
            name = RenameIsKeywordWord(name);
            name = ValidateFirstLetter(name);

            return name;
        }

        public static string GetValidateFieldName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название поля не может быть пустым.");

            name = ValidateAllLetter(name);
            name = RenameIsKeywordWord(name);
            name = ValidateFirstLetter(name);

            return name;
        }

        public static string GetTypeAdaptationToStandartMethodsType(string typeName)
        {
            var allTypes = SharpTypesList.Concat(GodotTypesList);
            var type = allTypes.Where(t => string.Equals(typeName, t, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            return type == null ? typeName : type;
        }

        public static bool IsItSharpType(string typeName)
        {
            var type = SharpTypesList.Where(t => string.Equals(typeName, t, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return type != null;
        }

        public static bool IsItGodotType(string typeName)
        {
            var type = GodotTypesList.Where(t => string.Equals(typeName, t, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return type != null;
        }

        static string ValidateFirstLetter(string name)
        {
            if (name[0] != '_' && name[0] != '@' && !char.IsLetter(name[0]))
                name = "_" + name;
            else if (!char.IsUpper(name[0]) && !ParseToken(name).IsKeyword())
                name = char.ToUpper(name[0]) + name.Substring(1);

            return name;
        }

        static string ValidateAllLetter(string name)
        {
            return Regex.Replace(name, "[^a-zA-Zа-яА-Я0-9_]", string.Empty);
        }

        static string RenameIsKeywordWord(string name)
        {
            if (ParseToken(name).IsKeyword())
                name = "@" + name;

            return name;
        }
    }
}
