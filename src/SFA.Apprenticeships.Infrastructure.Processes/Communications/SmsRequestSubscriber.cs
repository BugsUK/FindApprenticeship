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
        public ServiceBusMessageResult Consume(SmsRequest request)
        {
            try
            {
                _dispatcher.SendSms(request);
                return ServiceBusMessageResult.Complete();
            }
            catch (Exception)
            {
                request.ProcessTime = request.ProcessTime.HasValue ? DateTime.UtcNow.AddMinutes(5) : DateTime.UtcNow.AddSeconds(30);

                return ServiceBusMessageResult.Requeue(request.ProcessTime.Value);
            }
        }
    }
}
