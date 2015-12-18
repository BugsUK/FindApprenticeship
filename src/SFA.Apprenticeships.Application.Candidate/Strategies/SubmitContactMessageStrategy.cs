namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using UserAccount.Configuration;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
        public const string DefaultUserFullName = "(anonymous)";
        public const string DefaultUserEnquiryDetails = "(no details provided)";

        private readonly ILogService _logService;
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationService _configurationService;
        private readonly IContactMessageRepository _contactMessageRepository;

        public SubmitContactMessageStrategy(
            ILogService logService,
            ICommunicationService communicationService,
            IConfigurationService configurationService,
            IContactMessageRepository contactMessageRepository)
        {
            _logService = logService;
            _communicationService = communicationService;
            _configurationService = configurationService;
            _contactMessageRepository = contactMessageRepository;
        }

        public void SubmitMessage(ContactMessage contactMessage)
        {
            _contactMessageRepository.Save(contactMessage);

            switch (contactMessage.Type)
            {
                case ContactMessageTypes.ContactUs:
                    SubmitContactUsMessage(contactMessage);
                    break;

                case ContactMessageTypes.Feedback:
                    SubmitFeedbackMesage(contactMessage);
                    break;

                default:
                    _logService.Error("Invalid contact message type '{0}' for id {1}", contactMessage.Type, contactMessage.EntityId);
                    break;
            }
        }

        #region Helpers

        private void SubmitFeedbackMesage(ContactMessage contactMessage)
        {
            var noReplyEmailAddress = _configurationService.Get<UserAccountConfiguration>().NoReplyEmailAddress;
            var feedbackEmailAddress = _configurationService.Get<UserAccountConfiguration>().FeedbackEmailAddress;

            var userEmailAddress = DefaultCommunicationToken(contactMessage.Email, noReplyEmailAddress);
            var userFullName = DefaultCommunicationToken(contactMessage.Name, DefaultUserFullName);

            _communicationService.SendContactMessage(contactMessage.UserId, MessageTypes.CandidateFeedbackMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, feedbackEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, userEmailAddress),
                new CommunicationToken(CommunicationTokens.UserFullName, userFullName),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, contactMessage.Details)
            });
        }

        private void SubmitContactUsMessage(ContactMessage contactMessage)
        {
            var helpdeskEmailAddress = _configurationService.Get<UserAccountConfiguration>().HelpdeskEmailAddress;
            var userEnquiryDetails = DefaultCommunicationToken(contactMessage.Details, DefaultUserEnquiryDetails);

            _communicationService.SendContactMessage(contactMessage.UserId, MessageTypes.CandidateContactUsMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, helpdeskEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, contactMessage.Email),
                new CommunicationToken(CommunicationTokens.UserFullName, contactMessage.Name),
                new CommunicationToken(CommunicationTokens.UserEnquiry, contactMessage.Enquiry),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, userEnquiryDetails)
            });
        }

        private static string DefaultCommunicationToken(string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        #endregion
    }
}