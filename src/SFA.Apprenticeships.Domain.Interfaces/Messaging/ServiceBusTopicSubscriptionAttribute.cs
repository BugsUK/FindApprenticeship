namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    using System;

    public class ServiceBusTopicSubscriptionAttribute : Attribute
    {
        public ServiceBusTopicSubscriptionAttribute()
        {
        }

        public string TopicName { get; set; }

        public string SubscriptionName { get; set; }
    }
}
