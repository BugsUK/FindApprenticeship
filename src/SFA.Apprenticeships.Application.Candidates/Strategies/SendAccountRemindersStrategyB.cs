namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategyB : HousekeepingStrategy
    {
        private readonly ILogService _logService;

        public SendAccountRemindersStrategyB(IConfigurationService configurationService, ILogService logService)
            : base(configurationService)
        {
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if(user.Status != UserStatuses.PendingActivation) return false;

            var timeSinceCreation = DateTime.Now - user.DateCreated;

            var housekeepingCyclesSinceCreation = (int)(timeSinceCreation.TotalHours / Configuration.HousekeepingCycleInHours);

            //Only remind if enough time has passed and not due for deletion
            if (housekeepingCyclesSinceCreation < Configuration.SendAccountReminderAfterCycles 
                || housekeepingCyclesSinceCreation >= Configuration.SetPendingDeletionAfterCycles)
            {
                return false;
            }

            //Remind on the first cycle
            if (housekeepingCyclesSinceCreation == Configuration.SendAccountReminderAfterCycles)
            {
                return true;
            }

            //Then every X cycles after that
            if ((housekeepingCyclesSinceCreation - Configuration.SendAccountReminderAfterCycles)%
                Configuration.SendAccountReminderEveryCycles == 0)
            {
                return true;
            }

            return false;
        }
    }
}