using System;

namespace Logging
{
    public interface IFileLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(string message, Exception ex);
    }
}