using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDShrapt.Converter.Tests
{
    public static class GeneralTypeHelper
    {
        public static GeneralType GetGeneralType(string text)
        {
            if (CustomSyntaxKindConverter.ContainsKey(text))
                return new GeneralType(CustomSyntaxKindConverter[text]);

            return new GeneralType(text);
        }

        public static Dictionary<string, CustomSyntaxKind> CustomSyntaxKindConverter = new Dictionary<string, CustomSyntaxKind>()
        {
            [""] = CustomSyntaxKind.None,
            ["Resource"] = CustomSyntaxKind.Resource,
            ["Color"] = CustomSyntaxKind.Color,
            ["IEnumerable"] = CustomSyntaxKind.IEnumerable,
            ["Dictionary"] = CustomSyntaxKind.Dictionary,
            ["Variant"] = CustomSyntaxKind.Variant,
            ["Vector2"] = CustomSyntaxKind.Vector2,
            ["Vector2I"] = CustomSyntaxKind.Vector2I,
            ["Vector3"] = CustomSyntaxKind.Vector3,
            ["Vector3I"] = CustomSyntaxKind.Vector3I,
            ["Vector4"] = CustomSyntaxKind.Vector4,
            ["Vector4I"] = CustomSyntaxKind.Vector4I,
            ["Rect2"] = CustomSyntaxKind.Rect2,
            ["Rect2I"] = CustomSyntaxKind.Rect2I,
            ["Transform2D"] = CustomSyntaxKind.Transform2D,
            ["Transform3D"] = CustomSyntaxKind.Transform3D,
            ["Rid"] = CustomSyntaxKind.Rid,
            ["WeakRef"] = CustomSyntaxKind.WeakRef,
            ["Plane"] = CustomSyntaxKind.Plane,
            ["Quaternion"] = CustomSyntaxKind.Quaternion,
            ["Aabb"] = CustomSyntaxKind.Aabb,
            ["Basis"] = CustomSyntaxKind.Basis,
            ["Projection"] = CustomSyntaxKind.Projection,
            ["StringName"] = CustomSyntaxKind.StringName,
            ["NodePath"] = CustomSyntaxKind.NodePath,
            ["GodotObject"] = CustomSyntaxKind.GodotObject,
            ["Callable"] = CustomSyntaxKind.Callable,
            ["Signal"] = CustomSyntaxKind.Signal,
            ["CollectionsDictionary"] = CustomSyntaxKind.CollectionsDictionary,
            ["CollectionsArray"] = CustomSyntaxKind.CollectionsArray,
            ["SpecialType"] = CustomSyntaxKind.SpecialType,
            ["ByteArray"] = CustomSyntaxKind.ByteArray,
            ["IntArray"] = CustomSyntaxKind.IntArray,
            ["LongArray"] = CustomSyntaxKind.LongArray,
            ["FloatArray"] = CustomSyntaxKind.FloatArray,
            ["DoubleArray"] = CustomSyntaxKind.DoubleArray,
            ["StringArray"] = CustomSyntaxKind.StringArray,
            ["ColorArray"] = CustomSyntaxKind.ColorArray,
            ["Vector2Array"] = CustomSyntaxKind.Vector2Array,
            ["Vector3Array"] = CustomSyntaxKind.Vector3Array,

            ["BoolKeyword"] = CustomSyntaxKind.BoolKeyword,
            ["IntKeyword"] = CustomSyntaxKind.IntKeyword,
            ["LongKeyword"] = CustomSyntaxKind.LongKeyword,
            ["FloatKeyword"] = CustomSyntaxKind.FloatKeyword,
            ["StringKeyword"] = CustomSyntaxKind.StringKeyword,
            ["CharKeyword"] = CustomSyntaxKind.CharKeyword,
            ["VoidKeyword"] = CustomSyntaxKind.VoidKeyword,
            ["ObjectKeyword"] = CustomSyntaxKind.ObjectKeyword,
            ["NullKeyword"] = CustomSyntaxKind.NullKeyword,
            ["PublicKeyword"] = CustomSyntaxKind.PublicKeyword,
            ["StaticKeyword"] = CustomSyntaxKind.StaticKeyword,
            ["ReadOnlyKeyword"] = CustomSyntaxKind.ReadOnlyKeyword,
            ["ConstKeyword"] = CustomSyntaxKind.ConstKeyword,
            ["TypeKeyword"] = CustomSyntaxKind.TypeKeyword,

            ["Bool"] = CustomSyntaxKind.BoolKeyword,
            ["Int"] = CustomSyntaxKind.IntKeyword,
            ["Long"] = CustomSyntaxKind.LongKeyword,
            ["Float"] = CustomSyntaxKind.FloatKeyword,
            ["String"] = CustomSyntaxKind.StringKeyword,
            ["Char"] = CustomSyntaxKind.CharKeyword,
            ["Void"] = CustomSyntaxKind.VoidKeyword,
            ["Object"] = CustomSyntaxKind.ObjectKeyword,
            ["Null"] = CustomSyntaxKind.NullKeyword,
            ["Public"] = CustomSyntaxKind.PublicKeyword,
            ["Static"] = CustomSyntaxKind.StaticKeyword,
            ["ReadOnly"] = CustomSyntaxKind.ReadOnlyKeyword,
            ["Const"] = CustomSyntaxKind.ConstKeyword,
            ["Type"] = CustomSyntaxKind.TypeKeyword,

            ["CollectionInitializerExpression"] = CustomSyntaxKind.CollectionInitializerExpression,
            ["ArrayInitializerExpression"] = CustomSyntaxKind.ArrayInitializerExpression,
            ["ComplexElementInitializerExpression"] = CustomSyntaxKind.ComplexElementInitializerExpression,
            ["AddExpression"] = CustomSyntaxKind.AddExpression,
            ["SubtractExpression"] = CustomSyntaxKind.SubtractExpression,
            ["MultiplyExpression"] = CustomSyntaxKind.MultiplyExpression,
            ["DivideExpression"] = CustomSyntaxKind.DivideExpression,
            ["ModuloExpression"] = CustomSyntaxKind.ModuloExpression,
            ["LeftShiftExpression"] = CustomSyntaxKind.LeftShiftExpression,
            ["RightShiftExpression"] = CustomSyntaxKind.RightShiftExpression,
            ["LogicalOrExpression"] = CustomSyntaxKind.LogicalOrExpression,
            ["LogicalAndExpression"] = CustomSyntaxKind.LogicalAndExpression,
            ["BitwiseOrExpression"] = CustomSyntaxKind.BitwiseOrExpression,
            ["BitwiseAndExpression"] = CustomSyntaxKind.BitwiseAndExpression,
            ["ExclusiveOrExpression"] = CustomSyntaxKind.ExclusiveOrExpression,
            ["EqualsExpression"] = CustomSyntaxKind.EqualsExpression,
            ["NotEqualsExpression"] = CustomSyntaxKind.NotEqualsExpression,
            ["LessThanExpression"] = CustomSyntaxKind.LessThanExpression,
            ["LessThanOrEqualExpression"] = CustomSyntaxKind.LessThanOrEqualExpression,
            ["GreaterThanExpression"] = CustomSyntaxKind.GreaterThanExpression,
            ["GreaterThanOrEqualExpression"] = CustomSyntaxKind.GreaterThanOrEqualExpression,
            ["IsExpression"] = CustomSyntaxKind.IsExpression,
            ["AsExpression"] = CustomSyntaxKind.AsExpression,
            ["SimpleMemberAccessExpression"] = CustomSyntaxKind.SimpleMemberAccessExpression,
            ["SimpleAssignmentExpression"] = CustomSyntaxKind.SimpleAssignmentExpression,
            ["AddAssignmentExpression"] = CustomSyntaxKind.AddAssignmentExpression,
            ["SubtractAssignmentExpression"] = CustomSyntaxKind.SubtractAssignmentExpression,
            ["DivideAssignmentExpression"] = CustomSyntaxKind.DivideAssignmentExpression,
            ["ModuloAssignmentExpression"] = CustomSyntaxKind.ModuloAssignmentExpression,
            ["NumericLiteralExpression"] = CustomSyntaxKind.NumericLiteralExpression,
            ["StringLiteralExpression"] = CustomSyntaxKind.StringLiteralExpression,
            ["TrueLiteralExpression"] = CustomSyntaxKind.TrueLiteralExpression,
            ["FalseLiteralExpression"] = CustomSyntaxKind.FalseLiteralExpression,
            ["NullLiteralExpression"] = CustomSyntaxKind.NullLiteralExpression,
            ["EqualsValueClause"] = CustomSyntaxKind.EqualsValueClause
        };
    }
}
