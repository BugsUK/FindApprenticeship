namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Factory
{
    using Microsoft.ServiceBus.Messaging;
    using Model;

    public interface IClientFactory
    {
        ITopicClient CreateFromConnectionString(string connectionString, string path);

        ISubscriptionClient CreateFromConnectionString(string connectionString, string topicPath, string name, ReceiveMode mode);
    }
}