using IniParser;
using IniParser.Model;

namespace DataEngine.Generator
{
    internal class Program
    {
        static void Main()
        {
            var currentDirPath = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(currentDirPath, "config.ini");
            var csvImportFolder = string.Empty;
            var dataExportFolder = string.Empty;
            var modelExportFolder = string.Empty;

            if (!File.Exists(configPath))
            {
                var parser = new FileIniDataParser();
                var data = new IniData();

                data["Settings"]["CsvImportFolder"] = "";
                data["Settings"]["DataExportFolder"] = "";
                data["Settings"]["ModelExportFolder"] = "";

                parser.WriteFile(configPath, data);
                Console.WriteLine($"新配置文件已生成：{configPath}");
                Environment.Exit(-1);
            }
            else
            {
                var parser = new FileIniDataParser();
                var data = parser.ReadFile(configPath);

                csvImportFolder = data["Settings"]["CsvImportFolder"];
                dataExportFolder = data["Settings"]["DataExportFolder"];
                modelExportFolder = data["Settings"]["ModelExportFolder"];

                if (!Directory.Exists(csvImportFolder) || !Directory.Exists(dataExportFolder) || !Directory.Exists(modelExportFolder))
                    Environment.Exit(-2);

                Console.WriteLine($"--配置文件已加载：{configPath}");
                Console.WriteLine($"----CsvImportFolder:{csvImportFolder}");
                Console.WriteLine($"----DataExportFolder:{dataExportFolder}");
                Console.WriteLine($"----DataExportFolder:{modelExportFolder}");
            }

            var csvDirInfo = new DirectoryInfo(csvImportFolder);
            var csvFiles = csvDirInfo.GetFiles("*.csv", SearchOption.AllDirectories).ToList();
            var generator = new DataEngineGenerator();

            int i = 0;
            foreach (var csvFile in csvFiles)
            {
                StreamReader sr = new StreamReader(csvFile.FullName);
                Dictionary<int, string[]> dataTable = new Dictionary<int, string[]>();
                int row = 0;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                        continue;
                    var elems = line.Split(',').Select(str => str.Replace(" ", "")).ToArray();
                    dataTable.Add(row, elems);
                    row++;
                }
                var modelName = csvFile.Name.Replace(".csv", "") + "Config";
                Console.WriteLine($"----正在解析Csv: {csvFile.FullName}");
                generator.ParseCsv(modelName, dataTable);
                string exportModelFilePath = Path.Combine(modelExportFolder, modelName + ".cs");
                Console.WriteLine($"----正在生成模型: {exportModelFilePath}");
                generator.GenerateModelFile(exportModelFilePath);
                i++;
            }

            string exportDataFilePath = Path.Combine(dataExportFolder, "ConfigData.cds");
            Console.WriteLine($"----正在生成数据文件: {exportDataFilePath}");
            generator.GenerateDataFile(new Version(), exportDataFilePath);
            Console.WriteLine($"----生成完成");
            Console.ReadLine();
        }

    }

}
