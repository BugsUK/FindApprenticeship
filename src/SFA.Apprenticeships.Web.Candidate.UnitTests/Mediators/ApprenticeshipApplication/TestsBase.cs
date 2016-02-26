namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Common.Validators;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected Mock<IApprenticeshipApplicationProvider> ApprenticeshipApplicationProvider;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected IApprenticeshipApplicationMediator Mediator;

        [SetUp]
        public void Setup()
        {
            ApprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();

            ConfigurationService = new Mock<IConfigurationService>();

            ConfigurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration
                {
                    VacancyResultsPerPage = 5
                });
            
            UserDataProvider = new Mock<IUserDataProvider>();

            Mediator = new ApprenticeshipApplicationMediator(
                ConfigurationService.Object,
                UserDataProvider.Object,
                ApprenticeshipApplicationProvider.Object,
                new ApprenticeshipApplicationViewModelServerValidator(),
                new ApprenticeshipApplicationViewModelSaveValidator(),
                new ApprenticeshipApplicationPreviewViewModelValidator());
        }
    }
}
