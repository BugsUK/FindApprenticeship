namespace ApprenticeshipScraper.CmdLine.Services.Logger
{
    using System;

    public interface IStepLogger
    {
        void Info(string message);

        void Error(string message, Exception exception);

        void Warn(string message);
    }
}