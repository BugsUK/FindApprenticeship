namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System.Diagnostics;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;

    internal interface IStep
    {
        IStepLogger Logger { get; }
        void Run(ApplicationArguments arguments);
    }
}