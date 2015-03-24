namespace SFA.Apprenticeships.Infrastructure.Monitor.Configuration
{
    public class MonitorConfiguration
    {
        public const string ConfigurationName = "MonitorConfiguration";

        public int ExpectedMongoReplicaSetCount { get; set; }

        public int ExpectedMinimumLogCount { get; set; }

        public int ExpectedTimeframeInMinutes { get; set; }

        public string DailyMetricsFromEmailAddress { get; set; }

        public string DailyMetricsToEmailAddress { get; set; }
    }
}
