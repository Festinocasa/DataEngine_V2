namespace SourceCodeGenerator
{
    public class PropertyData
    {
        public string AccessModifier;
        public string PropertyModifier;
        public string PropertyType;
        public string PropertyName;
        public string SetterModifier;
        public string GetterModifier;
        public string DefaultValue;
        public XMLAnnotationData XMLAnnotation;
        public AnnotationData Annotation;

        public PropertyData(string accessModifier, string propertyModifier, string propertyType, string propertyName, string getterModifier = "", string setterModifier = "", string defaultVal = "", string annotation = "", string summuary = "")
        {
            AccessModifier = accessModifier;
            PropertyModifier = propertyModifier;
            PropertyType = propertyType;
            PropertyName = propertyName;
            SetterModifier = setterModifier;
            GetterModifier = getterModifier;
            DefaultValue = defaultVal;
            Annotation = new AnnotationData(annotation);
            XMLAnnotation = new XMLAnnotationData(summuary);
        }

        public string GenPropertyLineString(int defaultTab = 0)
        {
            string result = "";

            result += XMLAnnotation.GenXMLAnnotationString(defaultTab);

            for (int i = 0; i < defaultTab; i++)
            {
                result += "\t";
            }

            result += AccessModifier;

            if (PropertyModifier != ClassMemberModifierEnum.NOME)
            {
                result += " " + PropertyModifier;
            }

            result += " " + PropertyType;
            result += " " + PropertyName;

            result += " " + "{";
            if (GetterModifier != "")
            {
                result += " " + GetterModifier;
            }
            result += " " + "get;";

            if (SetterModifier != "")
            {
                result += " " + SetterModifier;
            }
            result += " " + "set;";
            result += " " + "}";

            if (DefaultValue != "")
            {
                result += " = " + DefaultValue;
                result += ";";
            }

            result += Annotation.GenAnnotationString();

            return result;
        }

    }
}
