using System;

namespace WebApi1
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, Exception e)
        {
            Console.WriteLine($"{message}  {e.Message}");
        }
    }
}