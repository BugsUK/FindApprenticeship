namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    using Interfaces;

    public class SetPendingDeletionStrategy : HousekeepingStrategy
    {
        private readonly ISetUserStatusPendingDeletionStrategy _setUserStatusPendingDeletionStrategy;
        private readonly IUserReadRepository _userReadRepository;

        public SetPendingDeletionStrategy(IConfigurationService configurationService,
            IUserReadRepository userReadRepository, ISetUserStatusPendingDeletionStrategy setUserStatusPendingDeletionStrategy)
            : base(configurationService)
        {
            _userReadRepository = userReadRepository;
            _setUserStatusPendingDeletionStrategy = setUserStatusPendingDeletionStrategy;
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
                    return _setUserStatusPendingDeletionStrategy.SetUserStatusPendingDeletion(user);
                }
            }

            return false;
        }
    }
}