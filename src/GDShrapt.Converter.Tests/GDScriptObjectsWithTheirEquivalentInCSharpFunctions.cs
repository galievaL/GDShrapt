using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GDShrapt.Converter.Tests.MyType;

namespace GDShrapt.Converter.Tests
{
    public static class GDScriptObjectsWithTheirEquivalentInCSharpFunctions
    {
        public static Dictionary<string, (string csharpEquivalent, MyType returnTypes)> GDScriptVariantTypesToLower2 = new Dictionary<string, (string, MyType)>()
        {
            ["nil"] = ("null", SyntaxKind.NullKeyword),
            ["bool"] = ("bool", SyntaxKind.BoolKeyword),
            ["int"] = ("long", SyntaxKind.LongKeyword),
            ["float"] = ("double", SyntaxKind.DoubleKeyword),
            ["string"] = ("string", SyntaxKind.StringKeyword),
            ["vector2"] = ("Vector2", AnotherType.Vector2), 
            ["vector2i"] = ("Vector2I", AnotherType.Vector2I),
            ["rect2"] = ("Rect2", AnotherType.Rect2),
            ["rect2i"] = ("Rect2I", AnotherType.Rect2I),
            ["vector3"] = ("Vector3", AnotherType.Vector3),
            ["vector3i"] = ("Vector3I", AnotherType.Vector3I),
            ["transform2d"] = ("Transform2D", AnotherType.Transform2D),
            ["vector4"] = ("Vector4", AnotherType.Vector4),
            ["vector4i"] = ("Vector4I", AnotherType.Vector4I),
            ["plane"] = ("Plane", AnotherType.Plane),
            ["quaternion"] = ("Quaternion", AnotherType.Quaternion),
            ["aabb"] = ("Aabb", AnotherType.Aabb),
            ["basis"] = ("Basis", AnotherType.Basis),
            ["transform3d"] = ("Transform3D", AnotherType.Transform3D),
            ["projection"] = ("Projection", AnotherType.Projection),
            ["color"] = ("Color", AnotherType.Color),
            ["stringname"] = ("StringName", AnotherType.StringName),
            ["nodepath"] = ("NodePath", AnotherType.NodePath),
            ["rid"] = ("Rid", AnotherType.Rid),
            ["object"] = ("GodotObject", AnotherType.GodotObject),
            ["callable"] = ("Callable", AnotherType.Callable),
            ["signal"] = ("Signal", AnotherType.Signal),
            ["dictionary"] = ("Collections.Dictionary", AnotherType.CollectionsDictionary),
            ["array"] = ("Collections.Array", AnotherType.CollectionsArray),
            ["packedbytearray"] = ("byte[]", ArrayTypes.ByteArray),
            ["packedint32array"] = ("int[]", ArrayTypes.IntArray),
            ["packedint64array"] = ("long[]", ArrayTypes.LongArray),
            ["packedfloat32array"] = ("float[]", ArrayTypes.FloatArray),
            ["packedfloat64array"] = ("double[]", ArrayTypes.DoubleArray),
            ["packedstringarray"] = ("string[]", ArrayTypes.StringArray),
            ["packedvector2array"] = ("Vector2[]", ArrayTypes.Vector2Array),
            ["packedvector3array"] = ("Vector3[]", ArrayTypes.Vector3Array),
            ["packedcolorarray"] = ("Color[]", ArrayTypes.ColorArray)
        };

        public static Dictionary<string, (string csharpEquivalent, SyntaxKind returnTypes)> GDScriptConstsToLower_TheirEquivalentAndReturnTypes = new Dictionary<string, (string, SyntaxKind)>()
        {
            ["pi"] = ("Mathf.Pi", SyntaxKind.DoubleKeyword),
            ["tau"] = ("Mathf.Tau", SyntaxKind.DoubleKeyword),
            ["inf"] = ("Mathf.Inf", SyntaxKind.DoubleKeyword),
            ["nan"] = ("Mathf.NaN", SyntaxKind.DoubleKeyword)
        };

        public static Dictionary<string, (string csharpEquivalent, MyType returnTypes)> GDScriptGlobalScopeFunctions = new Dictionary<string, (string, MyType)>
        {
            ["abs"] = ("Mathf.Abs", SyntaxKind.DoubleKeyword),
            ["absf"] = ("Mathf.Abs", SyntaxKind.DoubleKeyword),
            ["absi"] = ("Mathf.Abs", SyntaxKind.LongKeyword),
            ["acos"] = ("Mathf.Acos", SyntaxKind.DoubleKeyword),
            ["asin"] = ("Mathf.Asin", SyntaxKind.DoubleKeyword),
            ["atan"] = ("Mathf.Atan", SyntaxKind.DoubleKeyword),
            ["atan2"] = ("Mathf.Atan2", SyntaxKind.DoubleKeyword),
            ["bezier_derivative"] = ("Mathf.BezierDerivative", SyntaxKind.DoubleKeyword),
            ["bezier_interpolate"] = ("Mathf.BezierInterpolate", SyntaxKind.DoubleKeyword),
            ["bytes_to_var"] = ("GD.BytesToVar", SyntaxKind.ObjectKeyword),
            ["bytes_to_var_with_objects"] = ("GD.BytesToVarWithObjects", SyntaxKind.ObjectKeyword),
            ["ceil"] = ("Mathf.Ceil", SyntaxKind.DoubleKeyword),
            ["ceilf"] = ("Mathf.Ceil", SyntaxKind.DoubleKeyword),
            ["ceili"] = ("Mathf.CeilToInt", SyntaxKind.LongKeyword),
            ["clamp"] = ("Mathf.Clamp", SyntaxKind.DoubleKeyword),
            ["clampf"] = ("Mathf.Clamp", SyntaxKind.DoubleKeyword),
            ["clampi"] = ("Mathf.Clamp", SyntaxKind.LongKeyword),
            ["cos"] = ("Mathf.Cos", SyntaxKind.DoubleKeyword),
            ["cosh"] = ("Mathf.Cosh", SyntaxKind.DoubleKeyword),
            ["cubic_interpolate"] = ("Mathf.CubicInterpolate", SyntaxKind.DoubleKeyword),
            ["cubic_interpoalte_angle"] = ("Mathf.CubicInterpolateAngle", SyntaxKind.DoubleKeyword),
            ["cubic_interpolate_angle_in_time"] = ("Mathf.CubicInterpolateInTime", SyntaxKind.DoubleKeyword),
            ["cubic_interpolate_in_time"] = ("Mathf.CubicInterpolateAngleInTime", SyntaxKind.DoubleKeyword),
            ["db_to_linear"] = ("Mathf.DbToLinear", SyntaxKind.DoubleKeyword),
            ["deg_to_rad"] = ("Mathf.DegToRad", SyntaxKind.DoubleKeyword),
            ["ease"] = ("Mathf.Ease", SyntaxKind.DoubleKeyword),
            ["error_string"] = ("Error.ToString", SyntaxKind.StringKeyword),
            ["exp"] = ("Mathf.Exp", SyntaxKind.DoubleKeyword),
            ["floor"] = ("Mathf.Floor", SyntaxKind.DoubleKeyword),
            ["floorf"] = ("Mathf.Floor", SyntaxKind.DoubleKeyword),
            ["floori"] = ("Mathf.FloorToInt", SyntaxKind.LongKeyword),
            ["fposmod"] = ("Mathf.PosMod", SyntaxKind.DoubleKeyword),
            ["hash"] = ("GD.Hash", SyntaxKind.LongKeyword),
            ["instance_from_id"] = ("GodotObject.InstanceFromId", SyntaxKind.ObjectKeyword),
            ["inverse_lerp"] = ("Mathf.InverseLerp", SyntaxKind.DoubleKeyword),
            ["is_equal_approx"] = ("Mathf.IsEqualApprox", SyntaxKind.BoolKeyword),
            ["is_finite"] = ("Mathf.IsFinite", SyntaxKind.BoolKeyword),
            ["is_inf"] = ("Mathf.IsInf", SyntaxKind.BoolKeyword),
            ["is_instance_id_valid"] = ("GodotObject.IsInstanceIdValid", SyntaxKind.BoolKeyword),
            ["is_instance_valid"] = ("GodotObject.IsInstanceValid", SyntaxKind.BoolKeyword),
            ["is_nan"] = ("Mathf.IsNaN", SyntaxKind.BoolKeyword),
            ["is_same"] = ("object.ReferenceEquals", SyntaxKind.BoolKeyword),
            ["is_zero_approx"] = ("Mathf.IsZeroApprox", SyntaxKind.BoolKeyword),
            ["lerp"] = ("Mathf.Lerp", SyntaxKind.DoubleKeyword),
            ["lerp_angle"] = ("Mathf.LerpAngle", SyntaxKind.DoubleKeyword),
            ["lerpf"] = ("Mathf.Lerp", SyntaxKind.DoubleKeyword),
            ["linear_to_db"] = ("Mathf.LinearToDb", SyntaxKind.DoubleKeyword),
            ["log"] = ("Mathf.Log", SyntaxKind.DoubleKeyword),
            ["max"] = ("Mathf.Max", SyntaxKind.DoubleKeyword),
            ["maxf"] = ("Mathf.Max", SyntaxKind.DoubleKeyword),
            ["maxi"] = ("Mathf.Max", SyntaxKind.LongKeyword),
            ["min"] = ("Mathf.Min", SyntaxKind.DoubleKeyword),
            ["minf"] = ("Mathf.Min", SyntaxKind.DoubleKeyword),
            ["mini"] = ("Mathf.Min", SyntaxKind.LongKeyword),
            ["move_toward"] = ("Mathf.MoveToward", SyntaxKind.DoubleKeyword),
            ["nearest_po2"] = ("Mathf.NearestPo2", SyntaxKind.LongKeyword),
            ["pingpong"] = ("Mathf.PingPong", SyntaxKind.DoubleKeyword),
            ["posmod"] = ("Mathf.PosMod", SyntaxKind.DoubleKeyword),
            ["pow"] = ("Mathf.Pow", SyntaxKind.DoubleKeyword),
            ["print"] = ("GD.Print", SyntaxKind.VoidKeyword),
            ["print_rich"] = ("GD.PrintRich", SyntaxKind.VoidKeyword),
            ["print_verbose"] = ("GD.Print", SyntaxKind.VoidKeyword),
            ["printerr"] = ("GD.PrintErr", SyntaxKind.VoidKeyword),
            ["printraw"] = ("GD.PrintRaw", SyntaxKind.VoidKeyword),
            ["prints"] = ("GD.PrintS", SyntaxKind.VoidKeyword),
            ["printt"] = ("GD.PrintT", SyntaxKind.VoidKeyword),
            ["push_error"] = ("GD.PushError", SyntaxKind.VoidKeyword),
            ["push_warning"] = ("GD.PushWarning", SyntaxKind.VoidKeyword),
            ["rad_to_deg"] = ("Mathf.RadToDeg", SyntaxKind.DoubleKeyword),
            ["rand_from_seed"] = ("GD.RandFromSeed", SyntaxKind.DoubleKeyword),
            ["randf"] = ("GD.Randf", SyntaxKind.DoubleKeyword),
            ["randf_range"] = ("GD.RandRange", SyntaxKind.DoubleKeyword),
            ["randfn"] = ("GD.Randfn", SyntaxKind.DoubleKeyword),
            ["randi"] = ("GD.Randi", SyntaxKind.LongKeyword),
            ["randi_range"] = ("GD.RandRange", SyntaxKind.LongKeyword),
            ["randomize"] = ("GD.Randomize", SyntaxKind.VoidKeyword),
            ["remap"] = ("Mathf.Remap", SyntaxKind.DoubleKeyword),
            ["round"] = ("Mathf.Round", SyntaxKind.DoubleKeyword),
            ["roundf"] = ("Mathf.Round", SyntaxKind.DoubleKeyword),
            ["roundi"] = ("Mathf.RoundToInt", SyntaxKind.LongKeyword),
            ["seed"] = ("GD.Seed", SyntaxKind.VoidKeyword),
            ["sign"] = ("Mathf.Sign", SyntaxKind.DoubleKeyword),
            ["signf"] = ("Mathf.Sign", SyntaxKind.DoubleKeyword),
            ["signi"] = ("Mathf.Sign", SyntaxKind.LongKeyword),
            ["sin"] = ("Mathf.Sin", SyntaxKind.DoubleKeyword),
            ["sinh"] = ("Mathf.Sinh", SyntaxKind.DoubleKeyword),
            ["smoothstep"] = ("Mathf.SmoothStep", SyntaxKind.DoubleKeyword),
            ["snapped"] = ("Mathf.Snapped", SyntaxKind.DoubleKeyword),
            ["snappedf"] = ("Mathf.Snapped", SyntaxKind.DoubleKeyword),
            ["snappedi"] = ("Mathf.Snapped", SyntaxKind.LongKeyword),
            ["sqrt"] = ("Mathf.Sqrt", SyntaxKind.DoubleKeyword),
            ["step_decimals"] = ("Mathf.StepDecimals", SyntaxKind.LongKeyword),
            ["str_to_var"] = ("GD.StrToVar", SyntaxKind.ObjectKeyword),
            ["tan"] = ("Mathf.Tan", SyntaxKind.DoubleKeyword),
            ["tanh"] = ("Mathf.Tanh", SyntaxKind.DoubleKeyword),
            ["typeof"] = ("Variant.VariantType", SyntaxKind.TypeKeyword),
            ["var_to_str"] = ("GD.VarToStr", SyntaxKind.StringKeyword),
            ["wrap"] = ("Mathf.Wrap", SyntaxKind.DoubleKeyword),
            ["wrapf"] = ("Mathf.Wrap", SyntaxKind.DoubleKeyword),
            ["wrapi"] = ("Mathf.Wrap", SyntaxKind.LongKeyword),
            ["var_to_bytes"] = ("GD.VarToBytes", ArrayTypes.ByteArray),
            ["var_to_bytes_with_objects"] = ("GD.VarToBytesWithObjects", ArrayTypes.ByteArray),
            ["weakref"] = ("GodotObject.WeakRef", AnotherType.WeakRef),
        };

        public static Dictionary<string, (string csharpEquivalent, MyType returnTypes)> GDScriptGlobalScopeFunctions_NAequivalent = new Dictionary<string, (string, MyType)>
        {
            ["rid_allocate_id"] = ("rid_allocate_id", AnotherType.Rid),
            ["rid_from_int64"] = ("rid_from_int64", AnotherType.Rid),
            ["fmod"] = ("fmod", SyntaxKind.DoubleKeyword),
            ["str"] = ("str", SyntaxKind.StringKeyword)
        };

        public static Dictionary<string, (string csharpEquivalent, MyType returnTypes)> GDScriptUtilityFunctions = new Dictionary<string, (string, MyType)>
        {
            ["assert"] = ("System.Diagnostics.Debug.Assert", SyntaxKind.VoidKeyword),
            ["convert"] = ("GD.Convert", SyntaxKind.ObjectKeyword),
            ["get_stack"] = ("System.Environment.StackTrace", SyntaxKind.StringKeyword),
            ["print_stack"] = ("GD.Print(System.Environment.StackTrace)", SyntaxKind.VoidKeyword),
            ["type_exists"] = ("ClassDB.ClassExists(type)", SyntaxKind.BoolKeyword),
            ["load"] = ("GD.Load", AnotherType.Resource),
            ["preload"] = ("GD.Load", AnotherType.Resource),
            ["range"] = ("GD.Range", AnotherType.IEnumerable),
        };

        public static Dictionary<string, (string csharpEquivalent, MyType returnTypes)> GDScriptUtilityFunctions_NAequivalen = new Dictionary<string, (string, MyType)>
        {
            ["dict_to_inst"] = ("dict_to_inst", SyntaxKind.ObjectKeyword),
            ["print_debug"] = ("print_debug", SyntaxKind.VoidKeyword),
            ["len"] = ("len", SyntaxKind.LongKeyword),
            ["inst_to_dict"] = ("inst_to_dict", AnotherType.Dictionary),
            ["char"] = ("char", SyntaxKind.CharKeyword),
            ["float"] = ("float", SyntaxKind.DoubleKeyword)
        };
    }
}
