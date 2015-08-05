namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Factory
{
    using Microsoft.ServiceBus.Messaging;
    using Model;

    public class ClientFactory : IClientFactory
    {
        public ITopicClient CreateFromConnectionString(string connectionString, string path)
        {
            var topicClient = TopicClient.CreateFromConnectionString(connectionString, path);
            return new TopicClientWrapper(topicClient);
        }

        public ISubscriptionClient CreateFromConnectionString(string connectionString, string topicPath, string name, ReceiveMode mode)
        {
            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString, topicPath, name, mode);
            return new SubscriptionClientWrapper(subscriptionClient);
        }
    }
}