using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore
{

    /// <summary>
    /// The logger needs to be done. It now only logs to the console!
    /// </summary>
    public static class Logger
    {
        public enum LogLevels
        {
            Info,
            Warning,
            Error
        }

        public static void LogInfo(string text)
        {
            Console.WriteLine(text);
        }

        public static void LogError(string text)
        {
            Console.WriteLine(text);
        }

        public static void LogWarning(string text)
        {
            Console.WriteLine(text);
        }
    }
}
