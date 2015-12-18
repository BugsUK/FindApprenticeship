namespace SFA.Infrastructure.Console
{
    using Interfaces;
    using System;

    public class ConsoleLogService : ILogService
    {
        public void Debug(string message, params object[] args)
        {
            Log("DEBUG", message, null, args);
        }

        public void Info(string message, params object[] args)
        {
            Log("INFO", message, null, args);
        }

        public void Warn(string message, Exception exception, params object[] args)
        {
            Log("WARN", message, null, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log("WARN", message, null, args);
        }

        public void Warn(Exception exception, object data = null)
        {
            Log("WARN", data == null ? "" : data.ToString(), exception, new object[] { });
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            Log("ERROR", message, exception, args);
        }

        public void Error(string message, params object[] args)
        {
            Log("ERROR", message, null, args);
        }

        public void Error(Exception exception, object data = null)
        {
            Log("ERROR", data == null ? "" : data.ToString(), exception, new object[] { });
        }

        private void Log(string level, string message, Exception exception, object[] args)
        {
            // $"{level:0,5}"
            Console.WriteLine(string.Format("{0,-6} ", level + ":") + string.Format(message, args));
            if (exception != null)
                Console.WriteLine(exception.ToString());
        }
    }
}
