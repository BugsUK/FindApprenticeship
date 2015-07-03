namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Configuration
{
    public class AzureServiceBusTopicConfiguration
    {
        public string TopicName { get; set; }

        public string MessageType { get; set; }
        
        public AzureServiceBusSubscriptionConfiguration[] Subscriptions { get; set; }
    }
}