namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;

    public abstract class TestBase
    {
        protected Mock<IConfigurationService> MockConfigurationService;
        protected Mock<ILogService> MockLogService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IReferenceDataService> MockReferenceDataService;
        protected Mock<IDateTimeService> MockTimeService;

        protected Mock<IVacancyPostingService> MockVacancyPostingService;

        [SetUp]
        public void SetUpBase()
        {
            MockLogService = new Mock<ILogService>();
            MockConfigurationService = new Mock<IConfigurationService>();
            MockMapper = new Mock<IMapper>();

            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();

            MockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration());

            MockTimeService = new Mock<IDateTimeService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyPostingProvider(
                MockLogService.Object,
                MockConfigurationService.Object,
                MockVacancyPostingService.Object,
                MockReferenceDataService.Object,
                MockProviderService.Object,
                MockTimeService.Object);
        }
    }
}