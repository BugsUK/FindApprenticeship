﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Application.Interfaces.ReferenceData;
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
        protected static IApprenticeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, ICandidateServiceProvider candidateServiceProvider, IUserDataProvider userDataProvider, IReferenceDataService referenceDataService, IApprenticeshipVacancyProvider apprenticeshipVacancyProvider)
        protected static IApprenticeshipSearchMediator GetMediator(IConfigurationService configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, ICandidateServiceProvider candidateServiceProvider, IUserDataProvider userDataProvider, IReferenceDataService referenceDataService)
        {
            return new ApprenticeshipSearchMediator(configurationManager, searchProvider, apprenticeshipVacancyDetailProvider, candidateServiceProvider, userDataProvider, referenceDataService, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), apprenticeshipVacancyProvider);
        }

        protected Mock<IApprenticeshipVacancyDetailProvider> ApprenticeshipVacancyDetailProvider;
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<IReferenceDataService> ReferenceDataService;
        protected Mock<IApprenticeshipVacancyProvider> ApprenticeshipVacancyProvider;
        protected IApprenticeshipSearchMediator Mediator;

        [SetUp]
        public virtual void Setup()
        {
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            ApprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(cm => cm.Get<WebConfiguration>(WebConfiguration.WebConfigurationName))
                .Returns(new WebConfiguration() {VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99"});
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            ReferenceDataService = new Mock<IReferenceDataService>();
            ApprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            Mediator = new ApprenticeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, ApprenticeshipVacancyDetailProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator());
            Mediator = new ApprenticeshipSearchMediator(ConfigurationManager.Object, SearchProvider.Object, ApprenticeshipVacancyDetailProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), ApprenticeshipVacancyProvider.Object);
        }
    }
}
