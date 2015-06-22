namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using UserAccount.Configuration;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
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

            var userFullName = string.IsNullOrWhiteSpace(contactMessage.Email) ? string.Empty : contactMessage.Name;
            var userEmailAddress = string.IsNullOrWhiteSpace(contactMessage.Email) ? string.Empty : contactMessage.Email;
            var userEnquiry = string.IsNullOrWhiteSpace(contactMessage.Enquiry) ? string.Empty : contactMessage.Enquiry;
            var userEnquiryDetails = string.IsNullOrWhiteSpace(contactMessage.Details) ? string.Empty : contactMessage.Details;

            _contactMessageRepository.Save(contactMessage);
                
            _communicationService.SendContactMessage(contactMessage.UserId, MessageTypes.CandidateContactMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, recipientEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, userEmailAddress),
                new CommunicationToken(CommunicationTokens.UserFullName, userFullName),
                new CommunicationToken(CommunicationTokens.UserEnquiry, userEnquiry),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, userEnquiryDetails)
            });
        }
    }
}