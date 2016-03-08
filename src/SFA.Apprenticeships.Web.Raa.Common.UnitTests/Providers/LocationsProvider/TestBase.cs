namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.LocationsProvider
{
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;
    using Common.Providers;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Web.Common.Configuration;

    public abstract class TestBase
    {
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<ILogService> MockLogService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IEmployerService> MockEmployerService;
        protected Mock<IUserProfileService> MockUserProfileService;
        protected Mock<IReferenceDataService> MockReferenceDataService;
        protected Mock<IDateTimeService> MockTimeService;
        protected Mock<IApprenticeshipApplicationService> ApprenticeshipApplicationService;
        protected Mock<ITraineeshipApplicationService> TraineeshipApplicationService;
        protected Mock<IVacancyReadRepository> MockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        protected Mock<IVacancyWriteRepository> MockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();

        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUpBase()
        {
            MockLogService = new Mock<ILogService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();
            MockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
            MockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            MockEmployerService = new Mock<IEmployerService>();
            MockUserProfileService = new Mock<IUserProfileService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();

            MockProviderService.Setup(s => s.GetProviderSite(It.IsAny<int>()))
                .Returns(new Fixture().Build<ProviderSite>().Create());
            MockEmployerService.Setup(s => s.GetEmployer(It.IsAny<int>()))
                .Returns(new Fixture().Build<Employer>().Create());
            MockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());

            MockTimeService = new Mock<IDateTimeService>();
            ApprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
            TraineeshipApplicationService = new Mock<ITraineeshipApplicationService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyProvider(MockLogService.Object,
                MockConfigurationService.Object,
                MockVacancyPostingService.Object,
                MockReferenceDataService.Object,
                MockProviderService.Object,
                MockEmployerService.Object,
                MockTimeService.Object,
                MockMapper.Object,
                ApprenticeshipApplicationService.Object,
                TraineeshipApplicationService.Object,
                MockUserProfileService.Object);
        }
    }
}