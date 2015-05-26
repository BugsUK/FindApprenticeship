﻿namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;

    public class SetPendingDeletionStrategy : HousekeepingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ILogService _logService;

        public SetPendingDeletionStrategy(IConfigurationService configurationService, IUserWriteRepository userWriteRepository, ILogService logService)
            : base(configurationService)
        {
            _userWriteRepository = userWriteRepository;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (candidate == null)
            {
                _logService.Warn("Setting user status to PendingDeletion for user with Id: {0} as it is associated with a null candidate", user.EntityId);
                return SetUserStatusPendingDeletion(user);
            }

            if (user.Status != UserStatuses.PendingActivation) return false;

            var housekeepingCyclesSinceCreation = GetHousekeepingCyclesSince(user.DateCreated);

            if (housekeepingCyclesSinceCreation >= Configuration.ActivationReminderStrategy.SetPendingDeletionAfterCycles)
            {
                return SetUserStatusPendingDeletion(user);
            }

            return false;
        }

        private bool SetUserStatusPendingDeletion(User user)
        {
            _logService.Info("Setting User: {0} Status to PendingDeletion", user.EntityId);

            user.Status = UserStatuses.PendingDeletion;
            _userWriteRepository.Save(user);

            _logService.Info("Set User: {0} Status to PendingDeletion", user.EntityId);

            return true;
        }
    }
}