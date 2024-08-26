namespace SourceCodeGenerator
{
    public class FieldData
    {
        public string AccessModifier;
        public string FieldModifier;
        public string FieldType;
        public string FieldName;
        public string DefaultValue;
        public XMLAnnotationData XMLAnnotation;
        public AnnotationData Annotation;

        public FieldData(string accessModifier, string fieldModifier, string fieldType, string fieldName, string defaultVal = "", string annotation = "", string summuary = "")
        {
            AccessModifier = accessModifier;
            FieldModifier = fieldModifier;
            FieldType = fieldType;
            FieldName = fieldName;
            DefaultValue = defaultVal;
            Annotation = new AnnotationData(annotation);
            XMLAnnotation = new XMLAnnotationData(summuary);
        }

        public string GenFieldLineString(int defaultTab = 0)
        {
            string result = "";

            result += XMLAnnotation.GenXMLAnnotationString(defaultTab);

            for (int i = 0; i < defaultTab; i++)
            {
                result += "\t";
            }

            result += AccessModifier;

            if (FieldModifier != ClassMemberModifierEnum.NOME)
            {
                result += " " + FieldModifier;
            }

            result += " " + FieldType;
            result += " " + FieldName;

            if (DefaultValue != "")
            {
                result += " = " + DefaultValue;
            }

            result += ";";

            result += Annotation.GenAnnotationString();

            return result;
        }

    }
}
