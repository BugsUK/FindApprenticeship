namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public abstract class CommunicationCommand
    {
        private readonly IServiceBus _serviceBus;

        protected CommunicationCommand(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public abstract bool CanHandle(CommunicationRequest communicationRequest);

        public abstract void Handle(CommunicationRequest communicationRequest);

        protected void QueueEmailMessage(CommunicationRequest communicationRequest)
        {
            var toEmail = communicationRequest.Tokens.First(token => token.Key == CommunicationTokens.RecipientEmailAddress).Value;

            var request = new EmailRequest
            {
                ToEmail = toEmail,
                MessageType = communicationRequest.MessageType,
                Tokens = GetMessageTokens(communicationRequest),
            };

            _serviceBus.PublishMessage(request);
        }

        protected void QueueSmsMessage(CommunicationRequest communicationRequest)
        {
            var toMobile = communicationRequest.Tokens.First(token => token.Key == CommunicationTokens.CandidateMobileNumber).Value;

            var request = new SmsRequest
            {
                ToNumber = toMobile,
                MessageType = communicationRequest.MessageType,
                Tokens = GetMessageTokens(communicationRequest),
            };

            _serviceBus.PublishMessage(request);
        }

        private IEnumerable<CommunicationToken> GetMessageTokens(CommunicationRequest communicationRequest)
        {
            return communicationRequest.Tokens
                 .Where(token =>
                     token.Key != CommunicationTokens.RecipientEmailAddress &&
                     token.Key != CommunicationTokens.CandidateMobileNumber);
        }
    }
}