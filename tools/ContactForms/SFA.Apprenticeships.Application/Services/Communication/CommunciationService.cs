namespace SFA.Apprenticeships.Application.Services.Communication
{
    using System;
    using System.Collections.Generic;
    using Strategies.Interfaces;
    using Interfaces;
    using Interfaces.Communications;

    public class CommunciationService : ICommunciationService
    {
        private readonly ISendAccessRequestStrategy _accessRequestSendStrategy;
        private readonly ISendEmployerEnquiryStrategy _employerEnquirySendStrategy;
        private readonly ISendGlaEmployerEnquiryStrategy _glaEmployerEnquirySendStrategy;
        private readonly ILogService _logger;
        public CommunciationService(ILogService logger, ISendAccessRequestStrategy accessRequestSendStrategy, ISendEmployerEnquiryStrategy employerEnquirySendStrategy, ISendGlaEmployerEnquiryStrategy glaEmployerEnquirySendStrategy)
        {
            _logger = logger;
            _accessRequestSendStrategy = accessRequestSendStrategy;
            _employerEnquirySendStrategy = employerEnquirySendStrategy;
            _glaEmployerEnquirySendStrategy = glaEmployerEnquirySendStrategy;
        }

        public void SendMessage(MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _logger.Debug("CommunicationService called to send a message of type {0}", messageType);
            switch (messageType)
            {
                //Messages to helpdesk
                case MessageTypes.EmployerEnquiry:
                    _employerEnquirySendStrategy.Send(messageType, tokens);
                    break;
                case MessageTypes.GlaEmployerEnquiry:
                    _glaEmployerEnquirySendStrategy.Send(messageType, tokens);
                    break;
                case MessageTypes.WebAccessRequest:
                    _accessRequestSendStrategy.Send(messageType, tokens);
                    break;
                    //Confirmation messages to applciants
                case MessageTypes.EmployerEnquiryConfirmation:
                    _employerEnquirySendStrategy.SendMessageToApplicant(messageType, tokens);
                    break;
                case MessageTypes.GlaEmployerEnquiryConfirmation:
                    _glaEmployerEnquirySendStrategy.SendMessageToApplicant(messageType, tokens);
                    break;
                case MessageTypes.WebAccessRequestConfirmation:
                    _accessRequestSendStrategy.SendMessageToApplicant(messageType, tokens);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }
    }
}