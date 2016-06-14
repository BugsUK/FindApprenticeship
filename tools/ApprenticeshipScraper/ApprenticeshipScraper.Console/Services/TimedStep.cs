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
            var logger = underlying.Logger as TimedStepLogger;
            if (logger != null)
            {
                logger.Timer = this.timer;
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