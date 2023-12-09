using GDShrapt.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GDShrapt.Converter.Tests
{
    public class MethodNameHelper
    {
        public GDIdentifier MethodName;

        public readonly string MethodNameText;
        public readonly string ValidMethodName;

        public MethodNameHelper(GDIdentifier methodName)
        {
            MethodName = methodName;

            MethodNameText = MethodName.ToString();
            ValidMethodName = ValidateTypeAndNameHelper.GetValidateFieldName(MethodNameText);
        }
    }
}
