namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;

    public class HardDeleteStrategy : HousekeepingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogService _logService;

        public HardDeleteStrategy(IConfigurationService configurationService, IUserWriteRepository userWriteRepository, ICandidateWriteRepository candidateWriteRepository, IAuditRepository auditRepository, ILogService logService)
            : base(configurationService)
        {
            _userWriteRepository = userWriteRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _auditRepository = auditRepository;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user.Status != UserStatuses.PendingDeletion) return false;

            if (!user.DateUpdated.HasValue) return false;

            var housekeepingCyclesSinceDateUpdated = GetHousekeepingCyclesSince(user.DateUpdated.Value);

            if(housekeepingCyclesSinceDateUpdated < Configuration.HardDeleteAccountAfterCycles) return false;

            var candidateUser = new {User = user, Candidate = candidate};

            _auditRepository.Audit(candidateUser, AuditEventTypes.HardDeleteCandidateUser, user.EntityId);

            //This isn't transactional however the code can cope with a missing candidate and so will self heal over time in the unlikely event of partial failure
            if (candidate != null)
            {
                _logService.Info("Hard deleting Candidate: {0}", candidate.EntityId);

                _candidateWriteRepository.Delete(candidate.EntityId);

                _logService.Info("Hard deleted Candidate: {0}", candidate.EntityId);
            }

            _logService.Info("Hard deleting User: {0}", user.EntityId);

            _userWriteRepository.Delete(user.EntityId);

            _logService.Info("Hard deleted User: {0}", user.EntityId);

            return true;
        }
    }
}