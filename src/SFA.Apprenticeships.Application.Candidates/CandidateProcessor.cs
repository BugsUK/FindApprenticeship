namespace SFA.Apprenticeships.Application.Candidates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
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
        private readonly IConfigurationService _configurationService;

        public CandidateProcessor(
            ILogService logService,
            IMessageBus messageBus,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository,
            IConfigurationService configurationService)
        {
            _logService = logService;
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _configurationService = configurationService;
        }

        public void QueueCandidates()
        {
            var candidateIds =
                GetUsersPendingActivationOrDeletion()
                .Union(GetPotentiallyDormantUsers())
                .Union(GetDormantUsersPotentiallyEligibleForSoftDelete())
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

        private IEnumerable<Guid> GetUsersPendingActivationOrDeletion()
        {
            var userStatuses = new[] { UserStatuses.PendingActivation, UserStatuses.PendingDeletion };

            return _userReadRepository.GetUsersWithStatus(userStatuses);
        }

        private IEnumerable<Guid> GetPotentiallyDormantUsers()
        {
            var configuration = _configurationService.Get<HousekeepingConfiguration>();
            var lastValidLoginHours = configuration.DormantAccountStrategy.SendReminderAfterCycles*
                                      configuration.HousekeepingCycleInHours;
            var lastValidLogin = DateTime.UtcNow.AddHours(-lastValidLoginHours);

            return _userReadRepository.GetPotentiallyDormantUsers(lastValidLogin);
        }

        private IEnumerable<Guid> GetDormantUsersPotentiallyEligibleForSoftDelete()
        {
            var configuration = _configurationService.Get<HousekeepingConfiguration>();
            var potentiallyDormantHours = configuration.DormantAccountStrategy.SendFinalReminderAfterCycles*
                                          configuration.HousekeepingCycleInHours;
            var dormantAfterDateTime = DateTime.UtcNow.AddHours(-potentiallyDormantHours);

            return _userReadRepository.GetDormantUsersPotentiallyEligibleForSoftDelete(dormantAfterDateTime);
        }

        private IEnumerable<Guid> GetCandidatesPendingMobileVerification()
        {
            return _candidateReadRepository.GetCandidatesWithPendingMobileVerification()
                .Select(each => each.EntityId);
        }
    }
}