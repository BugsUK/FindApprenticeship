namespace ApprenticeshipScraper.CmdLine.Services.Logger
{
    using System;

    public class ThreadSafeStepLogger : IThreadSafeStepLogger
    {
        private readonly IStepLogger logger;

        static object mutex = new object();

        public ThreadSafeStepLogger(IStepLogger logger)
        {
            this.logger = logger;
        }

        public void Info(string message)
        {
            lock (mutex)
            {
                this.logger.Info(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            lock (mutex)
            {
                this.Error(message,exception);
            }
        }
    }
}