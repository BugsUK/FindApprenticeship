namespace ApprenticeshipScraper.CmdLine.Services.Logger
{
    public interface IThreadSafeStepLogger : IStepLogger
    {
        IStepLogger UnderlyingStepLogger { get; }
    }
}