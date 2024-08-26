using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace CSVExporter
{
    public class Program
    {
        private static string currentDirPath;
        private const string INI_SECTION_EXPORTCONFIG = "Exporter";
        private const string INI_KEY_EXPORTPATH = "ExportPath";
        private const string INI_EXPORTFOLDER = "CSVExportData";
        private const string INI_KEY_IMPORTFLODER = "ImportFolder";
        private const string SPLIT = "****************";
        private const string CONFIG_FILE = "CSVExporter.ini";

        private static void Main()
        {
            currentDirPath = Directory.GetCurrentDirectory();
            DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
            string configPath = Path.Combine(currentDirPath, CONFIG_FILE);
            if (File.Exists(configPath) == false)
            {
                string configDefault_ImportFolder = Path.Combine(currentDirPath, INI_EXPORTFOLDER);
                string configDefault_ExportPath = Path.Combine(currentDirPath, INI_EXPORTFOLDER);
                ConfigINI.INIWrite(INI_SECTION_EXPORTCONFIG, INI_KEY_IMPORTFLODER, configDefault_ExportPath, configPath);
                ConfigINI.INIWrite(INI_SECTION_EXPORTCONFIG, INI_KEY_EXPORTPATH, configDefault_ExportPath, configPath);
            }
            string importFolder = ConfigINI.INIRead(INI_SECTION_EXPORTCONFIG, INI_KEY_IMPORTFLODER, configPath);
            string exportPath = ConfigINI.INIRead(INI_SECTION_EXPORTCONFIG, INI_KEY_EXPORTPATH, configPath);

            DirectoryInfo importFolderInfo = new DirectoryInfo(importFolder);
            var fileInfos = importFolderInfo.GetFiles("*.xlsx", SearchOption.AllDirectories).ToList();
            for (int i = fileInfos.Count - 1; i >= 0; i--)
            {
                if (fileInfos[i].Name.StartsWith("~"))
                {
                    fileInfos.RemoveAt(i);
                }
            }

            IEnumerable<string> fileNames = fileInfos.Select(f => { return f.FullName; });
            string fileNamesJoined = string.Join("\n", fileNames);

            Console.WriteLine($"将以下文件:\n\n{SPLIT}\n\n{fileNamesJoined}\n\n{SPLIT}\n\n导出.csv至目录【{exportPath}】\n按Enter键继续");
            Console.ReadLine();

            DirectoryInfo exportDirInfo;
            if (Directory.Exists(exportPath) == false)
            {
                Console.WriteLine("目录不存在，是否自动生成？按任意键继续");
                Console.ReadLine();
                exportDirInfo = Directory.CreateDirectory(exportPath);
            }
            else
            {
                exportDirInfo = new DirectoryInfo(exportPath);
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Range sheetRange;

            foreach (var excelWorkbookPath in fileNames)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(excelWorkbookPath);
                Console.WriteLine($"{SPLIT}\n\nOpen wookbook: {excelWorkbook.Name}");
                foreach (Excel.Worksheet sheet in excelWorkbook.Sheets)
                {
                    sheetRange = sheet.UsedRange;
                    Console.WriteLine($"Process sheet: {sheet.Name} Row: {sheetRange.Rows.Count} Column: {sheetRange.Columns.Count}");
                    StringBuilder csvSB = new StringBuilder();
                    for (int i = 1; i <= sheetRange.Rows.Count; i++)
                    {
                        for (int j = 1; j <= sheetRange.Columns.Count; j++)
                        {
                            Excel.Range cell = sheetRange.Cells[i, j] as Excel.Range;
                            string cellVal = cell.Text.ToString();
                            Console.WriteLine($"Process cell: [{i}, {j}] Cell value: {cellVal}");
                            csvSB.Append(cellVal);
                            if (j < sheetRange.Columns.Count)
                            {
                                csvSB.Append(',');
                            }
                        }
                        csvSB.Append(Environment.NewLine);
                    }

                    string csvFilePath = Path.Combine(exportPath, sheet.Name + ".csv");
                    if (File.Exists(csvFilePath))
                    {
                        File.Delete(csvFilePath);
                    }

                    File.WriteAllText(csvFilePath, csvSB.ToString());
                }

                Console.WriteLine($"Read success: {excelWorkbook.Name}\n");
                excelWorkbook.Close(false);
                Marshal.ReleaseComObject(excelWorkbook);
            }

            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);

            Console.WriteLine("导出文件成功！");
            Console.ReadLine();
        }
    }
}
