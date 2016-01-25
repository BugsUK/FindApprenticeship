namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class SetPendingDeletionStrategy : HousekeepingStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogService _logService;

        public SetPendingDeletionStrategy(IConfigurationService configurationService,
            IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository,
            IAuditRepository auditRepository, ILogService logService)
            : base(configurationService)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _auditRepository = auditRepository;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user == null || candidate == null) return false;

            if (user.Status != UserStatuses.Dormant) return false;

            var housekeepingCyclesSinceLastLogin = GetHousekeepingCyclesSince(user.GetLastLogin());

            if (housekeepingCyclesSinceLastLogin >= Configuration.DormantAccountStrategy.SetPendingDeletionAfterCycles)
            {
                var existingUser = _userReadRepository.Get(user.Username, false);

                if (existingUser == null || !existingUser.IsInState(UserStatuses.PendingDeletion))
                {
                    return SetUserStatusPendingDeletion(user);
                }
            }

            return false;
        }

        private bool SetUserStatusPendingDeletion(User user)
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