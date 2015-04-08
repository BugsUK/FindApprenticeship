namespace SFA.Apprenticeships.Infrastructure.Monitor.Configuration
{
    public class MonitorConfiguration
    {
        public const string ConfigurationName = "MonitorConfiguration";

        public bool IsEnabled { get; set; }

        public bool IsDailyMetricsEnabled { get; set; }

        public int ValidNumberOfDaysSinceUserActivity { get; set; }

        public int ExpectedMongoReplicaSetCount { get; set; }

        public int ExpectedMinimumLogCount { get; set; }

        public int ExpectedLogTimeframeInMinutes { get; set; }

        public string DailyMetricsFromEmailAddress { get; set; }

        public string DailyMetricsToEmailAddress { get; set; }
    }
}
