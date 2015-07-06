namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IServiceBusConsumer<in TMessage>
        where TMessage : class 
    {
        ServiceBusMessageResult Consume(TMessage message);
    }
}