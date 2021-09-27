using System;
using System.IO;

namespace Logging
{
    public class FileLogger : IFileLogger
    {
        public void LogError(string message)
        {
            LogMessage($"Error : {message}");
        }

        public void LogError(string message, Exception ex)
        {
            LogMessage($"Error: {message} - Exception : {ex.Message} - {ex.StackTrace}");
        }

        public void LogInfo(string message)
        {
            LogMessage($"Info : {message}");
        }

        public void LogWarning(string message)
        {
            LogMessage($"Warn : {message}");
        }


        private void LogMessage(string message)
        {
            using(StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine($"{DateTime.UtcNow} : {message}", w);
            }
        }
    }
}