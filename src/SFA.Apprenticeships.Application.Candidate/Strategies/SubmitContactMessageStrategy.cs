namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationManager _configurationManager;
        private readonly IContactMessageRepository _contactMessageRepository;

        public SubmitContactMessageStrategy(
            ICommunicationService communicationService,
            IConfigurationManager configurationManager,
            IContactMessageRepository contactMessageRepository)
        {
            _communicationService = communicationService;
            _configurationManager = configurationManager;
            _contactMessageRepository = contactMessageRepository;
        }

        public void SubmitMessage(ContactMessage contactMessage)
        {
            var helpdeskAddress = _configurationManager.GetAppSetting<string>("HelpdeskEmailAddress");
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