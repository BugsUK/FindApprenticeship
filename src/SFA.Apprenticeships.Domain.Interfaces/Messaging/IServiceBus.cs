namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IServiceBus
    {
        void PublishMessage<T>(T message) where T : class;
    }
}
