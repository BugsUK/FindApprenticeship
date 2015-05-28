namespace SFA.Apprenticeships.Application.Candidates.Strategies.ActivationReminder
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public class ActivationReminderHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public ActivationReminderHousekeeping(IConfigurationService configurationService, ICommunicationService communicationService, IUserWriteRepository userWriteRepository, IAuditRepository auditRepository, ILogService logService)
        {
            var sendAccountRemindersStrategyA = new SendAccountRemindersStrategyA(configurationService, communicationService, logService);
            var sendAccountRemindersStrategyB = new SendAccountRemindersStrategyB(configurationService, communicationService, logService);
            var setPendingDeletionStrategy = new SetPendingDeletionStrategy(configurationService, userWriteRepository, auditRepository, logService);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            sendAccountRemindersStrategyA.SetSuccessor(sendAccountRemindersStrategyB);
            sendAccountRemindersStrategyB.SetSuccessor(setPendingDeletionStrategy);
            setPendingDeletionStrategy.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = sendAccountRemindersStrategyA;
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}