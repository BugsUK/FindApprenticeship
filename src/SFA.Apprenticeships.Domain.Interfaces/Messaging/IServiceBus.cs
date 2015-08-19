namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    using System.Collections.Generic;

    public interface IServiceBus
    {
        void PublishMessage<T>(T message) where T : class;

        int PublishMessages<T>(IEnumerable<T> message) where T : class;
    }
}
