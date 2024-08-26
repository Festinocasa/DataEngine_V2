namespace DataEngine.Generator
{
    internal class DataEngineGenerator
    {
        private DataGeneratorContext generatorContext;
        private DataFileWriter fileWriter;
        private ModelFileGenerator modelGenerator;

        public DataEngineGenerator()
        {
            generatorContext = new DataGeneratorContext();
            fileWriter = new DataFileWriter(generatorContext);
            modelGenerator = new ModelFileGenerator(generatorContext);
        }

        internal void ParseCsv(string modelName, Dictionary<int, string[]> dataTable)
        {
            var rowMax = dataTable.Count;
            var colMax = dataTable[0].Length;
            var sheetInfo = new SheetInfo();
            sheetInfo.SheetModelName = modelName;
            sheetInfo.ElemCount = rowMax - 4;

            modelGenerator.WriteClass(modelName);

            for (int col = 0; col < colMax; col++)
            {
                var fieldInfo = new FieldInfo();

                fieldInfo.FieldName = dataTable[0][col];
                fieldInfo.FieldTypeName = dataTable[1][col];
                fieldInfo.FieldMetaTypeName = dataTable[2][col];
                fieldInfo.FieldDescribe = dataTable[3][col];

                if (fieldInfo.FieldMetaTypeName == "NOTF")
                    continue;

                sheetInfo.FieldStrings.Add(fieldInfo.FieldName);
                sheetInfo.ElemByteLen += FieldTypeManager.Instance.GetFieldType(fieldInfo.FieldTypeName).TableBodyByteSize;
                fileWriter.WriteFieldInfo(fieldInfo);
                modelGenerator.WriteFieldInfo(fieldInfo);
            }

            for (int row = 4; row < rowMax; row++)
            {
                var elementInfo = new ElementInfo();
                for (int col = 0; col < colMax; col++)
                {
                    elementInfo.ElementFieldsType.Add(dataTable[1][col]);
                    elementInfo.ElementMetaFieldsType.Add(dataTable[2][col]);
                    elementInfo.ElementValues.Add(dataTable[row][col]);
                }
                fileWriter.WriteElement(elementInfo);
                modelGenerator.WriteElementId(elementInfo);
            }
            fileWriter.WriteSheetInfo(sheetInfo);
        }

        internal void GenerateDataFile(Version version, string exportDataFilePath)
        {
            fileWriter.FlushToFile(version, exportDataFilePath);
        }

        internal void GenerateModelFile(string exportModelFilePath)
        {
            modelGenerator.FlushToModelFile(exportModelFilePath);
        }

    }

}