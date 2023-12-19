using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dunet;

namespace GDShrapt.Converter.Tests
{
    public class GeneralType
    {
        public CustomSyntaxKind CustomType { get; set; }

        string _userType;
        public string UserType 
        {
            get { return _userType; } 
            set 
            {
                CustomType = CustomSyntaxKind.UserType;
                _userType = value; 
            }
        }
        public GeneralType()
        {
            CustomType = CustomSyntaxKind.None;
        }

        public GeneralType(string userType)
        {
            UserType = userType;
        }

        public GeneralType(CustomSyntaxKind customType)
        {
            CustomType = customType;
        }

        public override string ToString()
        {
            if (CustomType == CustomSyntaxKind.UserType)
                return UserType;
            else if (CustomType == CustomSyntaxKind.None)
                return string.Empty;
            else
                return CustomSyntaxToString(CustomType);
        }

        string CustomSyntaxToString(CustomSyntaxKind kind)
        {
            switch (kind)
            {
                case CustomSyntaxKind.BoolKeyword:
                    return "bool";
                case CustomSyntaxKind.IntKeyword:
                    return "int";
                case CustomSyntaxKind.LongKeyword:
                    return "long";
                case CustomSyntaxKind.DoubleKeyword:
                    return "double";
                case CustomSyntaxKind.FloatKeyword:
                    return "float";
                case CustomSyntaxKind.StringKeyword:
                    return "string";
                case CustomSyntaxKind.CharKeyword:
                    return "char";
                case CustomSyntaxKind.VoidKeyword:
                    return "void";
                case CustomSyntaxKind.ObjectKeyword:
                    return "object";
                case CustomSyntaxKind.NullKeyword:
                    return "null";
                case CustomSyntaxKind.PublicKeyword:
                    return "public";
                case CustomSyntaxKind.StaticKeyword:
                    return "static";
                case CustomSyntaxKind.ReadOnlyKeyword:
                    return "readonly";
                case CustomSyntaxKind.ConstKeyword:
                    return "const";
                case CustomSyntaxKind.TypeKeyword:
                    return "type";
                default:
                    return kind.ToString();
            }
        }

        Dictionary<CustomSyntaxKind, SyntaxKind> ToSyntaxKindConverter = new Dictionary<CustomSyntaxKind, SyntaxKind>()
        {
            [CustomSyntaxKind.BoolKeyword] = SyntaxKind.BoolKeyword,
            [CustomSyntaxKind.IntKeyword] = SyntaxKind.IntKeyword,
            [CustomSyntaxKind.LongKeyword] = SyntaxKind.LongKeyword,
            [CustomSyntaxKind.DoubleKeyword] = SyntaxKind.DoubleKeyword,
            [CustomSyntaxKind.FloatKeyword] = SyntaxKind.FloatKeyword,
            [CustomSyntaxKind.StringKeyword] = SyntaxKind.StringKeyword,
            [CustomSyntaxKind.CharKeyword] = SyntaxKind.CharKeyword,
            [CustomSyntaxKind.VoidKeyword] = SyntaxKind.VoidKeyword,
            [CustomSyntaxKind.ObjectKeyword] = SyntaxKind.ObjectKeyword,
            [CustomSyntaxKind.NullKeyword] = SyntaxKind.NullKeyword,
            [CustomSyntaxKind.PublicKeyword] = SyntaxKind.PublicKeyword,
            [CustomSyntaxKind.StaticKeyword] = SyntaxKind.StaticKeyword,
            [CustomSyntaxKind.ReadOnlyKeyword] = SyntaxKind.ReadOnlyKeyword,
            [CustomSyntaxKind.ConstKeyword] = SyntaxKind.ConstKeyword,
            [CustomSyntaxKind.TypeKeyword] = SyntaxKind.TypeKeyword
        };
    }

    /*
    [Union]
    public partial record MyType
    {
        public partial record Syntax(SyntaxKind Kind);
        public partial record Another(AnotherType Type);
        public partial record Arrays(ArrayTypes Array);
    }

    public enum ArrayTypes
    {
        ByteArray,
        IntArray,
        LongArray,
        FloatArray,
        DoubleArray,
        StringArray,
        ColorArray,
        Vector2Array,
        Vector3Array
    }

    public enum AnotherType
    {
        Resource,
        Color,
        IEnumerable,
        Dictionary,
        Variant,
        Vector2I,
        Vector2,
        Vector3,
        Vector3I,
        Vector4,
        Vector4I,
        Rect2,
        Rect2I,
        Transform2D,
        Transform3D,
        Rid,
        WeakRef,
        Plane,
        Quaternion,
        Aabb,
        Basis,
        Projection,
        StringName,
        NodePath,
        GodotObject,
        Callable,
        Signal,
        CollectionsDictionary,
        CollectionsArray,
        SpecialType
    }*/
}
