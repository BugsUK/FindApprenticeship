namespace ApprenticeshipScraper.CmdLine.Settings
{
    using System;
    using System.Configuration;

    public class GlobalSettings : IGlobalSettings
    {
        public int MaxDegreeOfParallelism => Convert.ToInt32(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"] ?? "4");

        public int WaitBetweenRequestsMs
            => Convert.ToInt32(ConfigurationManager.AppSettings["WaitBetweenRequestsMs"] ?? "20");

        public string PageSize => ConfigurationManager.AppSettings["PageSize"] ?? "100";
    }
}