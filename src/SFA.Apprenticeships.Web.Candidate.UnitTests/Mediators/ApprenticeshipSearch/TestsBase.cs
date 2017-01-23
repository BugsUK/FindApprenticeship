namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Application.Interfaces.ReferenceData;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class TestsBase
    {
        protected static IApprenticeshipSearchMediator GetMediator(IConfigurationService configurationService, ISearchProvider searchProvider, ICandidateServiceProvider candidateServiceProvider, IUserDataProvider userDataProvider, IReferenceDataService referenceDataService, IApprenticeshipVacancyProvider apprenticeshipVacancyProvider)
        {
            return new ApprenticeshipSearchMediator(configurationService, searchProvider, candidateServiceProvider, userDataProvider, referenceDataService, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), apprenticeshipVacancyProvider, new Mock<IGoogleMapsProvider>().Object);
        }

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
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(cm => cm.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration() {VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99"});
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            ReferenceDataService = new Mock<IReferenceDataService>();
            ApprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            Mediator = new ApprenticeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), ApprenticeshipVacancyProvider.Object, new Mock<IGoogleMapsProvider>().Object);
        }
    }
}
