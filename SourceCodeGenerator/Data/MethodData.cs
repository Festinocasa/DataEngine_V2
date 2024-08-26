using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCodeGenerator
{
    public class MethodData
    {
        public string AccessModifier;
        public string ReturnType;
        public string MethodModifier;
        public string MethodName;
        public List<string> MethodBodyLines;
        public List<Tuple<string, string>> MethodArgs;
        public List<string> DerivatedParams;

        public MethodData(string accessModifier, string methodModifer, string returnType, string methodName, Tuple<string, string>[] methodArgs = null, string[] derivatedParams = null)
        {
            AccessModifier = accessModifier;
            MethodModifier = methodModifer;
            ReturnType = returnType;
            MethodName = methodName;
            MethodBodyLines = new List<string>();
            MethodArgs = new List<Tuple<string, string>>();
            if (methodArgs != null)
                MethodArgs.AddRange(methodArgs);
            DerivatedParams = new List<string>();
            if (derivatedParams != null)
                DerivatedParams.AddRange(derivatedParams);
        }

        public void AddArg(string argType, string argName)
        {
            MethodArgs.Add(new Tuple<string, string>(argType, argName));
        }

        public void AddMethodLine(string methodLine)
        {
            MethodBodyLines.Add(methodLine);
        }

        private string GenMethodHeadString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(AccessModifier + " ");
            if (MethodModifier != ClassMemberModifierEnum.NOME)
            {
                sb.Append(MethodModifier + " ");
            }
            if (ReturnType != MethodReturnTypeEnum.NONE)
            {
                sb.Append(ReturnType + " ");
            }
            sb.Append(MethodName);
            sb.Append("(");
            for (int i = 0; i < MethodArgs.Count; i++)
            {
                var argTup = MethodArgs[i];
                sb.Append($"{argTup.Item1} {argTup.Item2}");

                if (i < MethodArgs.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(")");
            if (DerivatedParams.Count != 0)
            {
                sb.Append(" : base(");
                for (int i = 0; i < DerivatedParams.Count; i++)
                {
                    var devStr = DerivatedParams[i];
                    sb.Append(devStr);
                    if (i < DerivatedParams.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(")");
            }

            return sb.ToString();
        }

        public string GenMethodString(int defaultTab = 0)
        {
            string tab = "";
            for (int i = 0; i < defaultTab; i++)
            {
                tab += "\t";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"{tab}{GenMethodHeadString()}" + SourceGenerator.NewLineChar);
            sb.Append($"{tab}{{" + SourceGenerator.NewLineChar);
            foreach (var methodLine in MethodBodyLines)
            {
                sb.Append($"{tab}\t{methodLine}" + SourceGenerator.NewLineChar);
            }
            sb.Append($"{tab}}}" + SourceGenerator.NewLineChar);

            return sb.ToString();
        }
    }
}
