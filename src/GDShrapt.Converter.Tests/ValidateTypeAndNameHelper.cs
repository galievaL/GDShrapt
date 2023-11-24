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
            if (GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower.ContainsKey(type))
                return GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower[type];

            return typeName;
        }

        public static bool IsItGodotType(string typeName)
        {
            var type = typeName.ToLower();
            return GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptVariantTypesToLower.ContainsKey(type);
        }

        public static bool IsItGodotFunctions(ref string functionsName, out string type)
        {
            type = null;
            functionsName = functionsName.ToLower();

            var b1 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstantsToLower_TheirEquivalentAndReturnTypes.ContainsKey(functionsName);
            var b2 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes.ContainsKey(functionsName);
            var b3 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes_NAequivalent.ContainsKey(functionsName);
            var b4 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes.ContainsKey(functionsName);
            var b5 = GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes_NAequivalen.ContainsKey(functionsName);

            if (!(b1 || b2 || b3 || b4 || b5))
                return false;

            var group = b1 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptConstantsToLower_TheirEquivalentAndReturnTypes[functionsName] :
                            b2 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes[functionsName] :
                            b3 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes_NAequivalent[functionsName] :
                            b4 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes[functionsName] :
                            b5 ? GDScriptObjectsWithTheirEquivalentInCSharpFunctions.GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes_NAequivalen[functionsName] :
                            throw new NotImplementedException();

            type = group.type;
            functionsName = group.csharpFunction;

            return true;
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
