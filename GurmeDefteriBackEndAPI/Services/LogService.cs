using System;
using System.IO;

namespace GurmeDefteriBackEndAPI.Services
{
    public class LogService
    {
        private readonly string _logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        public string FindLogFileByDate(DateTime date)
        {
            string dateString = date.ToString("yyyyMMdd");
            string logFileName = $"gurmeDefteriLogs-{dateString}.txt";
            string logFilePath = Path.Combine(_logFolderPath, logFileName);

            if (File.Exists(logFilePath))
            {
                string logContent = File.ReadAllText(logFilePath);
                return logContent;
            }

            return null;
        }
    }
}
