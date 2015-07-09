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

        [ServiceBusTopicSubscription(TopicName = "sms-request")]
        public ServiceBusMessageResult Consume(SmsRequest request)
        {
            try
            {
                _dispatcher.SendSms(request);
                return ServiceBusMessageResult.Complete();
            }
            catch (Exception)
            {
                request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);

                return ServiceBusMessageResult.Requeue(request.ProcessTime.Value);
            }
        }
    }
}
