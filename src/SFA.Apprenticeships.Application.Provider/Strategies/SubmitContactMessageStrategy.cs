namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using SFA.Apprenticeships.Application.Interfaces.Communications;
    using SFA.Apprenticeships.Application.UserAccount.Configuration;
    using SFA.Apprenticeships.Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
        public const string DefaultUserFullName = "(anonymous)";
        public const string DefaultUserEnquiryDetails = "(no details provided)";

        private readonly ILogService _logService;        
        private readonly IConfigurationService _configurationService;
        private readonly IProviderCommunicationService _communicationService;        

        public SubmitContactMessageStrategy(
            ILogService logService,
            IProviderCommunicationService communicationService,
            IConfigurationService configurationService)
        {
            _logService = logService;
            _communicationService = communicationService;
            _configurationService = configurationService;            
        }

        public void SubmitMessage(ProviderContactMessage contactMessage)
        {
           // _contactMessageRepository.Save(contactMessage);

            switch (contactMessage.Type)
            {
                case ContactMessageTypes.ContactUs:
                    SubmitContactUsMessage(contactMessage);
                    break;                

                default:
                    _logService.Error("Invalid contact message type '{0}' for id {1}", contactMessage.Type, contactMessage.EntityId);
                    break;
            }
        }

        #region Helpers        

        private void SubmitContactUsMessage(ProviderContactMessage contactMessage)
        {
            var helpdeskEmailAddress = _configurationService.Get<UserAccountConfiguration>().HelpdeskEmailAddress;
            var userEnquiryDetails = DefaultCommunicationToken(contactMessage.Details, DefaultUserEnquiryDetails);

            _communicationService.SendMessageToProviderUser(contactMessage.Name, MessageTypes.ProviderContactUsMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, helpdeskEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, contactMessage.Email),
                new CommunicationToken(CommunicationTokens.UserFullName, contactMessage.Name),                
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