using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SourceCodeGenerator
{
    public class SourceGenerator
    {
        private const string SOURCE_HEAD = "//+-------------------------------------------------+\r\n" +
                                           "//|        This source code is auto generated       |\r\n" +
                                           "//+-------------------------------------------------+";

        public NamespaceData NamespaceData;

        private List<string> namespaceReference;
        private StreamWriter sw;
        private StringBuilder sb;
        public const string NewLineChar = "\r\n";

        public SourceGenerator(string path, string namespaceName)
        {
            sw = new StreamWriter(path);
            sb = new StringBuilder();

            namespaceReference = new List<string>();
            NamespaceData = new NamespaceData(namespaceName);

            DefualtNamespaceRef();
        }

        private void DefualtNamespaceRef()
        {
            namespaceReference.Add("System");
            namespaceReference.Add("System.Collections.Generic");
        }

        public void AddNamespaceRef(string namespaceRefString)
        {
            namespaceReference.Add(namespaceRefString);
        }

        public void GenerateSourceFile()
        {
            sb.Append(SOURCE_HEAD + NewLineChar);
            sb.Append(NewLineChar);
            foreach (var refString in namespaceReference)
            {
                sb.Append($"using {refString};" + NewLineChar);
            }
            sb.Append(NewLineChar);
            sb.Append(NamespaceData.GenNamespaceString());

            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }
    }
}
