using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Reflection;

namespace GDShrapt.Converter.Tests
{
    public static class ValidateTypeAndNameHelper
    {
        public static string GetValidateClassName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название поля не может быть пустым.");

            name = RemoveExtraCharacters(name);
            name = RenameIsKeywordWord(name);
            name = ValidateFirstLetter(name);

            return name;
        }

        public static string GetValidateFieldName(this string name, ScopeType scopeType)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название поля не может быть пустым.");

            name = RemoveExtraCharacters(name);
            name = RenameIsKeywordWord(name);

            if (scopeType == ScopeType.Class || scopeType == ScopeType.MethodName)
                name = ValidateFirstLetter(name);
            else if (scopeType == ScopeType.LocalVariable)
            {
                if (name[0] != '_' && name[0] != '@' && !char.IsLetter(name[0]))
                    name = "_" + name;
            }

            return name;
        }

        public static string GetValidateMethodName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Название поля не может быть пустым.");

            name = RemoveExtraCharacters(name);
            name = RenameIsKeywordWord(name);

            if (name[0] != '_' && name[0] != '@' && !char.IsLetter(name[0]))
                name = "_" + name;

            return name;
        }

        //public static string GetTypeAdaptationToStandartMethodsType(string typeName)
        //{
        //    if (!IsStandartGodotType(typeName))
        //        return typeName;

        //    return GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower2[typeName.ToLower()].csharpEquivalent;
        //}

        public static GeneralType GetTypeAdaptationToStandartMethodsType(string typeName)
        {
            if (!IsStandartGodotType(typeName))
                return null;

            var kind = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower2[typeName.ToLower()].returnTypes;
            return new GeneralType(kind);
        }

        public static bool GetTypeAdaptationToStandartMethodsType(ref string typeName)
        {
            var isStandartGodotType = IsStandartGodotType(typeName);

            if (isStandartGodotType)
                typeName = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower2[typeName.ToLower()].csharpEquivalent;

            return isStandartGodotType;
        }

        public static bool IsStandartGodotType(string typeName)
        {
            return GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower2.ContainsKey(typeName.ToLower());
        }

        public static bool IsGodotFunctions(ref string functionsName, out GeneralType type)
        {
            type = null;
            var functionsNameLower = functionsName.ToLower();

            var b1 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstsToLower_TheirEquivalentAndReturnTypes.ContainsKey(functionsNameLower);
            var b2 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions.ContainsKey(functionsNameLower);
            var b3 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_NAequivalent.ContainsKey(functionsNameLower);
            var b4 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions.ContainsKey(functionsNameLower);
            var b5 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_NAequivalen.ContainsKey(functionsNameLower);

            if (!(b1 || b2 || b3 || b4 || b5))
                return false;

            var tuple = b1 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstsToLower_TheirEquivalentAndReturnTypes[functionsNameLower] :
                            b2 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions[functionsNameLower] :
                            b3 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_NAequivalent[functionsNameLower] :
                            b4 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions[functionsNameLower] :
                            b5 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_NAequivalen[functionsNameLower] :
                            throw new NotImplementedException();

            type = new GeneralType(tuple.returnTypes);
            functionsName = tuple.csharpEquivalent;

            return true;
        }

        public static bool IsGDScriptConsts(ref string functionsName, out GeneralType type)
        {
            type = null;
            var functionsNameLower = functionsName.ToLower();

            if (GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstsToLower_TheirEquivalentAndReturnTypes.ContainsKey(functionsNameLower))
            {
                var group = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstsToLower_TheirEquivalentAndReturnTypes[functionsNameLower];

                functionsName = group.csharpEquivalent;
                type = new GeneralType(group.returnTypes);

                return true;
            }

            return false;
        }
        /*
        static void Nn()
        {
            var types = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions2;

            var type = types["abs"].returnTypes;

            var number = type.Match(
                godot => 1,
                gdShrapt => 2);

        }*/

        static string ValidateFirstLetter(string name)
        {
            if (name[0] != '_' && name[0] != '@' && !char.IsLetter(name[0]))
                name = "_" + name;
            else if (!char.IsUpper(name[0]) && !ParseToken(name).IsKeyword())
                name = char.ToUpper(name[0]) + name.Substring(1);

            return name;
        }

        static string RemoveExtraCharacters(string name)
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
