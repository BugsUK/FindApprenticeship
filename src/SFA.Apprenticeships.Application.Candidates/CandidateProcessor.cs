namespace SFA.Apprenticeships.Application.Candidates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces.Logging;

    public class CandidateProcessor : ICandidateProcessor
    {
        private readonly ILogService _logService;
        private readonly IMessageBus _messageBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CandidateProcessor(
            ILogService logService,
            IMessageBus messageBus,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _logService = logService;
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public void QueueCandidates()
        {
            var candidateIds =
                GetCandidatesPendingActivationOrDeletion()
                .Union(GetCandidatesPendingMobileVerification());

            var counter = 0;

            Parallel.ForEach(candidateIds, candidateId =>
            {
                var candidateHousekeeping = new CandidateHousekeeping
                {
                    CandidateId = candidateId
                };

                _messageBus.PublishMessage(candidateHousekeeping);
                Interlocked.Increment(ref counter);
            });

            _logService.Debug("Queued {0} candidates for Housekeeping", counter);
        }

        private IEnumerable<Guid> GetCandidatesPendingActivationOrDeletion()
        {
            var userStatuses = new[] { UserStatuses.PendingActivation, UserStatuses.PendingDeletion };

            return _userReadRepository.GetUsersWithStatus(userStatuses)
                .Select(each => each.EntityId);
        }

        private IEnumerable<Guid> GetCandidatesPendingMobileVerification()
        {
            return _candidateReadRepository.GetCandidatesWithPendingMobileVerification()
                .Select(each => each.EntityId);
        }
    }
}