namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Configuration
{
    public class AzureEventHubLogIndexerConfiguration
    {
        public string EventHubConnectionString { get; set; }

        public string EventHubPath { get; set; }

        public string StorageConnectionString { get; set; }

        public string ConsumerGroupName { get; set; }

        public string ElasticsearchHostName { get; set; }
    }
}
