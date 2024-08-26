using System.Collections.Generic;
using System.Text;

namespace SourceCodeGenerator
{
    public class NamespaceData
    {
        public string NamespaceName;
        private List<ClassData> classDatas;

        public NamespaceData(string nameSpaceName)
        {
            NamespaceName = nameSpaceName;
            classDatas = new List<ClassData>();
        }

        public void AddClass(ClassData classData)
        {
            classDatas.Add(classData);
        }

        public string GenNamespaceString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"namespace {NamespaceName}" + SourceGenerator.NewLineChar);
            sb.Append($"{{" + SourceGenerator.NewLineChar);
            for (int i = 0; i < classDatas.Count; i++)
            {
                ClassData classData = classDatas[i];
                sb.Append(classData.BuildClassString(1) + SourceGenerator.NewLineChar);
                if (i < classDatas.Count - 1)
                {
                    sb.Append(SourceGenerator.NewLineChar);
                }
            }
            sb.Append($"}}" + SourceGenerator.NewLineChar);
            return sb.ToString();
        }
    }
}
