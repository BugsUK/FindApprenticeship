namespace ApprenticeshipScraper.CmdLine.Settings
{
    public interface IGlobalSettings
    {
        int MaxDegreeOfParallelism { get; }

        int WaitBetweenRequestsMs { get; }

        string PageSize { get; }
    }
}