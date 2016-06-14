namespace ApprenticeshipScraper.CmdLine.Services.Logger
{
    using System;
    using System.Diagnostics;

    using ApprenticeshipScraper.CmdLine.Extensions;

    public class TimedStepLogger : IStepLogger
    {
        public Stopwatch Timer { get; set; }

        public void Info(string message)
        {
            Console.WriteLine($"[{this.Timer.ElapsedWithoutMs()}] -> {message}");
        }

        public void Error(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{this.Timer.ElapsedWithoutMs()}] -> {message} {exception.ToNiceString()}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}