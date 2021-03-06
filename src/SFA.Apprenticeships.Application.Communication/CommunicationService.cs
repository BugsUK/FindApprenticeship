﻿namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;

    using SFA.Apprenticeships.Application.Interfaces;
    using Strategies;

    public class CommunicationService : ICommunicationService
    {
        private readonly ILogService _logger;

        private readonly ISendApplicationSubmittedStrategy _sendApplicationSubmittedStrategy;
        private readonly ISendTraineeshipApplicationSubmittedStrategy _sendTraineeshipApplicationSubmittedStrategy;
        private readonly ISendCandidateCommunicationStrategy _sendCandidateCommunicationStrategy;
        private readonly ISendContactMessageStrategy _sendContactMessageStrategy;
        private readonly ISendUsernameUpdateCommunicationStrategy _sendUsernameUpdateCommunicationStrategy;

        public CommunicationService(
            ILogService logger,
            ISendApplicationSubmittedStrategy sendApplicationSubmittedStrategy,
            ISendTraineeshipApplicationSubmittedStrategy sendTraineeshipApplicationSubmittedStrategy, 
            ISendCandidateCommunicationStrategy sendCandidateCommunicationStrategy, 
            ISendContactMessageStrategy sendContactMessageStrategy,
            ISendUsernameUpdateCommunicationStrategy sendUsernameUpdateCommunicationStrategy)
        {
            _logger = logger;
            _sendApplicationSubmittedStrategy = sendApplicationSubmittedStrategy;
            _sendTraineeshipApplicationSubmittedStrategy = sendTraineeshipApplicationSubmittedStrategy;
            _sendCandidateCommunicationStrategy = sendCandidateCommunicationStrategy;
            _sendContactMessageStrategy = sendContactMessageStrategy;
            _sendUsernameUpdateCommunicationStrategy = sendUsernameUpdateCommunicationStrategy;
        }

        public void SendMessageToCandidate(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _logger.Debug("CommunicationService called to send a message of type {0} to candidate with Id={1}", messageType, candidateId);

            switch (messageType)
            {
                case MessageTypes.ApprenticeshipApplicationSubmitted:
                    _sendApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.TraineeshipApplicationSubmitted:
                    _sendTraineeshipApplicationSubmittedStrategy.Send(candidateId, tokens);
                    break;

                case MessageTypes.SendActivationCode:
                case MessageTypes.SendPasswordResetCode:
                case MessageTypes.SendAccountUnlockCode:
                case MessageTypes.PasswordChanged:
                case MessageTypes.SendMobileVerificationCode:
                case MessageTypes.SendMobileVerificationCodeReminder:
                case MessageTypes.SendEmailReminder:
                case MessageTypes.SendActivationCodeReminder:
                case MessageTypes.SendDormantAccountReminder:
                    _sendCandidateCommunicationStrategy.Send(candidateId, messageType, tokens);
                    break;
                case MessageTypes.SendPendingUsernameCode:
                    _sendUsernameUpdateCommunicationStrategy.Send(candidateId, messageType, tokens);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType));
            }
        }

        public void SendContactMessage(Guid? userId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            _sendContactMessageStrategy.Send(userId, messageType, tokens);
        }
    }
}
