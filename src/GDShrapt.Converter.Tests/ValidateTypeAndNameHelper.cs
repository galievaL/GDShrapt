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
        public static Dictionary<string, string> VariantTypesAndTheirEquivalentSharpTypes = new Dictionary<string, string>()
        {
            ["Nil"] = "null",
            ["Bool"] = "bool",
            ["Int"] = "long",
            ["Float"] = "double",
            ["String"] = "string",
            ["Vector2"] = "Vector2",
            ["Vector2i"] = "Vector2I",
            ["Rect2"] = "Rect2",
            ["Rect2i"] = "Rect2I",
            ["Vector3"] = "Vector3",
            ["Vector3i"] = "Vector3I",
            ["Transform2D"] = "Transform2D",
            ["Vector4"] = "Vector4",
            ["Vector4i"] = "Vector4I",
            ["Plane"] = "Plane",
            ["Quaternion"] = "Quaternion",
            ["Aabb"] = "Aabb",
            ["Basis"] = "Basis",
            ["Transform3D"] = "Transform3D",
            ["Projection"] = "Projection",
            ["Color"] = "Color",
            ["StringName"] = "StringName",
            ["NodePath"] = "NodePath",
            ["Rid"] = "Rid",
            ["Object"] = "GodotObject ",
            ["Callable"] = "Callable",
            ["Signal"] = "Signal",
            ["Dictionary"] = "Collections.Dictionary",
            ["Array"] = "Collections.Array",
            ["PackedByteArray"] = "byte[]",
            ["PackedInt32Array"] = "int[]",
            ["PackedInt64Array"] = "long[]",
            ["PackedFloat32Array"] = "float[]",
            ["PackedFloat64Array"] = "double[]",
            ["PackedStringArray"] = "string[]",
            ["PackedVector2Array"] = "Vector2[]",
            ["PackedVector3Array"] = "Vector3[]",
            ["PackedColorArray"] = "Color[]"
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
            if (VariantTypesAndTheirEquivalentSharpTypes.ContainsKey(typeName))
                return VariantTypesAndTheirEquivalentSharpTypes[typeName];

            return typeName;
        }

        public static bool IsItGodotType(string typeName)
        {
            return VariantTypesAndTheirEquivalentSharpTypes.ContainsKey(typeName);
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
