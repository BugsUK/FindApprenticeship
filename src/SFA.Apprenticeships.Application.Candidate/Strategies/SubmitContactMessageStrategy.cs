namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using UserAccount.Configuration;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
        public const string DefaultUserFullName = "(anonymous)";
        public const string DefaultUserEmailAddress = "noreply@findapprenticeship.service.gov.uk";
        public const string DefaultUserEnquiryDetails = "(none)";

        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationService _configurationService;
        private readonly IContactMessageRepository _contactMessageRepository;

        public SubmitContactMessageStrategy(
            ICommunicationService communicationService,
            IConfigurationService configurationService,
            IContactMessageRepository contactMessageRepository)
        {
            _communicationService = communicationService;
            _configurationService = configurationService;
            _contactMessageRepository = contactMessageRepository;
        }

        public void SubmitMessage(ContactMessage contactMessage)
        {
            var recipientEmailAddress = _configurationService.Get<UserAccountConfiguration>().HelpdeskEmailAddress;

            var userFullName = DefaultCommunicationToken(contactMessage.Name, DefaultUserFullName);
            var userEmailAddress = DefaultCommunicationToken(contactMessage.Email, DefaultUserEmailAddress);
            var userEnquiryDetails = DefaultCommunicationToken(contactMessage.Details, DefaultUserEnquiryDetails);

            _contactMessageRepository.Save(contactMessage);
                
            _communicationService.SendContactMessage(contactMessage.UserId, MessageTypes.CandidateContactMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, recipientEmailAddress),
                new CommunicationToken(CommunicationTokens.UserFullName, userFullName),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, userEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEnquiry, contactMessage.Enquiry),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, userEnquiryDetails)
            });
        }

        #region Helpers

        private static string DefaultCommunicationToken(string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        #endregion
    }
}