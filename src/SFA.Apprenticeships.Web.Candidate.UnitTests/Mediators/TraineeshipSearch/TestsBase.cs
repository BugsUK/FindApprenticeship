namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Web.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected static ITraineeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider, ITraineeshipVacancyProvider traineeshipVacancyProvider)
        protected static ITraineeshipSearchMediator GetMediator(IConfigurationService configurationService, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            return new TraineeshipSearchMediator(configurationManager, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator(), traineeshipVacancyProvider);
            return new TraineeshipSearchMediator(configurationService, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator());
        }

        protected Mock<ITraineeshipVacancyDetailProvider> TraineeshipVacancyDetailProvider;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<ITraineeshipVacancyProvider> TraineeshipVacancyProvider;
        protected ITraineeshipSearchMediator Mediator;

        [SetUp]
        public virtual void Setup()
        {
            TraineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(cm => cm.Get<WebConfiguration>(WebConfiguration.WebConfigurationName))
                .Returns(new WebConfiguration() { VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99" });
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            TraineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            Mediator = new TraineeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, TraineeshipVacancyDetailProvider.Object, UserDataProvider.Object, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator());
            Mediator = new TraineeshipSearchMediator(ConfigurationManager.Object, SearchProvider.Object, TraineeshipVacancyDetailProvider.Object, UserDataProvider.Object, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator(), TraineeshipVacancyProvider.Object);
        }
    }
}