using Moq;
using NUnit.Framework;


namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.ProviderProvider
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Domain.Interfaces.Configuration;
    using Recruit.Providers;

    public class TestBase
    {
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUp()
        {
            MockProviderService = new Mock<IProviderService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
        }

        public IProviderProvider GetProvider()
        {
            return new ProviderProvider(MockProviderService.Object, MockConfigurationService.Object,
                MockVacancyPostingService.Object);
        }
    }
}
