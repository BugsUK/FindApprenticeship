using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public sealed class ServiceBusMessageResult
    {
        private ServiceBusMessageResult()
        {    
        }

        public ServiceBusMessageStates State { get; private set; }

        public DateTime? RequeueDateTimeUtc { get; private set; }

        public static ServiceBusMessageResult Complete()
        {
            return new ServiceBusMessageResult
            {
                State = ServiceBusMessageStates.Complete
            };
        }

        public static ServiceBusMessageResult Reqeue(DateTime reqeueDateTimeUtc)
        {
            return new ServiceBusMessageResult
            {
                State = ServiceBusMessageStates.Requeue,
                RequeueDateTimeUtc = reqeueDateTimeUtc
            };
        }

        public static ServiceBusMessageResult Abandon()
        {
            return new ServiceBusMessageResult
            {
                State = ServiceBusMessageStates.Abandon
            };
        }

        public static ServiceBusMessageResult DeadLetter()
        {
            return new ServiceBusMessageResult
            {
                State = ServiceBusMessageStates.DeadLetter
            };
        }
    }
}
