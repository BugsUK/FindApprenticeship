namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Web.Configuration;
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
            ConfigurationService.Setup(x => x.Get<WebConfiguration>(WebConfiguration.WebConfigurationName))
                .Returns(new WebConfiguration() { VacancyResultsPerPage = 5 });
            UserDataProvider = new Mock<IUserDataProvider>();
            Mediator = new ApprenticeshipApplicationMediator(ApprenticeshipApplicationProvider.Object, new ApprenticeshipApplicationViewModelServerValidator(), new ApprenticeshipApplicationViewModelSaveValidator(), ConfigurationService.Object, UserDataProvider.Object);
        }
    }
}
