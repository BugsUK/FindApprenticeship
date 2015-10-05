namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Moq;
    using NUnit.Framework;
    using Recruit.Providers;

    public abstract class TestBase
    {
        protected Mock<ILogService> MockLogService;
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<IMapper> MockMapper;

        protected Mock<IVacancyPostingService> MockVacancyPostingService;
        protected Mock<IUserProfileService> MockUserProfileService;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IReferenceDataService> MockReferenceDataService;

        [SetUp]
        public void SetUpBase()
        {
            MockLogService = new Mock<ILogService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();

            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockUserProfileService = new Mock<IUserProfileService>();
            MockProviderService = new Mock<IProviderService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();
        }

        protected IVacancyPostingProvider GetProvider()
        {
            return new VacancyPostingProvider(
                MockLogService.Object,
                MockConfigurationService.Object,
                MockMapper.Object,
                MockVacancyPostingService.Object,
                MockUserProfileService.Object,
                MockProviderService.Object,
                MockReferenceDataService.Object);
        }
    }
}
