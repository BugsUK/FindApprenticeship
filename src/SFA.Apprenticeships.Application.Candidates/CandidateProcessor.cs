namespace SFA.Apprenticeships.Application.Candidates
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;

    public class CandidateProcessor : ICandidateProcessor
    {
        private readonly ILogService _logService;
        private readonly IServiceBus _serviceBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IConfigurationService _configurationService;

        public CandidateProcessor(
            ILogService logService,
            IConfigurationService configurationService,
            IServiceBus serviceBus,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _logService = logService;
            _configurationService = configurationService;
            _serviceBus = serviceBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public void QueueCandidates()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var candidateIds =
                GetUsersPendingActivationOrDeletion()
                .Union(GetPotentiallyDormantUsers())
                .Union(GetDormantUsersPotentiallyEligibleForSoftDelete())
                .Union(GetCandidatesPendingMobileVerification());

            var message = string.Format("Querying candidates for housekeeping took {0}", stopwatch.Elapsed);

            var count = _serviceBus.PublishMessages(candidateIds.Select(cid => new CandidateHousekeeping {CandidateId = cid}));

            stopwatch.Stop();

            message += string.Format(". Queuing {0} candidates for housekeeping took {1}", count, stopwatch.Elapsed);

            if (stopwatch.ElapsedMilliseconds > 60000 * 5)
            {
                _logService.Warn(message);
            }
            else
            {
                _logService.Info(message);
            }
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
            return _candidateReadRepository.GetCandidatesWithPendingMobileVerification();
        }
    }
}