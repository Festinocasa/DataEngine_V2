using System.Runtime.InteropServices;
using System.Text;

namespace CSVExporter
{
    public static class ConfigINI
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defaultVal, StringBuilder returnVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);

        public static void INIWrite(string section, string key, string val, string path)
        {
            WritePrivateProfileString(section, key, val, path);
        }

        public static string INIRead(string section, string key, string path)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, "", sb, 1024, path);
            return sb.ToString();
        }
    }
}
