using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Logging
{
    public interface ILogger
    {
        LogLevel LogLevel { get; }

        void Log(LogLevel level, string message);

        void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        void Warn(string message)
        {
            Log(LogLevel.Warning, message);
        }

        void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        void Fatal(string message)
        {
            Log(LogLevel.Fatal, message);
        }
    }



    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5,
    }

}
