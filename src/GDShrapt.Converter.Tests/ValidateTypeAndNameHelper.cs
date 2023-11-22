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
        public static Dictionary<string, string> VariantTypesToLowerAndTheirEquivalentSharpTypes = new Dictionary<string, string>()
        {
            ["nil"] = "null",
            ["bool"] = "bool",
            ["int"] = "long",
            ["float"] = "double",
            ["string"] = "string",
            ["vector2"] = "Vector2",
            ["vector2i"] = "Vector2I",
            ["rect2"] = "Rect2",
            ["rect2i"] = "Rect2I",
            ["vector3"] = "Vector3",
            ["vector3i"] = "Vector3I",
            ["transform2d"] = "Transform2D",
            ["vector4"] = "Vector4",
            ["vector4i"] = "Vector4I",
            ["plane"] = "Plane",
            ["quaternion"] = "Quaternion",
            ["aabb"] = "Aabb",
            ["basis"] = "Basis",
            ["transform3d"] = "Transform3D",
            ["projection"] = "Projection",
            ["color"] = "Color",
            ["stringname"] = "StringName",
            ["nodepath"] = "NodePath",
            ["rid"] = "Rid",
            ["object"] = "GodotObject ",
            ["callable"] = "Callable",
            ["signal"] = "Signal",
            ["dictionary"] = "Collections.Dictionary",
            ["array"] = "Collections.Array",
            ["packedbytearray"] = "byte[]",
            ["packedint32array"] = "int[]",
            ["packedint64array"] = "long[]",
            ["packedfloat32array"] = "float[]",
            ["packedfloat64array"] = "double[]",
            ["packedstringarray"] = "string[]",
            ["packedvector2array"] = "Vector2[]",
            ["packedvector3array"] = "Vector3[]",
            ["packedcolorarray"] = "Color[]"
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
            var type = typeName.ToLower();
            if (VariantTypesToLowerAndTheirEquivalentSharpTypes.ContainsKey(type))
                return VariantTypesToLowerAndTheirEquivalentSharpTypes[type];

            return typeName;
        }

        public static bool IsItGodotType(string typeName)
        {
            return VariantTypesToLowerAndTheirEquivalentSharpTypes.ContainsKey(typeName.ToLower());
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
