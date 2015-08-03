namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class SmsRequestSubscriber : IServiceBusSubscriber<SmsRequest>
    {
        private readonly ISmsDispatcher _dispatcher;

        public SmsRequestSubscriber(ISmsDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [ServiceBusTopicSubscription(TopicName = "SendSms")]
        public ServiceBusMessageStates Consume(SmsRequest request)
        {
            try
            {
                _dispatcher.SendSms(request);
                return ServiceBusMessageStates.Complete;
            }
            catch (Exception)
            {
                return ServiceBusMessageStates.Requeue;
            }
        }
    }
}
