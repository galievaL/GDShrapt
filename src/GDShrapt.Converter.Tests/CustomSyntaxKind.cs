using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDShrapt.Converter.Tests
{
    public enum CustomSyntaxKind
    {
        None = 0,
        UserType,

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
        SpecialType,

        ByteArray,
        IntArray,
        LongArray,
        FloatArray,
        DoubleArray,
        StringArray,
        ColorArray,
        Vector2Array,
        Vector3Array,

        // Keywords
        /// <summary>Represents <see langword="bool"/>.</summary>
        BoolKeyword = 8304,
        /// <summary>Represents <see langword="int"/>.</summary>
        IntKeyword = 8309,
        /// <summary>Represents <see langword="long"/>.</summary>
        LongKeyword = 8311,
        /// <summary>Represents <see langword="double"/>.</summary>
        DoubleKeyword = 8313,
        /// <summary>Represents <see langword="float"/>.</summary>
        FloatKeyword = 8314,
        /// <summary>Represents <see langword="string"/>.</summary>
        StringKeyword = 8316,
        /// <summary>Represents <see langword="char"/>.</summary>
        CharKeyword = 8317,
        /// <summary>Represents <see langword="void"/>.</summary>
        VoidKeyword = 8318,
        /// <summary>Represents <see langword="object"/>.</summary>
        ObjectKeyword = 8319,
        /// <summary>Represents <see langword="null"/>.</summary>
        NullKeyword = 8322,
        /// <summary>Represents <see langword="public"/>.</summary>
        PublicKeyword = 8343,
        /// <summary>Represents <see langword="static"/>.</summary>
        StaticKeyword = 8347,
        /// <summary>Represents <see langword="readonly"/>.</summary>
        ReadOnlyKeyword = 8348,
        /// <summary>Represents <see langword="const"/>.</summary>
        ConstKeyword = 8350,

        // contextual keywords
        /// <summary>Represents <see langword="type"/>.</summary>
        TypeKeyword = 8411,

        // expressions
        CollectionInitializerExpression = 8645,
        ArrayInitializerExpression = 8646,
        ComplexElementInitializerExpression = 8648,

        // binary expressions
        AddExpression = 8668,
        SubtractExpression = 8669,
        MultiplyExpression = 8670,
        DivideExpression = 8671,
        ModuloExpression = 8672,
        LeftShiftExpression = 8673,
        RightShiftExpression = 8674,
        LogicalOrExpression = 8675,
        LogicalAndExpression = 8676,
        BitwiseOrExpression = 8677,
        BitwiseAndExpression = 8678,
        ExclusiveOrExpression = 8679,
        EqualsExpression = 8680,
        NotEqualsExpression = 8681,
        LessThanExpression = 8682,
        LessThanOrEqualExpression = 8683,
        GreaterThanExpression = 8684,
        GreaterThanOrEqualExpression = 8685,
        IsExpression = 8686,
        AsExpression = 8687,
        SimpleMemberAccessExpression = 8689,  // dot access:   a.b

        // binary assignment expressions
        SimpleAssignmentExpression = 8714,
        AddAssignmentExpression = 8715,
        SubtractAssignmentExpression = 8716,
        DivideAssignmentExpression = 8718,
        ModuloAssignmentExpression = 8719,

        // primary expression
        NumericLiteralExpression = 8749,
        StringLiteralExpression = 8750,
        TrueLiteralExpression = 8752,
        FalseLiteralExpression = 8753,
        NullLiteralExpression = 8754,

        // statements
        EqualsValueClause = 8796,
    }
}
