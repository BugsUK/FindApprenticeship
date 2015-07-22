namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class EmailRequestSubscriber : IServiceBusSubscriber<EmailRequest>
    {
        private readonly IEmailDispatcher _dispatcher;

        public EmailRequestSubscriber(IEmailDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [ServiceBusTopicSubscription(TopicName = "SendEmail")]
        public ServiceBusMessageResult Consume(EmailRequest request)
        {
            _dispatcher.SendEmail(request);

            return ServiceBusMessageResult.Complete();
        }
    }
}
