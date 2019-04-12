using System;

namespace WebApi1
{
    public interface ILogger
    {
        void Log(string message);

        void Log(string message, Exception e);
    }
}