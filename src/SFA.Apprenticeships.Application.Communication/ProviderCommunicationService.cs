namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;
    using Interfaces;
    using Strategies;

    public class ProviderCommunicationService : IProviderCommunicationService
    {
        private readonly ISendProviderUserCommunicationStrategy _sendProviderUserCommunicationStrategy;
        private readonly ILogService _logger;

        public ProviderCommunicationService(
            ILogService logger,
            ISendProviderUserCommunicationStrategy sendProviderUserCommunicationStrategy)
        {
            _logger = logger;
            _sendProviderUserCommunicationStrategy = sendProviderUserCommunicationStrategy;
        }

        public void SendMessageToProviderUser(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _logger.Debug("CommunicationService called to send a message of type {0} to provider user with name='{1}'", messageType, username);

            switch (messageType)
            {
                case MessageTypes.SendProviderUserEmailVerificationCode:
                    _sendProviderUserCommunicationStrategy.Send(username, messageType, tokens);
                    break;

                case MessageTypes.ProviderContactUsMessage:
                    _sendProviderUserCommunicationStrategy.Send(username, messageType, tokens);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType));
            }
        }
    }
}
