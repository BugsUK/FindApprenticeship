namespace SFA.Apprenticeships.Application.Candidates.Strategies.ActivationReminder
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Interfaces.Communications;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly ICommunicationService _communicationService;

        public SendAccountRemindersStrategy(
            IConfigurationService configurationService,
            ICommunicationService communicationService)
            : base(configurationService)
        {
            _communicationService = communicationService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user == null || candidate == null) return false;

            if (user.Status != UserStatuses.PendingActivation) return false;

            var housekeepingCyclesSinceCreation = GetHousekeepingCyclesSince(user.DateCreated);

            var configuration = Configuration.ActivationReminderStrategy.SendAccountReminderStrategy;

            //Only remind if enough time has passed and not due for deletion
            if (housekeepingCyclesSinceCreation < configuration.SendAccountReminderAfterCycles
                || housekeepingCyclesSinceCreation >= Configuration.ActivationReminderStrategy.SetPendingDeletionAfterCycles)
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

        protected void SendAccountReminder(User user, Candidate candidate)
        {
            var activationCodeExpiryInDays = user.ActivateCodeExpiry.HasValue ? (user.ActivateCodeExpiry.Value - DateTime.UtcNow).Days : 0;
            var activationCodeExpiryInDaysFormatted = activationCodeExpiryInDays == 1 ? "1 day" : string.Format("{0} days", activationCodeExpiryInDays);
            if (user.ActivateCodeExpiry.HasValue)
            {
                activationCodeExpiryInDaysFormatted += " on " + user.ActivateCodeExpiry.Value.ToLongDateString();
            }

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendActivationCodeReminder,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.ActivationCode, user.ActivationCode),
                    new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, activationCodeExpiryInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                });
        }
    }
}