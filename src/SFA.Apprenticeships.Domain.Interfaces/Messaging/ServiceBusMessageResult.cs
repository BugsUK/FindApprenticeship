using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public class ServiceBusMessageResult
    {
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

        public ServiceBusMessageStates State { get; private set; }

        public DateTime? RequeueDateTimeUtc { get; private set; }
    }
}
