namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Configuration
{
    public class AzureServiceBusConfiguration
    {
        public string ConnectionString { get; set; }

        public string TopicNameFormatter { get; set; }
        
        public int DefaultMessageCountWarningLimit { get; set; }
        
        public int DefaultDeadLetterMessageCountWarningLimit { get; set; }
        
        public int DefaultMaxConcurrentMessagesPerNode { get; set; }
     
        public AzureServiceBusTopicConfiguration[] Topics { get; set; }
    }
}