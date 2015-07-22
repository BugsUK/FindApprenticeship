namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    public interface IServiceBusMessageBroker
    {
        void Subscribe();

        void Unsubscribe();
    }

    public interface IServiceBusMessageBroker<TMessage> : IServiceBusMessageBroker
        where TMessage : class
    {
    }
}