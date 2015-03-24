namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;

    internal class ApprenticeshipApplicationProviderBuilder
    {
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider =
            new Mock<IApprenticeshipVacancyDetailProvider>();

        private Mock<ICandidateService> _candidateService = new Mock<ICandidateService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        public ApprenticeshipApplicationProviderBuilder With(Mock<ICandidateService> candidateServiceMock)
        {
            _candidateService = candidateServiceMock;
            return this;
        }

        public ApprenticeshipApplicationProviderBuilder With(
            Mock<IApprenticeshipVacancyDetailProvider> apprenticeshipVacancyDetailProviderMock)
        {
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProviderMock;
            return this;
        }

        public ApprenticeshipApplicationProvider Build()
        {
            return new ApprenticeshipApplicationProvider(_apprenticeshipVacancyDetailProvider.Object,
                _candidateService.Object, new ApprenticeshipCandidateWebMappers(), _configurationService.Object,
                _logService.Object);
        }
    }
}
