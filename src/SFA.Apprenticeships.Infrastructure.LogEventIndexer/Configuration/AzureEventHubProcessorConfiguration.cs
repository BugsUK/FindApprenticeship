namespace SFA.Apprenticeships.Infrastructure.Log.Configuration
{
    public class AzureEventHubLogProcessorConfiguration
    {
        public string EventHubConnectionString { get; set; }

        public string EventHubPath { get; set; }

        public string StorageConnectionString { get; set; }

        public string ConsumerGroupName { get; set; }
    }
}
