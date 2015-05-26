namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategyB : SendAccountRemindersStrategy
    {
        public SendAccountRemindersStrategyB(IConfigurationService configurationService, ICommunicationService communicationService, ILogService logService)
            : base(configurationService, communicationService, logService)
        {
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user == null || candidate == null) return false;

            //Only handle 50% of the users based on date of birth
            if (candidate.RegistrationDetails.DateOfBirth.Day%2 == 1) return false;

            if (user.Status != UserStatuses.PendingActivation) return false;

            var housekeepingCyclesSinceCreation = GetHousekeepingCyclesSince(user.DateCreated);

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
                SendAccountReminder(user, candidate);
                return true;
            }

            //Then every X cycles after that
            if ((housekeepingCyclesSinceCreation - configuration.SendAccountReminderAfterCycles) %
                configuration.SendAccountReminderEveryCycles == 0)
            {
                SendAccountReminder(user, candidate);
                return true;
            }

            return false;
        }
    }
}