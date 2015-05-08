namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategyA : SendAccountRemindersStrategy
    {
        public SendAccountRemindersStrategyA(IConfigurationService configurationService, ICommunicationService communicationService, ILogService logService)
            : base(configurationService, communicationService, logService)
        {
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            //Only handle 50% of the users based on id
            if (Math.Abs(user.EntityId.GetHashCode()%2) == 0) return false;

            if (user.Status != UserStatuses.PendingActivation) return false;

            var housekeepingCyclesSinceCreation = GetHousekeepingCyclesSinceCreation(user);

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
                SendAccountReminder(user, candidate);
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceCreation == configuration.SendAccountReminderTwoAfterCycles)
            {
                SendAccountReminder(user, candidate);
                return true;
            }

            return false;
        }
    }
}