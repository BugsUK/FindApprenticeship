namespace ApprenticeshipScraper.CmdLine.Services.Logger
{
    using System;

    public class ThreadSafeStepLogger : IThreadSafeStepLogger
    {
        static object mutex = new object();

        public IStepLogger UnderlyingStepLogger { get; }

        public ThreadSafeStepLogger(IStepLogger logger)
        {
            this.UnderlyingStepLogger = logger;
        }

        public void Info(string message)
        {
            lock (mutex)
            {
                this.UnderlyingStepLogger.Info(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            lock (mutex)
            {
                this.UnderlyingStepLogger.Error(message,exception);
            }
        }
    }
}