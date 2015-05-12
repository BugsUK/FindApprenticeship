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
            if (user.Status == UserStatuses.PendingDeletion)
            {
                _logService.Info("Hard deleting User: {0}", user.EntityId);

                _auditRepository.Audit(user, AuditEventTypes.HardDeleteUser);

                _userWriteRepository.Delete(user.EntityId);

                _logService.Info("Hard deleted User: {0}", user.EntityId);

                if (candidate != null)
                {
                    _logService.Info("Hard deleting Candidate: {0}", candidate.EntityId);

                    _auditRepository.Audit(candidate, AuditEventTypes.HardDeleteCandidate);

                    _candidateWriteRepository.Delete(candidate.EntityId);

                    _logService.Info("Hard deleted Candidate: {0}", candidate.EntityId);
                }

                return true;
            }

            return false;
        }
    }
}