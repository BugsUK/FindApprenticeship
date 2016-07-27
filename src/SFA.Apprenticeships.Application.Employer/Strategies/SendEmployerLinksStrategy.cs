namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Interfaces.Communications;
    using Newtonsoft.Json;

    public class SendEmployerLinksStrategy : ISendEmployerLinksStrategy
    {
        private readonly IEmployerCommunicationService _communicationService;

        public SendEmployerLinksStrategy(IEmployerCommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public void Send(IDictionary<string, string> applicationLinks, string recipientEmailAddress)
        {
            _communicationService.SendMessageToEmployer(recipientEmailAddress, MessageTypes.SendEmployerApplicationLinks,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.RecipientEmailAddress, recipientEmailAddress),
                    new CommunicationToken(CommunicationTokens.EmployerApplicationLinks, JsonConvert.SerializeObject(applicationLinks))
                });
        }
    }
}