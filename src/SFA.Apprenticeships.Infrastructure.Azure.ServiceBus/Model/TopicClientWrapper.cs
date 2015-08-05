namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using Microsoft.ServiceBus.Messaging;

    public class TopicClientWrapper : ITopicClient
    {
        private readonly TopicClient _topicClient;

        public TopicClientWrapper(TopicClient topicClient)
        {
            _topicClient = topicClient;
        }

        public void Send(BrokeredMessage message)
        {
            _topicClient.Send(message);
        }

        public void Close()
        {
            _topicClient.Close();
        }
    }
}