namespace SourceCodeGenerator
{
    public class AnnotationData
    {
        public string AnnotationStr;

        public AnnotationData(string annotation)
        {
            AnnotationStr = annotation;
        }

        public string GenAnnotationString()
        {
            if (AnnotationStr == "")
                return "";
            return $" // {AnnotationStr}";
        }

    }
}