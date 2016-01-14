namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Users;

    public class QueuedLegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IServiceBus _serviceBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserAccountService _registrationService;
        private readonly ILogService _logService;

        public QueuedLegacyActivateCandidateStrategy(
            ILogService logService,
            IServiceBus serviceBus,
            IUserReadRepository userReadRepository,
            IUserAccountService registrationService)
        {
            _logService = logService;
            _serviceBus = serviceBus;
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
        }

        public void ActivateCandidate(Guid id, string activationCode)
        {
            var user = _userReadRepository.Get(id);

            user.AssertState("Activate candidate", UserStatuses.PendingActivation);

            // Activate user before message submission so that they can continue using the site
            _registrationService.Activate(id, activationCode);
            
            // queue request for submission to legacy
            var message = new CreateCandidateRequest
            {
                CandidateId = user.EntityId
            };

            _logService.Info("Publishing CreateCandidateRequest for Candidate with Id: {0}", message.CandidateId);

            _serviceBus.PublishMessage(message);

            _logService.Info("Successfully published CreateCandidateRequest for Candidate with Id: {0}", message.CandidateId);
        }
    }
}