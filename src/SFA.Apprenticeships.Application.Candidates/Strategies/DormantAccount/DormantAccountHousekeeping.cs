namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public class DormantAccountHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public DormantAccountHousekeeping(IConfigurationService configurationService, ICommunicationService communicationService, IUserWriteRepository userWriteRepository, ICandidateWriteRepository candidateWriteRepository, ILogService logService)
        {
            var sendAccountRemindersStrategy = new SendAccountRemindersStrategy(configurationService, userWriteRepository, candidateWriteRepository, communicationService, logService);
            var setPendingDeletionStrategy = new SetPendingDeletionStrategy(configurationService, userWriteRepository, logService);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            sendAccountRemindersStrategy.SetSuccessor(setPendingDeletionStrategy);
            setPendingDeletionStrategy.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = sendAccountRemindersStrategy;
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}