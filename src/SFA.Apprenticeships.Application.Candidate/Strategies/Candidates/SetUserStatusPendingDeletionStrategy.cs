namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces;

    public class SetCandidateDeletionPendingStrategy : ISetCandidateDeletionPendingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogService _logService;

        public SetCandidateDeletionPendingStrategy(IUserWriteRepository userWriteRepository,
            IAuditRepository auditRepository,
            ILogService logService)
        {
            _userWriteRepository = userWriteRepository;
            _auditRepository = auditRepository;
            _logService = logService;
        }

        public bool SetUserStatusPendingDeletion(User user)
        {
            _logService.Info("Setting User: {0} Status to PendingDeletion", user.EntityId);

            _auditRepository.Audit(user, AuditEventTypes.UserSoftDelete, user.EntityId);

            user.Status = UserStatuses.PendingDeletion;
            _userWriteRepository.Save(user);

            _logService.Info("Set User: {0} Status to PendingDeletion", user.EntityId);

            return true;
        }
    }
}