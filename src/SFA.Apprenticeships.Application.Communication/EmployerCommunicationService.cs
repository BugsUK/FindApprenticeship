namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Interfaces.Communications;
    using Strategies;

    public class EmployerCommunicationService : IEmployerCommunicationService
    {
        private readonly ISendEmployerCommunicationStrategy _sendEmployerCommunicationStrategy;
        private readonly ILogService _logger;

        public EmployerCommunicationService(ISendEmployerCommunicationStrategy sendEmployerCommunicationStrategy, ILogService logger)
        {
            _sendEmployerCommunicationStrategy = sendEmployerCommunicationStrategy;
            _logger = logger;
        }

        public void SendMessageToEmployer(string recipientEmailAddress, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _logger.Debug("CommunicationService called to send a message of type {0} to employer user with email='{1}'", messageType, recipientEmailAddress);

            switch (messageType)
            {
                case MessageTypes.SendEmployerApplicationLinks:
                    _sendEmployerCommunicationStrategy.Send(messageType, tokens);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType));
            }
        }
    }
}