namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Configuration
{
    public class AzureServiceBusSubscriptionConfiguration
    {
        public string SubscriptionName { get; set; }

        // public string SubscriberType { get; set; }
        
        public int? MessageCountWarningLimit { get; set; }
        
        public int? DeadLetterMessageCountWarningLimit { get; set; }
        
        public int? MaxConcurrentMessagesPerNode { get; set; }
    }
}