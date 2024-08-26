using DataEngine.Models;
using DataEngine.Utility;
using SourceCodeGenerator;

namespace DataEngine.Generator
{
    internal class ModelFileGenerator
    {
        private DataGeneratorContext generatorContext;
        private ClassData dataClassData;
        private ClassData idClassData;
        private MethodData constructorMethodData;
        private int fieldIndex;

        public ModelFileGenerator(DataGeneratorContext generatorContext)
        {
            this.generatorContext = generatorContext;
        }

        internal void WriteClass(string modelName)
        {
            fieldIndex = 0;
            dataClassData = new ClassData(AccessModifierEnum.PUBLIC, ClassModifierEnum.NONE, ClassTypeEnum.CLASS, modelName, typeof(ConfigData).Name);
            idClassData = null;
            constructorMethodData = new MethodData(AccessModifierEnum.PUBLIC, ClassMemberModifierEnum.NOME, MethodReturnTypeEnum.NONE, modelName,
                new Tuple<string, string>[1] { new("object[]", "paras") },
                new string[1] { new("paras") });
            dataClassData.AddMethod(constructorMethodData);
        }

        internal void WriteFieldInfo(FieldInfo fieldInfo)
        {
            var fieldType = FieldTypeManager.Instance.GetFieldType(fieldInfo.FieldTypeName);
            if (fieldInfo.FieldMetaTypeName == "ARR")
            {
                dataClassData.AddProperty(new PropertyData(AccessModifierEnum.PUBLIC, ClassMemberModifierEnum.NOME, $"{fieldType.TypeInfo.Name}[]", fieldInfo.FieldName,
                    "", AccessModifierEnum.PRIVATE, "", "", fieldInfo.FieldDescribe));
                constructorMethodData.AddMethodLine($"{fieldInfo.FieldName} = ReadArray<{fieldType.TypeInfo.Name}>(paras[{fieldIndex}]);");
            }
            else
            {
                dataClassData.AddProperty(new PropertyData(AccessModifierEnum.PUBLIC, ClassMemberModifierEnum.NOME, fieldType.TypeInfo.Name, fieldInfo.FieldName,
                    "", AccessModifierEnum.PRIVATE, "", "", fieldInfo.FieldDescribe));
                constructorMethodData.AddMethodLine($"{fieldInfo.FieldName} = ReadValue<{fieldType.TypeInfo.Name}>(paras[{fieldIndex}]);");
            }
            fieldIndex++;
        }


        internal void WriteElementId(ElementInfo elementInfo)
        {
            var enumIdCols = elementInfo.ElementMetaFieldsType.Where(str => str == "ENUMID").Select(str => elementInfo.ElementMetaFieldsType.IndexOf(str));
            foreach (var col in enumIdCols)
            {
                if (idClassData == null)
                    idClassData = new ClassData(AccessModifierEnum.PUBLIC, ClassModifierEnum.STATIC, ClassTypeEnum.CLASS, dataClassData.ClassName.ToUpper() + "_ID");
                string idKey = elementInfo.ElementValues[col];
                uint idValue = generatorContext.ElementIdDic[idKey];
                idClassData.AddField(new FieldData(AccessModifierEnum.PUBLIC, ClassMemberModifierEnum.CONST, "uint", idKey.ToUpper(), NumericTransfer.ConvertUInt32ToBinaryString(idValue), "",
                    $"HEX: {NumericTransfer.ConvertUInt32ToHexString(idValue)} DEC: {idValue}"));
            }
        }

        internal void FlushToModelFile(string exportModelFilePath)
        {
            var sourceGenerator = new SourceGenerator(exportModelFilePath, "DataEngine.Models");
            sourceGenerator.AddNamespaceRef("DataEngine.Protocal");
            sourceGenerator.NamespaceData.AddClass(dataClassData);
            if (idClassData != null)
                sourceGenerator.NamespaceData.AddClass(idClassData);
            sourceGenerator.GenerateSourceFile();
        }

    }

}