using QueryBuilder.Enums;
using QueryBuilder.Interface;
using System.IO;

namespace QueryBuilder.Logger
{
    public static class Logger
    {
        private static string logFilePath = string.Empty;
        public static void Log(string message,LogLevel logLevel)
        {
            logFilePath = Path.Combine(@"C:\", "QueryCreatorLog.txt");
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);
            }
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine($"{logLevel} : {message}");
                }
            }            
        }
    }
}
