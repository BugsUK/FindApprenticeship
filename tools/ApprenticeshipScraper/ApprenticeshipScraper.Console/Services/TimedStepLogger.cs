namespace ApprenticeshipScraper.CmdLine.Services
{
    using System;
    using System.Diagnostics;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Steps;

    public class TimedStepLogger : IStepLogger
    {
        public Stopwatch Timer { get; set; }

        public void Info(string message)
        {
            Console.WriteLine($"[{this.Timer.ElapsedWithoutMs()}] -> {message}");
        }
    }
}