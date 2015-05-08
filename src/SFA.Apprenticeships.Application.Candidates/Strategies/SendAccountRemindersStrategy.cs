namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public abstract class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly ILogService _logService;

        protected SendAccountRemindersStrategy(IConfigurationService configurationService, ICommunicationService communicationService, ILogService logService)
            : base(configurationService)
        {
            _communicationService = communicationService;
            _logService = logService;
        }

        protected void SendAccountReminder(User user, Candidate candidate)
        {
            var activationCodeExpiryInDays = user.ActivateCodeExpiry.HasValue ? (user.ActivateCodeExpiry.Value - DateTime.UtcNow).Days : 0;
            var activationCodeExpiryInDaysFormatted = activationCodeExpiryInDays == 1 ? "1 day" : string.Format("{0} days", activationCodeExpiryInDays);

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