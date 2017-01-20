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

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApprenticeshipSearchMediatorBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;
        private readonly Mock<ISearchProvider> _searchProvider;
        private Mock<ICandidateServiceProvider> _candidateServiceProvider;
        private readonly Mock<IUserDataProvider> _userDataProvider;
        private readonly Mock<IReferenceDataService> _referenceDataService;
        private readonly Mock<IApprenticeshipVacancyProvider> _apprenticeshipVacancyProvider;

        public ApprenticeshipSearchMediatorBuilder()
        {
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration() { VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99" });
            _userDataProvider = new Mock<IUserDataProvider>();
            _searchProvider = new Mock<ISearchProvider>();
            _referenceDataService = new Mock<IReferenceDataService>();
            _apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
        }

        public ApprenticeshipSearchMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            return this;
        }

        public IApprenticeshipSearchMediator Build()
        {
            var mediator = new ApprenticeshipSearchMediator(_configurationService.Object, _searchProvider.Object,
                _candidateServiceProvider.Object, _userDataProvider.Object,
                _referenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(),
                new ApprenticeshipSearchViewModelLocationValidator(), _apprenticeshipVacancyProvider.Object, new Mock<IGoogleMapsProvider>().Object);
            return mediator;
        }
    }
}