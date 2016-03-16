namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Candidates.Strategies;
    using Apprenticeships.Application.Candidates.Strategies.DormantAccount;
    using Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class SendAccountRemindersStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserWriteRepository> _userWriteRepository; 
        private Mock<ICandidateWriteRepository> _candidateWriteRepository;
        private Mock<ICommunicationService> _communicationService;
        private readonly Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public SendAccountRemindersStrategyBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _userWriteRepository = new Mock<IUserWriteRepository>();
            _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            _communicationService = new Mock<ICommunicationService>();
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SendAccountRemindersStrategy Build()
        {
            var strategy = new SendAccountRemindersStrategy(_configurationService.Object, _userWriteRepository.Object, _candidateWriteRepository.Object, _auditRepository.Object, _communicationService.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public SendAccountRemindersStrategyBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }

        public SendAccountRemindersStrategyBuilder With(Mock<IUserWriteRepository> userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            return this;
        }

        public SendAccountRemindersStrategyBuilder With(Mock<ICandidateWriteRepository> candidateWriteRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            return this;
        }

        public SendAccountRemindersStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }
    }
}