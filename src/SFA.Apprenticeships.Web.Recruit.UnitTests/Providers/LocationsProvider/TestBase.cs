namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.LocationsProvider
{
    using Application.Interfaces.Applications;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using SFA.Infrastructure.Interfaces;

    public abstract class TestBase
    {
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<ILogService> MockLogService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IUserProfileService> MockUserProfileService;
        protected Mock<IReferenceDataService> MockReferenceDataService;
        protected Mock<IDateTimeService> MockTimeService;
        protected Mock<IApprenticeshipApplicationService> ApprenticeshipApplicationService;
        protected Mock<IApprenticeshipVacancyReadRepository> MockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
        protected Mock<IApprenticeshipVacancyWriteRepository> MockApprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();

        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUpBase()
        {
            MockLogService = new Mock<ILogService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();
            MockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            MockApprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            MockUserProfileService = new Mock<IUserProfileService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();

            MockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());

            MockTimeService = new Mock<IDateTimeService>();
            ApprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyProvider(MockLogService.Object,
                MockConfigurationService.Object,
                MockVacancyPostingService.Object,
                MockReferenceDataService.Object,
                MockProviderService.Object,
                MockTimeService.Object,
                MockMapper.Object,
                ApprenticeshipApplicationService.Object,
                MockUserProfileService.Object);
        }
    }
}