namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IServiceBusSubscriber<in T>
         where T : class
    {
        ServiceBusMessageResult Consume(T message);
    }
}
