namespace ApprenticeshipScraper.CmdLine.Services
{
    using System;
    using System.Diagnostics;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services.Logger;
    using ApprenticeshipScraper.CmdLine.Steps;

    internal class TimedStep
    {
        private readonly IStep underlying;

        private Stopwatch timer;

        public TimedStep(IStep underlying)
        {
            this.underlying = underlying;
            this.timer = new Stopwatch();
            this.SetTimedStepLogger(underlying.Logger);

            var logger = underlying.Logger as ThreadSafeStepLogger;
            if (logger != null)
            {
                this.SetTimedStepLogger(logger.UnderlyingStepLogger);
            }
        }

        private void SetTimedStepLogger(IStepLogger underlying)
        {
            var timedStepLogger = underlying as TimedStepLogger;
            if (timedStepLogger != null)
            {
                timedStepLogger.Timer = this.timer;
            }
        }

        public void Run(ApplicationArguments arguments)
        {
            Console.WriteLine();
            Console.WriteLine($"[{timer.ElapsedWithoutMs()}] Starting {this.underlying.GetType().Name.SplitCamelCase()}");
            this.timer.Start();
            this.underlying.Run(arguments);
            this.timer.Stop();
            Console.WriteLine($"[{this.timer.ElapsedWithoutMs()}] Finished");
            Console.WriteLine();
        }
    }
}