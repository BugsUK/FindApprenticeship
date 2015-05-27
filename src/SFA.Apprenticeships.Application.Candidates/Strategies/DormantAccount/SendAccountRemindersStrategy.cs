namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using System;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;

    public class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly ICommunicationService _communicationService;

        public SendAccountRemindersStrategy(IConfigurationService configurationService, ICommunicationService communicationService) : base(configurationService)
        {
            _communicationService = communicationService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user.Status != UserStatuses.Active && user.Status != UserStatuses.Locked) return false;

            if (!user.LastLogin.HasValue) return false;

            var housekeepingCyclesSinceLastLogin = GetHousekeepingCyclesSince(user.LastLogin.Value);

            var configuration = Configuration.DormantAccountStrategy;

            //Remind on the first cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration);
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendFinalReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration);
                return true;
            }

            return false;
        }

        protected void SendAccountReminder(User user, Candidate candidate, DormantAccountStrategy configuration)
        {
            var lastLogin = user.LastLogin ?? DateTime.UtcNow;
            var lastLoginInDays = (DateTime.UtcNow - lastLogin).Days;
            var lastLoginInDaysFormatted = lastLoginInDays >= configuration.SendFinalReminderAfterCycles ? "almost a year" : string.Format("{0} days", lastLoginInDays);

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.LastLogin, lastLoginInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.AccountExpiryDate, lastLogin.AddDays(configuration.SetPendingDeletionAfterCycles).ToLongDateString()),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                });
        }
    }
}