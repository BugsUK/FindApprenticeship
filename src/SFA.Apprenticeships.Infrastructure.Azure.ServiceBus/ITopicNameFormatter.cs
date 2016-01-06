namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    public interface ITopicNameFormatter
    {
        string GetTopicName(string topicName);
    }
}