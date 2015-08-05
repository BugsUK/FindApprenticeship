namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using System;
    using Microsoft.ServiceBus.Messaging;

    public class BrokeredMessageWrapper : IBrokeredMessage
    {
        private readonly BrokeredMessage _brokeredMessage;

        public BrokeredMessageWrapper(BrokeredMessage brokeredMessage)
        {
            _brokeredMessage = brokeredMessage;
        }

        public string MessageId
        {
            get { return _brokeredMessage.MessageId; }
        }

        public DateTime ScheduledEnqueueTimeUtc
        {
            get { return _brokeredMessage.ScheduledEnqueueTimeUtc; }
            set { _brokeredMessage.ScheduledEnqueueTimeUtc = value; }
        }

        public T GetBody<T>()
        {
            return _brokeredMessage.GetBody<T>();
        }

        public void Complete()
        {
            _brokeredMessage.Complete();
        }

        public void Abandon()
        {
            _brokeredMessage.Abandon();
        }

        public void DeadLetter()
        {
            _brokeredMessage.DeadLetter();
        }
    }
}