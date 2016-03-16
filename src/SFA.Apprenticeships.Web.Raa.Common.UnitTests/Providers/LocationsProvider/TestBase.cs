namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.LocationsProvider
{
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;
    using Common.Providers;
    using Domain.Entities.Raa.Parties;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Web.Common.Configuration;

    public abstract class TestBase
    {
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<ILogService> _mockLogService;
        private Mock<IReferenceDataService> _mockReferenceDataService;
        private Mock<IDateTimeService> _mockTimeService;
        private Mock<IApprenticeshipApplicationService> _mockApprenticeshipApplicationService;
        private Mock<ITraineeshipApplicationService> _mockTraineeshipApplicationService;
        private Mock<IVacancyLockingService> _mockVacancyLockingService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IEmployerService> MockEmployerService;
        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUpBase()
        {
            _mockLogService = new Mock<ILogService>();
            _mockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            MockEmployerService = new Mock<IEmployerService>();
            _mockReferenceDataService = new Mock<IReferenceDataService>();

            MockProviderService.Setup(s => s.GetProviderSite(It.IsAny<int>()))
                .Returns(new Fixture().Build<ProviderSite>().Create());
            MockEmployerService.Setup(s => s.GetEmployer(It.IsAny<int>()))
                .Returns(new Fixture().Build<Employer>().Create());
            _mockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());

            _mockTimeService = new Mock<IDateTimeService>();
            _mockApprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
            _mockTraineeshipApplicationService = new Mock<ITraineeshipApplicationService>();
            _mockVacancyLockingService = new Mock<IVacancyLockingService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyProvider(_mockLogService.Object,
                _mockConfigurationService.Object,
                MockVacancyPostingService.Object,
                _mockReferenceDataService.Object,
                MockProviderService.Object,
                MockEmployerService.Object,
                _mockTimeService.Object,
                MockMapper.Object,
                _mockApprenticeshipApplicationService.Object,
                _mockTraineeshipApplicationService.Object,
                _mockVacancyLockingService.Object);
        }
    }
}