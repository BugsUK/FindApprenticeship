namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IServiceBusSubscriber<in TMessage>
        where TMessage : class 
    {
        ServiceBusMessageResult Consume(TMessage message);
    }
}