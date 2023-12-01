using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dunet;

namespace GDShrapt.Converter.Tests
{
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
    }
}
