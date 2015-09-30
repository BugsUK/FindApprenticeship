namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;
    using Recruit.Providers;

    public abstract class TestBase
    {
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<IUserProfileService> MockUserProfileService;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IReferenceDataService> MockReferenceDataService;

        [SetUp]
        public void SetUpBase()
        {
            MockConfigurationService = new Mock<IConfigurationService>();
            MockUserProfileService = new Mock<IUserProfileService>();
            MockProviderService = new Mock<IProviderService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();
        }

        protected IVacancyPostingProvider GetProvider()
        {
            return new VacancyPostingProvider(
                MockConfigurationService.Object,
                MockUserProfileService.Object,
                MockProviderService.Object,
                MockReferenceDataService.Object);
        }
    }
}
