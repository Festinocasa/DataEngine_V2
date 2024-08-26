using System.Text;

namespace SourceCodeGenerator
{
    public class XMLAnnotationData
    {
        public string Summary;

        public XMLAnnotationData(string summary)
        {
            Summary = summary;
        }

        public string GenXMLAnnotationString(int defalutTab = 0)
        {
            if (Summary == "")
                return "";

            string tab = "";
            for (int i = 0; i < defalutTab; i++)
            {
                tab += "\t";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"{tab}/// <summary>" + SourceGenerator.NewLineChar);
            sb.Append($"{tab}/// {Summary}" + SourceGenerator.NewLineChar);
            sb.Append($"{tab}/// </summary>" + SourceGenerator.NewLineChar);

            return sb.ToString();
        }
    }
}