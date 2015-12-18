namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes.Builders
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Processes.Applications;
    using Infrastructure.Processes.Configuration;
    using Moq;

    public class ApplicationStatusChangedSubscriberBuilder
    {
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
        private Mock<IApplicationStatusAlertRepository> _applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
        private Mock<ILogService> _logService = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();

        public ApplicationStatusChangedSubscriberBuilder()
        {
            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    StrictEtlValidation = true
                });
        }

        public ApplicationStatusChangedSubscriberBuilder With(Mock<IApprenticeshipApplicationReadRepository> apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            return this;
        }

        public ApplicationStatusChangedSubscriberBuilder With(Mock<IApplicationStatusAlertRepository> applicationStatusAlertRepository)
        {
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            return this;
        }

        public ApplicationStatusChangedSubscriberBuilder With(Mock<ILogService> logService)
        {
            _logService = logService;
            return this;
        }

        public ApplicationStatusChangedSubscriber Build()
        {
            var subscriber = new ApplicationStatusChangedSubscriber(
                _apprenticeshipApplicationReadRepository.Object,
                _applicationStatusAlertRepository.Object,
                _logService.Object,
                _configurationService.Object);

            return subscriber;
        }
    }
}