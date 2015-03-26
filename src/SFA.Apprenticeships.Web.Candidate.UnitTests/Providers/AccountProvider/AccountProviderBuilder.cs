namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class AccountProviderBuilder
    {
        private Mock<ICandidateService> _candidateService;
        private readonly Mock<ILogService> _logger;
        private readonly Mock<IConfigurationService> _configurationService;
        
        public AccountProviderBuilder()
        {
            _candidateService = new Mock<ICandidateService>();
            _logger = new Mock<ILogService>();
            _configurationService = new Mock<IConfigurationService>();
        }

        public AccountProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public AccountProvider Build()
        {
            _configurationService.Setup(x => x.Get<WebConfiguration>(WebConfiguration.ConfigurationName))
                .Returns(new WebConfiguration(){Features = new Features()});
            var provider = new AccountProvider(_candidateService.Object, new ApprenticeshipCandidateWebMappers(), _logger.Object, _configurationService.Object);
            return provider;
        }
    }
}