namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Processes.Builders
{
    using Application.Interfaces.Communications;
    using Infrastructure.Processes.Communications;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class EmailRequestSubscriberBuilder
    {
        private Mock<IEmailDispatcher> _dispatcher = new Mock<IEmailDispatcher>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>(); 

        public EmailRequestSubscriber Build()
        {
            var subscriber = new EmailRequestSubscriber(_dispatcher.Object, _logService.Object);
            return subscriber;
        }

        public EmailRequestSubscriberBuilder With(Mock<IEmailDispatcher> dispatcher)
        {
            _dispatcher = dispatcher;
            return this;
        }
    }
}