namespace SFA.Apprenticeships.Application.UnitTests.Applications.Processes
{
    using Apprenticeships.Application.Applications.Strategies;
    using Domain.Interfaces.Messaging;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class ApplicationStatusAlertStrategyBuilder
    {
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();

        public ApplicationStatusAlertStrategyBuilder With(Mock<IServiceBus> serviceBus)
        {
            _serviceBus = serviceBus;
            return this;
        }

        public ApplicationStatusAlertStrategy Build()
        {
            var strategy = new ApplicationStatusAlertStrategy(_logService.Object, _serviceBus.Object);
            return strategy;
        }
    }
}