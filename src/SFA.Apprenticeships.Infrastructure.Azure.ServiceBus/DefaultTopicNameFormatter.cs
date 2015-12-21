namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    public class DefaultTopicNameFormatter : ITopicNameFormatter
    {
        public string GetTopicName(string topicName)
        {
            return topicName;
        }
    }
}