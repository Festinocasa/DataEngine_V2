using DataEngine;
using DataEngine.Models.TestConfig;

namespace Client
{
    public class Program
    {
        private static async Task Main()
        {
            string filePath = "D:\\BaiduSyncdisk\\Archive\\Game Projects\\2_Archive\\CS_DataEngineV2\\DataEngine.Models.TestConfig\\bin\\Debug\\net6.0\\Resources\\ConfigData.cds";
            string modelAssemblyName = "DataEngine.Models.TestConfig";

            ConfigManager configManager = ConfigManager.Instance;
            await configManager.LoadDataFileAsync(filePath, modelAssemblyName, new Progress<double>());

            configManager.GetConfig<TestAConfig>(TESTACONFIG_ID.TEST_ID_A, out var aConfigA);
            configManager.GetConfig<TestAConfig>(TESTACONFIG_ID.TEST_ID_B, out var aConfigB);
            configManager.GetConfig<TestAConfig>(TESTACONFIG_ID.TEST_ID_C, out var aConfigC);
            configManager.GetConfig<TestAConfig>(TESTACONFIG_ID.TEST_ID_D, out var aConfigD);
            configManager.GetConfigSet<TestDConfig>(out var testDConfigArr);

            configManager.GetConfig<TestCConfig>("Hellon", out var cConfigA);
            configManager.GetConfig<TestCConfig>("John", out var cConfigB);
            configManager.GetConfig<TestCConfig>("Helly", out var cConfigC);
            configManager.GetConfig<TestCConfig>("Sam", out var cConfigD);

            PrintTestAConfig(aConfigA);
            PrintTestAConfig(aConfigB);
            PrintTestAConfig(aConfigC);
            PrintTestAConfig(aConfigD);

            PrintTestCConfig(cConfigA);
            PrintTestCConfig(cConfigB);
            PrintTestCConfig(cConfigC);
            PrintTestCConfig(cConfigD);

            foreach (var dConfig in testDConfigArr)
                PrintTestDConfig(dConfig);
        }

        private static void PrintTestCConfig(TestCConfig cConfigData)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("TestC_Id: " + cConfigData.TestC_Id);
            Console.WriteLine("TestC_Id: " + cConfigData.TestC_NavId);
        }

        private static void PrintTestDConfig(TestDConfig dConfigData)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("TestD_Id: " + dConfigData.TestD_Id);
            Console.WriteLine("TestD_NavId: " + dConfigData.TestD_NavId);
        }

        private static void PrintTestAConfig(TestAConfig aConfigData)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("TestA_Id: " + aConfigData.TestA_Id);
            Console.WriteLine("TestA_NavId: " + aConfigData.TestA_NavId);
            Console.WriteLine("TestA_Int: " + aConfigData.TestA_Int);
            Console.WriteLine("TestA_Vec2: " + aConfigData.TestA_Vec2);
            Console.WriteLine("TestA_Str: " + aConfigData.TestA_Str);
            Console.Write("TestA_FloatArr: ");
            for (int i = 0; i < aConfigData.TestA_FloatArr.Length; i++)
                Console.Write(aConfigData.TestA_FloatArr[i] + " | ");
            Console.WriteLine("");
            Console.Write("TestA_StrArr: ");
            for (int i = 0; i < aConfigData.TestA_StrArr.Length; i++)
                Console.Write(aConfigData.TestA_StrArr[i] + " | ");
            Console.WriteLine("");
        }
    }

}
