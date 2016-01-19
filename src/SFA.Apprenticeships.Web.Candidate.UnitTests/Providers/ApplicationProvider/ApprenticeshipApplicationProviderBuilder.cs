namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Common.Configuration;
    using Common.Providers;
    using Moq;

    internal class ApprenticeshipApplicationProviderBuilder
    {
        private Mock<IApprenticeshipVacancyProvider> _apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
        private Mock<ICandidateService> _candidateService = new Mock<ICandidateService>();
        private Mock<IConfigurationService> _configurationService;
        private readonly Mock<ILogService> _logService;
        private Mock<IUserDataProvider> _userDataProvider;
        private readonly Mock<IReferenceDataService> _referenceDataService;

        public ApprenticeshipApplicationProviderBuilder()
        {
            _logService = new Mock<ILogService>();
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());
            _userDataProvider = new Mock<IUserDataProvider>();
            _referenceDataService = new Mock<IReferenceDataService>();
        }

        public ApprenticeshipApplicationProviderBuilder With(Mock<ICandidateService> candidateServiceMock)
        {
            _candidateService = candidateServiceMock;
            return this;
        }

        public ApprenticeshipApplicationProviderBuilder With(Mock<IApprenticeshipVacancyProvider> apprenticeshipVacancyProvider)
        {
            _apprenticeshipVacancyProvider = apprenticeshipVacancyProvider;
            return this;
        }

        public ApprenticeshipApplicationProviderBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public ApprenticeshipApplicationProviderBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }

        public ApprenticeshipApplicationProvider Build()
        {
            return new ApprenticeshipApplicationProvider(_apprenticeshipVacancyProvider.Object, _candidateService.Object,
                new ApprenticeshipCandidateWebMappers(), _configurationService.Object, _logService.Object,
                _userDataProvider.Object, _referenceDataService.Object);
        }
    }
}
