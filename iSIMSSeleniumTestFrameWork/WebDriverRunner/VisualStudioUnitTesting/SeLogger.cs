using System;

namespace WebDriverRunner
{
    public class SeLogger : SeSugar.Interfaces.ILogger
    {
        private readonly bool _enabled = false;

        public void Log(string message)
        {
            if (_enabled)
                Console.Write(message);
        }

        public void Log(string messageFormat, params object[] args)
        {
            if (_enabled)
                Console.Write(messageFormat, args);
        }

        public void LogLine(string message)
        {
            if (_enabled)
                Console.WriteLine(message);
        }

        public void LogLine(string messageFormat, params object[] args)
        {
            if (_enabled)
                Console.WriteLine(messageFormat, args);
        }
    }
}
