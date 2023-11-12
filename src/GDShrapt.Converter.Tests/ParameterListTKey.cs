using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace GDShrapt.Converter.Tests
{
    public struct ParameterListTKey
    {
        public List<ParameterSyntax> ParametersSyntax;

        public ParameterListTKey(List<ParameterSyntax> parametersSyntax)
        {
            ParametersSyntax = parametersSyntax;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (ParametersSyntax == null)
                return "".GetHashCode();

            foreach (var coll in ParametersSyntax)
                hashCode ^= coll.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is ParameterListTKey coll)
            {
                if (ParametersSyntax == null && ParametersSyntax == null)
                    return true;
                else if (ParametersSyntax == null || ParametersSyntax == null)
                    return false;
                else if (ParametersSyntax.Count != coll.ParametersSyntax.Count)
                    return false;

                for (int i = 0; i < ParametersSyntax.Count; i++)
                {
                    var type1 = ParametersSyntax[i].Type.GetText().ToString();
                    var type2 = coll.ParametersSyntax[i].Type.GetText().ToString();

                    if (type1 != type2)
                        return false;
                }

                return true;
            }
            else
                return base.Equals(obj);
        }
    }
}
