namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Users;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Common.Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class AccountProviderBuilder
    {
        private Mock<ICandidateService> _candidateService;
        private readonly Mock<ILogService> _logger;
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserAccountService> _userAccountService;
        
        public AccountProviderBuilder()
        {
            _candidateService = new Mock<ICandidateService>();
            _logger = new Mock<ILogService>();
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration { Features = new Features(), SubCategoriesFullNamesLimit = 5 });
            _userAccountService = new Mock<IUserAccountService>();
            _userAccountService.Setup(x => x.GetUser(It.IsAny<Guid>())).Returns(new User());
        }

        public AccountProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public AccountProviderBuilder With(Mock<IUserAccountService> userAccountService)
        {
            _userAccountService = userAccountService;
            return this;
        }

        public AccountProvider Build()
        {
            var provider = new AccountProvider(_candidateService.Object, _userAccountService.Object, new ApprenticeshipCandidateWebMappers(), _logger.Object, _configurationService.Object);
            return provider;
        }
    }
}