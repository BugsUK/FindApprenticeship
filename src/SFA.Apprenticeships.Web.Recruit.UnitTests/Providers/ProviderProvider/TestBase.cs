using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Web.Raa.Common.Providers;


namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.ProviderProvider
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using SFA.Infrastructure.Interfaces;

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
            return new Raa.Common.Providers.ProviderProvider(MockProviderService.Object, MockConfigurationService.Object,
                MockVacancyPostingService.Object);
        }
    }
}
