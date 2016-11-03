namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces;

    public class HardDeleteHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public HardDeleteHousekeeping(IConfigurationService configurationService,
            IUserWriteRepository userWriteRepository, IAuthenticationRepository authenticationRepository, ICandidateWriteRepository candidateWriteRepository,
            ISavedSearchReadRepository savedSearchReadRepository,
            ISavedSearchWriteRepository savedSearchWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            IAuditRepository auditRepository, ILogService logService, IServiceBus serviceBus)
        {
            var hardDeleteStrategy = new HardDeleteStrategy(configurationService, userWriteRepository, authenticationRepository,
                candidateWriteRepository, savedSearchReadRepository, savedSearchWriteRepository, apprenticeshipApplicationReadRepository,
                apprenticeshipApplicationWriteRepository, traineeshipApplicationReadRepository,
                traineeshipApplicationWriteRepository, auditRepository, logService, serviceBus);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            hardDeleteStrategy.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = hardDeleteStrategy;
        }

        public int Order
        {
            get { return 100; }
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}