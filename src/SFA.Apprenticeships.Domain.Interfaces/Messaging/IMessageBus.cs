namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IMessageBus
    {
        void PublishMessage<T>(T message) where T : class;
    }
}
