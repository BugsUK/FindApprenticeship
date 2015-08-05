namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using System;

    public interface IBrokeredMessage
    {
        string MessageId { get; }
        DateTime ScheduledEnqueueTimeUtc { get; set; }
        T GetBody<T>();
        void Complete();
        void Abandon();
        void DeadLetter();
    }
}