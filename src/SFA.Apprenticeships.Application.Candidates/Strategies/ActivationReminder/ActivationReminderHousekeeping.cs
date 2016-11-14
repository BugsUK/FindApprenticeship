namespace SFA.Apprenticeships.Application.Candidates.Strategies.ActivationReminder
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces;

    public class ActivationReminderHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public ActivationReminderHousekeeping(IConfigurationService configurationService, ICommunicationService communicationService, IUserWriteRepository userWriteRepository, IAuditRepository auditRepository, ILogService logService, IServiceBus serviceBus)
        {
            var sendAccountRemindersStrategy = new SendAccountRemindersStrategy(configurationService, communicationService);
            var setPendingDeletionStrategy = new SetPendingDeletionStrategy(configurationService, userWriteRepository, auditRepository, logService, serviceBus);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            sendAccountRemindersStrategy.SetSuccessor(setPendingDeletionStrategy);
            setPendingDeletionStrategy.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = sendAccountRemindersStrategy;
        }

        public int Order
        {
            get { return 1; }
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}