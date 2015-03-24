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
            var config = _configurationService.Get<UserAccountConfiguration>(UserAccountConfiguration.ConfigurationName);
            var helpdeskAddress = config.HelpdeskEmailAddress;
            var details = string.IsNullOrWhiteSpace(contactMessage.Details) ? string.Empty : contactMessage.Details;

            _contactMessageRepository.Save(contactMessage);
                
            _communicationService.SendContactMessage(contactMessage.UserId, MessageTypes.CandidateContactMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, helpdeskAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, contactMessage.Email),
                new CommunicationToken(CommunicationTokens.UserFullName, contactMessage.Name),
                new CommunicationToken(CommunicationTokens.UserEnquiry, contactMessage.Enquiry),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, details)
            });
        }
    }
}