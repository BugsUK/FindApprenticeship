namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Configuration;
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
            //Only handle 50% of the users based on id
            if (Math.Abs(user.EntityId.GetHashCode()%2) == 1) return false;

            if(user.Status != UserStatuses.PendingActivation) return false;

            var timeSinceCreation = DateTime.Now - user.DateCreated;

            var housekeepingCyclesSinceCreation = (int)(timeSinceCreation.TotalHours / Configuration.HousekeepingCycleInHours);

            var configuration = Configuration.SendAccountReminderStrategyB;

            //Only remind if enough time has passed and not due for deletion
            if (housekeepingCyclesSinceCreation < configuration.SendAccountReminderAfterCycles 
                || housekeepingCyclesSinceCreation >= Configuration.SetPendingDeletionAfterCycles)
            {
                return false;
            }

            //Remind on the first cycle
            if (housekeepingCyclesSinceCreation == configuration.SendAccountReminderAfterCycles)
            {
                return true;
            }

            //Then every X cycles after that
            if ((housekeepingCyclesSinceCreation - configuration.SendAccountReminderAfterCycles) %
                configuration.SendAccountReminderEveryCycles == 0)
            {
                return true;
            }

            return false;
        }
    }
}