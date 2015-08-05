namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using Microsoft.ServiceBus.Messaging;

    public interface ITopicClient
    {
        void Send(BrokeredMessage message);
        void Close();
    }
}