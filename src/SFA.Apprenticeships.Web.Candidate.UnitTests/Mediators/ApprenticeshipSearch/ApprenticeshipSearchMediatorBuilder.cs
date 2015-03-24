namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Application.Interfaces.ReferenceData;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class ApprenticeshipSearchMediatorBuilder
    {
        private readonly Mock<IConfigurationManager> _configurationManager;
        private readonly Mock<ISearchProvider> _searchProvider;
        private readonly Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ICandidateServiceProvider> _candidateServiceProvider;
        private readonly Mock<IUserDataProvider> _userDataProvider;
        private readonly Mock<IReferenceDataService> _referenceDataService;
        private readonly Mock<IApprenticeshipVacancyProvider> _apprenticeshipVacancyProvider;

        public ApprenticeshipSearchMediatorBuilder()
        {
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _configurationManager = new Mock<IConfigurationManager>();
            _configurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);
            _configurationManager.Setup(cm => cm.GetAppSetting("BlacklistedCategoryCodes")).Returns("00,99");
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
            var mediator = new ApprenticeshipSearchMediator(_configurationManager.Object, _searchProvider.Object, _apprenticeshipVacancyDetailProvider.Object, _candidateServiceProvider.Object, _userDataProvider.Object, _referenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), _apprenticeshipVacancyProvider.Object);
            return mediator;
        }
    }
}