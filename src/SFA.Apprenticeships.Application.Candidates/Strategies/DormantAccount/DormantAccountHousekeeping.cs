namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;

    using SFA.Apprenticeships.Application.Interfaces;

    public class DormantAccountHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public DormantAccountHousekeeping(IConfigurationService configurationService, ICommunicationService communicationService, IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, ICandidateWriteRepository candidateWriteRepository, IAuditRepository auditRepository, ILogService logService)
        {
            var sendAccountRemindersStrategy = new SendAccountRemindersStrategy(configurationService, userWriteRepository, candidateWriteRepository, auditRepository, communicationService, logService);
            var setPendingDeletionStrategy = new SetPendingDeletionStrategy(configurationService, userReadRepository, userWriteRepository, auditRepository, logService);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            sendAccountRemindersStrategy.SetSuccessor(setPendingDeletionStrategy);
            setPendingDeletionStrategy.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = sendAccountRemindersStrategy;
        }

        public int Order
        {
            get { return 2; }
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}