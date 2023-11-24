using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDShrapt.Converter.Tests
{
    public static class GDScriptObjectsWithTheirEquivalentInCSharpFunctions
    {
        public static Dictionary<string, string> GDScriptVariantTypesToLower = new Dictionary<string, string>()
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

        //public static Dictionary<string, string> GDScriptConstantsToUpper = new Dictionary<string, string>()
        //{
        //    ["PI"] = "Mathf.Pi",
        //    ["TAU"] = "Mathf.Tau",
        //    ["INF"] = "Mathf.Inf",
        //    ["NAN"] = "Mathf.NaN"
        //};

        //public static Dictionary<string, string> GDScriptGlobalScopeFunctions = new Dictionary<string, string>()
        //{
        //    ["abs"] = "Mathf.Abs",
        //    ["absf"] = "Mathf.Abs",
        //    ["absi"] = "Mathf.Abs",
        //    ["acos"] = "Mathf.Acos",
        //    ["asin"] = "Mathf.Asin",
        //    ["atan"] = "Mathf.Atan",
        //    ["atan2"] = "Mathf.Atan2",
        //    ["bezier_derivative"] = "Mathf.BezierDerivative",
        //    ["bezier_interpolate"] = "Mathf.BezierInterpolate",
        //    ["bytes_to_var"] = "GD.BytesToVar",
        //    ["bytes_to_var_with_objects"] = "GD.BytesToVarWithObjects",
        //    ["ceil"] = "Mathf.Ceil",
        //    ["ceilf"] = "Mathf.Ceil",
        //    ["ceili"] = "Mathf.CeilToInt",
        //    ["clamp"] = "Mathf.Clamp",
        //    ["clampf"] = "Mathf.Clamp",
        //    ["clampi"] = "Mathf.Clamp",
        //    ["cos"] = "Mathf.Cos",
        //    ["cosh"] = "Mathf.Cosh",
        //    ["cubic_interpolate"] = "Mathf.CubicInterpolate",
        //    ["cubic_interpoalte_angle"] = "Mathf.CubicInterpolateAngle",
        //    ["cubic_interpolate_angle_in_time"] = "Mathf.CubicInterpolateInTime",
        //    ["cubic_interpolate_in_time"] = "Mathf.CubicInterpolateAngleInTime",
        //    ["db_to_linear"] = "Mathf.DbToLinear",
        //    ["deg_to_rad"] = "Mathf.DegToRad",
        //    ["ease"] = "Mathf.Ease",
        //    ["error_string"] = "Error.ToString",
        //    ["exp"] = "Mathf.Exp",
        //    ["floor"] = "Mathf.Floor",
        //    ["floorf"] = "Mathf.Floor",
        //    ["floori"] = "Mathf.FloorToInt",
        //    ["fposmod"] = "Mathf.PosMod",
        //    ["hash"] = "GD.Hash",
        //    ["instance_from_id"] = "GodotObject.InstanceFromId",
        //    ["inverse_lerp"] = "Mathf.InverseLerp",
        //    ["is_equal_approx"] = "Mathf.IsEqualApprox",
        //    ["is_finite"] = "Mathf.IsFinite",
        //    ["is_inf"] = "Mathf.IsInf",
        //    ["is_instance_id_valid"] = "GodotObject.IsInstanceIdValid",
        //    ["is_instance_valid"] = "GodotObject.IsInstanceValid",
        //    ["is_nan"] = "Mathf.IsNaN",
        //    ["is_same"] = "object.ReferenceEquals",
        //    ["is_zero_approx"] = "Mathf.IsZeroApprox",
        //    ["lerp"] = "Mathf.Lerp",
        //    ["lerp_angle"] = "Mathf.LerpAngle",
        //    ["lerpf"] = "Mathf.Lerp",
        //    ["linear_to_db"] = "Mathf.LinearToDb",
        //    ["log"] = "Mathf.Log",
        //    ["max"] = "Mathf.Max",
        //    ["maxf"] = "Mathf.Max",
        //    ["maxi"] = "Mathf.Max",
        //    ["min"] = "Mathf.Min",
        //    ["minf"] = "Mathf.Min",
        //    ["mini"] = "Mathf.Min",
        //    ["move_toward"] = "Mathf.MoveToward",
        //    ["nearest_po2"] = "Mathf.NearestPo2",
        //    ["pingpong"] = "Mathf.PingPong",
        //    ["posmod"] = "Mathf.PosMod",
        //    ["pow"] = "Mathf.Pow",
        //    ["print"] = "GD.Print",
        //    ["print_rich"] = "GD.PrintRich",
        //    ["print_verbose"] = "GD.Print",
        //    ["printerr"] = "GD.PrintErr",
        //    ["printraw"] = "GD.PrintRaw",
        //    ["prints"] = "GD.PrintS",
        //    ["printt"] = "GD.PrintT",
        //    ["push_error"] = "GD.PushError",
        //    ["push_warning"] = "GD.PushWarning",
        //    ["rad_to_deg"] = "Mathf.RadToDeg",
        //    ["rand_from_seed"] = "GD.RandFromSeed",
        //    ["randf"] = "GD.Randf",
        //    ["randf_range"] = "GD.RandRange",
        //    ["randfn"] = "GD.Randfn",
        //    ["randi"] = "GD.Randi",
        //    ["randi_range"] = "GD.RandRange",
        //    ["randomize"] = "GD.Randomize",
        //    ["remap"] = "Mathf.Remap",
        //    ["round"] = "Mathf.Round",
        //    ["roundf"] = "Mathf.Round",
        //    ["roundi"] = "Mathf.RoundToInt",
        //    ["seed"] = "GD.Seed",
        //    ["sign"] = "Mathf.Sign",
        //    ["signf"] = "Mathf.Sign",
        //    ["signi"] = "Mathf.Sign",
        //    ["sin"] = "Mathf.Sin",
        //    ["sinh"] = "Mathf.Sinh",
        //    ["smoothstep"] = "Mathf.SmoothStep",
        //    ["snapped"] = "Mathf.Snapped",
        //    ["snappedf"] = "Mathf.Snapped",
        //    ["snappedi"] = "Mathf.Snapped",
        //    ["sqrt"] = "Mathf.Sqrt",
        //    ["step_decimals"] = "Mathf.StepDecimals",
        //    ["str_to_var"] = "GD.StrToVar",
        //    ["tan"] = "Mathf.Tan",
        //    ["tanh"] = "Mathf.Tanh",
        //    ["typeof"] = "Variant.VariantType",
        //    ["var_to_bytes"] = "GD.VarToBytes",
        //    ["var_to_bytes_with_objects"] = "GD.VarToBytesWithObjects",
        //    ["var_to_str"] = "GD.VarToStr",
        //    ["weakref"] = "GodotObject.WeakRef",
        //    ["wrap"] = "Mathf.Wrap",
        //    ["wrapf"] = "Mathf.Wrap",
        //    ["wrapi"] = "Mathf.Wrap"
        //};

        //public static Dictionary<string, string> GDScriptGlobalScopeFunctions_NAequivalent = new Dictionary<string, string>()
        //{
        //    ["rid_allocate_id"] = "rid_allocate_id",
        //    ["rid_from_int64"] = "rid_from_int64",
        //    //["fmod"] = "operator %"
        //    //["str"] = (то, что в скобках).ToString()
        //};

        //public static Dictionary<string, string> GDScriptUtilityFunctions = new Dictionary<string, string>()
        //{
        //    ["assert"] = "System.Diagnostics.Debug.Assert",
        //    ["convert"] = "GD.Convert",
        //    ["get_stack"] = "System.Environment.StackTrace",
        //    ["load"] = "GD.Load",
        //    ["preload"] = "GD.Load",
        //    ["print_stack"] = "GD.Print(System.Environment.StackTrace)",
        //    ["range"] = "GD.Range",
        //    ["type_exists"] = "ClassDB.ClassExists(type)"
        //};

        //public static Dictionary<string, string> GDScriptUtilityFunctions_NAequivalen = new Dictionary<string, string>()
        //{
        //    ["dict_to_inst"] = "dict_to_inst",
        //    ["inst_to_dict"] = "inst_to_dict",
        //    ["len"] = "len",
        //    ["print_debug"] = "print_debug",
        //    //["char"] = "Используйте явное преобразование: (char)65",
        //};

        //----------------------------------------------------------------------------

        public static Dictionary<string, (string csharpFunction, string type)> GDScriptConstantsToLower_TheirEquivalentAndReturnTypes = new Dictionary<string, (string, string)>()
        {
            ["pi"] = ("Mathf.Pi", "double"),
            ["tau"] = ("Mathf.Tau", "double"),
            ["inf"] = ("Mathf.Inf", "double"),
            ["nan"] = ("Mathf.NaN", "double")
        };

        public static Dictionary<string, (string csharpFunction, string type)> GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes = new Dictionary<string, (string, string)>
        {
            ["abs"] = ("Mathf.Abs", "double"),
            ["absf"] = ("Mathf.Abs", "float"),
            ["absi"] = ("Mathf.Abs", "int"),
            ["acos"] = ("Mathf.Acos", "float"),
            ["asin"] = ("Mathf.Asin", "float"),
            ["atan"] = ("Mathf.Atan", "float"),
            ["atan2"] = ("Mathf.Atan2", "float"),
            ["bezier_derivative"] = ("Mathf.BezierDerivative", "float"),
            ["bezier_interpolate"] = ("Mathf.BezierInterpolate", "float"),
            ["bytes_to_var"] = ("GD.BytesToVar", "object"),
            ["bytes_to_var_with_objects"] = ("GD.BytesToVarWithObjects", "object"),
            ["ceil"] = ("Mathf.Ceil", "double"),
            ["ceilf"] = ("Mathf.Ceil", "float"),
            ["ceili"] = ("Mathf.CeilToInt", "int"),
            ["clamp"] = ("Mathf.Clamp", "double"),
            ["clampf"] = ("Mathf.Clamp", "float"),
            ["clampi"] = ("Mathf.Clamp", "int"),
            ["cos"] = ("Mathf.Cos", "float"),
            ["cosh"] = ("Mathf.Cosh", "float"),
            ["cubic_interpolate"] = ("Mathf.CubicInterpolate", "float"),
            ["cubic_interpoalte_angle"] = ("Mathf.CubicInterpolateAngle", "float"),
            ["cubic_interpolate_angle_in_time"] = ("Mathf.CubicInterpolateInTime", "float"),
            ["cubic_interpolate_in_time"] = ("Mathf.CubicInterpolateAngleInTime", "float"),
            ["db_to_linear"] = ("Mathf.DbToLinear", "float"),
            ["deg_to_rad"] = ("Mathf.DegToRad", "float"),
            ["ease"] = ("Mathf.Ease", "float"),
            ["error_string"] = ("Error.ToString", "string"),
            ["exp"] = ("Mathf.Exp", "float"),
            ["floor"] = ("Mathf.Floor", "double"),
            ["floorf"] = ("Mathf.Floor", "float"),
            ["floori"] = ("Mathf.FloorToInt", "int"),
            ["fposmod"] = ("Mathf.PosMod", "float"),
            ["hash"] = ("GD.Hash", "int"),
            ["instance_from_id"] = ("GodotObject.InstanceFromId", "object"),
            ["inverse_lerp"] = ("Mathf.InverseLerp", "float"),
            ["is_equal_approx"] = ("Mathf.IsEqualApprox", "bool"),
            ["is_finite"] = ("Mathf.IsFinite", "bool"),
            ["is_inf"] = ("Mathf.IsInf", "bool"),
            ["is_instance_id_valid"] = ("GodotObject.IsInstanceIdValid", "bool"),
            ["is_instance_valid"] = ("GodotObject.IsInstanceValid", "bool"),
            ["is_nan"] = ("Mathf.IsNaN", "bool"),
            ["is_same"] = ("object.ReferenceEquals", "bool"),
            ["is_zero_approx"] = ("Mathf.IsZeroApprox", "bool"),
            ["lerp"] = ("Mathf.Lerp", "float"),
            ["lerp_angle"] = ("Mathf.LerpAngle", "float"),
            ["lerpf"] = ("Mathf.Lerp", "float"),
            ["linear_to_db"] = ("Mathf.LinearToDb", "float"),
            ["log"] = ("Mathf.Log", "float"),
            ["max"] = ("Mathf.Max", "double"),
            ["maxf"] = ("Mathf.Max", "float"),
            ["maxi"] = ("Mathf.Max", "int"),
            ["min"] = ("Mathf.Min", "float"),
            ["minf"] = ("Mathf.Min", "float"),
            ["mini"] = ("Mathf.Min", "int"),
            ["move_toward"] = ("Mathf.MoveToward", "float"),
            ["nearest_po2"] = ("Mathf.NearestPo2", "int"),
            ["pingpong"] = ("Mathf.PingPong", "float"),
            ["posmod"] = ("Mathf.PosMod", "float"),
            ["pow"] = ("Mathf.Pow", "float"),
            ["print"] = ("GD.Print", "void"),
            ["print_rich"] = ("GD.PrintRich", "void"),
            ["print_verbose"] = ("GD.Print", "void"),
            ["printerr"] = ("GD.PrintErr", "void"),
            ["printraw"] = ("GD.PrintRaw", "void"),
            ["prints"] = ("GD.PrintS", "void"),
            ["printt"] = ("GD.PrintT", "void"),
            ["push_error"] = ("GD.PushError", "void"),
            ["push_warning"] = ("GD.PushWarning", "void"),
            ["rad_to_deg"] = ("Mathf.RadToDeg", "float"),
            ["rand_from_seed"] = ("GD.RandFromSeed", "float"),
            ["randf"] = ("GD.Randf", "float"),
            ["randf_range"] = ("GD.RandRange", "float"),
            ["randfn"] = ("GD.Randfn", "float"),
            ["randi"] = ("GD.Randi", "int"),
            ["randi_range"] = ("GD.RandRange", "int"),
            ["randomize"] = ("GD.Randomize", "void"),
            ["remap"] = ("Mathf.Remap", "float"),
            ["round"] = ("Mathf.Round", "double"),
            ["roundf"] = ("Mathf.Round", "float"),
            ["roundi"] = ("Mathf.RoundToInt", "int"),
            ["seed"] = ("GD.Seed", "void"),
            ["sign"] = ("Mathf.Sign", "float"),
            ["signf"] = ("Mathf.Sign", "float"),
            ["signi"] = ("Mathf.Sign", "int"),
            ["sin"] = ("Mathf.Sin", "float"),
            ["sinh"] = ("Mathf.Sinh", "float"),
            ["smoothstep"] = ("Mathf.SmoothStep", "float"),
            ["snapped"] = ("Mathf.Snapped", "float"),
            ["snappedf"] = ("Mathf.Snapped", "float"),
            ["snappedi"] = ("Mathf.Snapped", "int"),
            ["sqrt"] = ("Mathf.Sqrt", "float"),
            ["step_decimals"] = ("Mathf.StepDecimals", "int"),
            ["str_to_var"] = ("GD.StrToVar", "object"),
            ["tan"] = ("Mathf.Tan", "float"),
            ["tanh"] = ("Mathf.Tanh", "float"),
            ["typeof"] = ("Variant.VariantType", "Type"),
            ["var_to_bytes"] = ("GD.VarToBytes", "byte[]"),
            ["var_to_bytes_with_objects"] = ("GD.VarToBytesWithObjects", "byte[]"),
            ["var_to_str"] = ("GD.VarToStr", "string"),
            ["weakref"] = ("GodotObject.WeakRef", "WeakRef"),
            ["wrap"] = ("Mathf.Wrap", "double"),
            ["wrapf"] = ("Mathf.Wrap", "float"),
            ["wrapi"] = ("Mathf.Wrap", "int")
        };

        public static Dictionary<string, (string csharpFunction, string type)> GDScriptGlobalScopeFunctions_TheirEquivalentAndReturnTypes_NAequivalent = new Dictionary<string, (string, string)>
        {
            ["rid_allocate_id"] = ("rid_allocate_id", "Rid"),
            ["rid_from_int64"] = ("rid_from_int64", "Rid")
            //["fmod"] = "operator %"
            //["str"] = (то, что в скобках).ToString()
        };

        public static Dictionary<string, (string csharpFunction, string type)> GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes = new Dictionary<string, (string, string)>
        {
            ["assert"] = ("System.Diagnostics.Debug.Assert", "void"),
            ["convert"] = ("GD.Convert", "object"),
            ["get_stack"] = ("System.Environment.StackTrace", "string"),
            ["load"] = ("GD.Load", "Resource"),
            ["preload"] = ("GD.Load", "Resource"),
            ["print_stack"] = ("GD.Print(System.Environment.StackTrace)", "void"),
            ["range"] = ("GD.Range", "IEnumerable"),
            ["type_exists"] = ("ClassDB.ClassExists(type)", "bool")
        };

        public static Dictionary<string, (string csharpFunction, string type)> GDScriptUtilityFunctions_TheirEquivalentAndReturnTypes_NAequivalen = new Dictionary<string, (string, string)>
        {
            ["dict_to_inst"] = ("dict_to_inst", "object"),
            ["inst_to_dict"] = ("inst_to_dict", "Dictionary"),
            ["len"] = ("len", "int"),
            ["print_debug"] = ("print_debug", "void")
            //["char"] = "Используйте явное преобразование: (char)65",
        };
    }
}
