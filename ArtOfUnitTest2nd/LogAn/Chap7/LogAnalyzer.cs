using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.Chap7
{
    // このクラスは、内部でLoggingFacilityを使っている
    public class LogAnalyzerChap7
    {
        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                LoggingFacility.Log("Filename too short:" + fileName);
            }

            // 残りのメソッドの内容
        }
    }

    // 別のクラスも、内部でLoggingFacilityを使っている
    public class ConfigurationManager
    {
        public bool IsConfigured(string configureName)
        {
            LoggingFacility.Log("checking " + configureName);
            return true;
        }
    }


    public static class LoggingFacility
    {
        public static ILoggerChap7 Logger { get; set; }

        public static void Log(string text)
        {
            Logger.Log(text);
        }

    }

    public interface ILoggerChap7
    {
        void Log(string text);
    }


}
