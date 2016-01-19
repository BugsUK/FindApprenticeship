namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Application.Interfaces.Applications;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;

    public abstract class TestBase
    {
        protected Mock<IConfigurationService> MockConfigurationService;
        private Mock<ILogService> _mockLogService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        private Mock<IUserProfileService> _mockUserProfileService;
        protected Mock<IReferenceDataService> MockReferenceDataService;
        protected Mock<IDateTimeService> MockTimeService;
        private Mock<IApprenticeshipApplicationService> _apprenticeshipApplicationService;

        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUpBase()
        {
            _mockLogService = new Mock<ILogService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            _mockUserProfileService = new Mock<IUserProfileService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();

            MockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());

            MockTimeService = new Mock<IDateTimeService>();
            _apprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyProvider(_mockLogService.Object,
                MockConfigurationService.Object,
                MockVacancyPostingService.Object,
                MockReferenceDataService.Object,
                MockProviderService.Object,
                MockTimeService.Object,
                MockMapper.Object,
                _apprenticeshipApplicationService.Object,
                _mockUserProfileService.Object);
        }
    }
}