namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using UserAccount.Entities;

    public class SetUserStatusPendingDeletionStrategy : ISetUserStatusPendingDeletionStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogService _logService;
        private readonly IServiceBus _serviceBus;

        public SetUserStatusPendingDeletionStrategy(IUserWriteRepository userWriteRepository,
            IAuditRepository auditRepository,
            ILogService logService, IServiceBus serviceBus)
        {
            _userWriteRepository = userWriteRepository;
            _auditRepository = auditRepository;
            _logService = logService;
            _serviceBus = serviceBus;
        }

        public bool SetUserStatusPendingDeletion(User user)
        {
            _logService.Info("Setting User: {0} Status to PendingDeletion", user.EntityId);

            _auditRepository.Audit(user, AuditEventTypes.UserSoftDelete, user.EntityId);

            user.Status = UserStatuses.PendingDeletion;
            _userWriteRepository.SoftDelete(user);
            _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Delete));

            _logService.Info("Set User: {0} Status to PendingDeletion", user.EntityId);

            return true;
        }
    }
}