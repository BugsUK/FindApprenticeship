namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategyA : HousekeepingStrategy
    {
        private readonly ILogService _logService;

        public SendAccountRemindersStrategyA(IConfigurationService configurationService, ILogService logService)
            : base(configurationService)
        {
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            //Only handle 50% of the users based on id
            if (Math.Abs(user.EntityId.GetHashCode()%2) == 0) return false;

            if (user.Status != UserStatuses.PendingActivation) return false;

            var timeSinceCreation = DateTime.Now - user.DateCreated;

            var housekeepingCyclesSinceCreation = (int)(timeSinceCreation.TotalHours / Configuration.HousekeepingCycleInHours);

            var configuration = Configuration.SendAccountReminderStrategyA;

            //Only remind if enough time has passed and not due for deletion
            if (housekeepingCyclesSinceCreation < configuration.SendAccountReminderOneAfterCycles
                || housekeepingCyclesSinceCreation >= Configuration.SetPendingDeletionAfterCycles)
            {
                return false;
            }

            //Remind on the first cycle
            if (housekeepingCyclesSinceCreation == configuration.SendAccountReminderOneAfterCycles)
            {
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceCreation == configuration.SendAccountReminderTwoAfterCycles)
            {
                return true;
            }

            return false;
        }
    }
}