﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected static ITraineeshipSearchMediator GetMediator(IConfigurationService configurationService, ISearchProvider searchProvider, IUserDataProvider userDataProvider, ITraineeshipVacancyProvider traineeshipVacancyProvider, ICandidateServiceProvider candidateServiceProvider)
        {
            return new TraineeshipSearchMediator(configurationService, searchProvider, userDataProvider, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator(), traineeshipVacancyProvider, candidateServiceProvider);
        }

        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<ITraineeshipVacancyProvider> TraineeshipVacancyProvider;
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected ITraineeshipSearchMediator Mediator;

        [SetUp]
        public virtual void Setup()
        {
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(cm => cm.Get<WebConfiguration>())
                .Returns(new WebConfiguration() { VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99" });
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            TraineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            Mediator = new TraineeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, UserDataProvider.Object, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator(), TraineeshipVacancyProvider.Object, CandidateServiceProvider.Object);
        }
    }
}