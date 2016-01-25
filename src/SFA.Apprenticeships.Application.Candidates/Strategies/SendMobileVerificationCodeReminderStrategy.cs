namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Interfaces.Communications;

    public class SendMobileVerificationCodeReminderStrategy : HousekeepingStrategy
    {
        private readonly ILogService _logger;
        private readonly ICommunicationService _communicationService;

        public SendMobileVerificationCodeReminderStrategy(
            ILogService logger,
            IConfigurationService configurationService,
            ICommunicationService communicationService)
            : base(configurationService)
        {
            _logger = logger;
            _communicationService = communicationService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user == null || candidate == null) return false;

            if (!candidate.CommunicationPreferences.MobileVerificationCodeDateCreated.HasValue ||
                string.IsNullOrWhiteSpace(candidate.CommunicationPreferences.MobileVerificationCode))
            {
                _logger.Debug("Will NOT send mobile verification code reminder to candidate '{0}': no verification date or verified", candidate.EntityId);
                return false;
            }

            if (!(candidate.CommunicationPreferences.ApplicationStatusChangePreferences.EnableText ||
                candidate.CommunicationPreferences.ExpiringApplicationPreferences.EnableText ||
                candidate.CommunicationPreferences.MarketingPreferences.EnableText ||
                candidate.CommunicationPreferences.SavedSearchPreferences.EnableText))
            {
                _logger.Debug("Will NOT send mobile verification code reminder to candidate '{0}': no text communications enabled.", candidate.EntityId);
                return false;
            }

            var cycleCount = GetHousekeepingCyclesSince(candidate.CommunicationPreferences.MobileVerificationCodeDateCreated.Value);

            if (cycleCount != Configuration.SendMobileVerificationCodeReminderAfterCycles)
            {
                _logger.Debug("Will NOT send mobile verification code reminder to candidate '{0}': {1} housekeeping cycle(s) not passed.",
                    candidate.EntityId, Configuration.SendMobileVerificationCodeReminderAfterCycles);
                return false;
            }

            var tokens = new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
                new CommunicationToken(CommunicationTokens.MobileVerificationCode, candidate.CommunicationPreferences.MobileVerificationCode)
            };

            _logger.Debug("Will send mobile verification code reminder to candidate '{0}'", candidate.EntityId);
            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendMobileVerificationCodeReminder, tokens);

            // Always allow successor strategies to handle this message.
            return false;
        }
    }
}