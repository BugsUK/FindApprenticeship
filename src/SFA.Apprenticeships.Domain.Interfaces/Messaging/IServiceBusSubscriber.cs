namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IServiceBusSubscriber<in TMessage>
        where TMessage : class 
    {
        ServiceBusMessageStates Consume(TMessage message);
    }
}