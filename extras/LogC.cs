
using System;

namespace caching_proxy.extras
{
    public class LogC : ILogC
    {
        private string _origin;

        public LogC(string origin)
        {
            _origin = origin ?? "Unknown";
        }

        private void Write(string level, ConsoleColor color, string message)
        {
            var previousColor = Console.ForegroundColor;

            // Timestamp en formato: [2025-11-07 15:42:10]
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine();

            Console.ForegroundColor = color;
            Console.Write($"[{timestamp}] [{level}] [{_origin}] ");

            Console.ForegroundColor = previousColor;
            Console.WriteLine(message);
        }
        
        public void Debug(string message)
        {
            Write("DEBUG", ConsoleColor.Gray, message);
        }

        public void Error(string message)
        {
            Write("ERROR", ConsoleColor.Red, message);
        }

        public void Info(string message)
        {
            Write("INFO", ConsoleColor.Cyan, message);
        }

        public void Warn(string message)
        {
            Write("WARNING", ConsoleColor.Yellow, message);
        }

        public void Ok(string message)
        {
            Write("OK", ConsoleColor.Green, message);
        }
    }
}