namespace SFA.Apprenticeships.Application.UnitTests.Applications.Processes
{
    using Application.Applications.Strategies;
    using Domain.Interfaces.Messaging;
    using Interfaces.Logging;
    using Moq;

    public class ApplicationStatusAlertStrategyBuilder
    {
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        
        private Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();

        public ApplicationStatusAlertStrategyBuilder With(Mock<IMessageBus> messageBus)
        {
            _messageBus = messageBus;
            return this;
        }

        public ApplicationStatusAlertStrategy Build()
        {
            var strategy = new ApplicationStatusAlertStrategy(_messageBus.Object, _logService.Object);
            return strategy;
        }
    }
}