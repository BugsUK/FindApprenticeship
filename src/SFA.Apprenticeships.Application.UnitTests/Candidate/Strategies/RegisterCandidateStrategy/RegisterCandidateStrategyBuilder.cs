namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.RegisterCandidateStrategy
{
    using Apprenticeships.Application.Candidate.Strategies;
    using Apprenticeships.Application.UserAccount.Configuration;
    using Domain.Entities.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;

    public class RegisterCandidateStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IUserAccountService> _userAccountService = new Mock<IUserAccountService>();
        private Mock<IAuthenticationService> _authenticationService = new Mock<IAuthenticationService>();
        private Mock<ICandidateWriteRepository> _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
        private Mock<ICodeGenerator> _codeGenerator = new Mock<ICodeGenerator>();
        private Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();

        public RegisterCandidateStrategyBuilder()
        {
            _configurationService.Setup(s => s.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration {ActivationCodeExpiryDays = 30});
            _candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Returns<Candidate>(c => c);
        }

        public IRegisterCandidateStrategy Build()
        {
            var strategy = new RegisterCandidateStrategy(_configurationService.Object, _userAccountService.Object,
                _authenticationService.Object, _candidateWriteRepository.Object, _communicationService.Object,
                _codeGenerator.Object, _userReadRepository.Object);
            return strategy;
        }

        public RegisterCandidateStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }

        public RegisterCandidateStrategyBuilder With(Mock<ICandidateWriteRepository> candidateWriteRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            return this;
        }

        public RegisterCandidateStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public RegisterCandidateStrategyBuilder With(Mock<IAuthenticationService> authenticationService)
        {
            _authenticationService = authenticationService;
            return this;
        }

        public RegisterCandidateStrategyBuilder With(Mock<IUserAccountService> userAccountService)
        {
            _userAccountService = userAccountService;
            return this;
        }

        public RegisterCandidateStrategyBuilder With(Mock<ICodeGenerator> codeGenerator)
        {
            _codeGenerator = codeGenerator;
            return this;
        }
    }
}