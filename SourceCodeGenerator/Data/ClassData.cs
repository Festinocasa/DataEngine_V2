using System.Collections.Generic;
using System.Text;

namespace SourceCodeGenerator
{
    public class ClassData
    {
        public string AccessModifier;
        public string ClassType;
        public string ClassModifier;
        public string ClassName;
        public string ClassDerivated;

        private List<FieldData> fieldDatas;
        private List<PropertyData> propertyDatas;
        private List<MethodData> methodDatas;

        public ClassData(string accessModifier, string classModifier, string classType, string className, string classDerivated = "")
        {
            AccessModifier = accessModifier;
            ClassType = classType;
            ClassModifier = classModifier;
            ClassName = className;
            ClassDerivated = classDerivated;

            fieldDatas = new List<FieldData>();
            propertyDatas = new List<PropertyData>();
            methodDatas = new List<MethodData>();
        }

        public string GenClassHeadString()
        {
            string classType = "";
            if (ClassModifier == ClassModifierEnum.NONE)
            {
                classType = ClassType;
            }
            else
            {
                classType = ClassModifier + " " + ClassType;
            }

            string className = "";
            if (ClassDerivated == "")
            {
                className = ClassName;
            }
            else
            {
                className = ClassName + " : " + ClassDerivated;
            }

            return $"{AccessModifier} {classType} {className}";
        }

        public void AddField(FieldData fieldData)
        {
            fieldDatas.Add(fieldData);
        }

        public void AddProperty(PropertyData propertyData)
        {
            propertyDatas.Add(propertyData);
        }

        public void AddMethod(MethodData methodData)
        {
            methodDatas.Add(methodData);
        }

        public string BuildClassString(int defaultTab = 0)
        {
            string tab = "";
            for (int i = 0; i < defaultTab; i++)
            {
                tab += "\t";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"{tab}{GenClassHeadString()}" + SourceGenerator.NewLineChar);
            sb.Append($"{tab}{{" + SourceGenerator.NewLineChar);
            foreach (var fieldData in fieldDatas)
            {
                sb.Append($"{fieldData.GenFieldLineString(defaultTab + 1)}" + SourceGenerator.NewLineChar);
            }
            sb.Append(SourceGenerator.NewLineChar);

            foreach (var propertyData in propertyDatas)
            {
                sb.Append($"{propertyData.GenPropertyLineString(defaultTab + 1)}" + SourceGenerator.NewLineChar);
            }
            sb.Append(SourceGenerator.NewLineChar);

            foreach (var methodData in methodDatas)
            {
                sb.Append($"{methodData.GenMethodString(defaultTab + 1)}" + SourceGenerator.NewLineChar);
            }

            sb.Append($"{tab}}}" + SourceGenerator.NewLineChar);

            return sb.ToString();
        }
    }
}
