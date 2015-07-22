namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System.Threading.Tasks;
    using Application.Interfaces.Communications;
    using EasyNetQ.AutoSubscribe;

    public class EmailRequestConsumerAsync : IConsumeAsync<EmailRequest>
    {
        private readonly IEmailDispatcher _dispatcher;

        public EmailRequestConsumerAsync(IEmailDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        //TODO: Potentially up this value once we stop seeing errors
        // [SubscriptionConfiguration(PrefetchCount = 5)]
        // [AutoSubscriberConsumer(SubscriptionId = "EmailRequestConsumerAsync")]
        public Task Consume(EmailRequest request)
        {
            return Task.Run(() => _dispatcher.SendEmail(request));
        }
    }
}
