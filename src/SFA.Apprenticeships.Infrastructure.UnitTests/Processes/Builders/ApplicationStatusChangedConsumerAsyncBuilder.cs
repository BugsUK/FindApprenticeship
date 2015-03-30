namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes.Builders
{
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Processes.Applications;
    using Moq;

    public class ApplicationStatusChangedConsumerAsyncBuilder
    {
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
        private Mock<IApplicationStatusAlertRepository> _applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public ApplicationStatusChangedConsumerAsyncBuilder With(Mock<IApprenticeshipApplicationReadRepository> apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            return this;
        }

        public ApplicationStatusChangedConsumerAsyncBuilder With(Mock<IApplicationStatusAlertRepository> applicationStatusAlertRepository)
        {
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            return this;
        }

        public ApplicationStatusChangedConsumerAsyncBuilder With(Mock<ILogService> logService)
        {
            _logService = logService;
            return this;
        }

        public ApplicationStatusChangedConsumerAsync Build()
        {
            var consumer = new ApplicationStatusChangedConsumerAsync(_apprenticeshipApplicationReadRepository.Object, _applicationStatusAlertRepository.Object, _logService.Object);
            return consumer;
        }
    }
}