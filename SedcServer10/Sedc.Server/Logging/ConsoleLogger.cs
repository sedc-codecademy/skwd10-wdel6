using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Logging
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel LogLevel { get; init; } = LogLevel.Debug;

        public void Log(LogLevel level, string message)
        {
            if (level >= LogLevel) {
                Console.WriteLine($"[{level}] {message}");
            }
        }
    }
}
