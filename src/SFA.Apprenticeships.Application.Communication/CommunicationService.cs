﻿namespace SFA.Apprenticeships.Application.Communication
{
    using System;
    using System.Collections.Generic;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Strategies;

    public class CommunicationService : ICommunicationService
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ISendActivationCodeStrategy _sendActivationCodeStrategy;

        public CommunicationService(ICandidateReadRepository candidateReadRepository, IUserReadRepository userReadRepository, ISendActivationCodeStrategy sendActivationCodeStrategy)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _sendActivationCodeStrategy = sendActivationCodeStrategy;
            
            // TODO: NOTIMPL: other strategies go here...
        }

        public void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType, IList<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var templateName = GetTemplateName(messageType);

            switch (messageType)
            {
                case CandidateMessageTypes.SendActivationCode:
                    var candidate = _candidateReadRepository.Get(candidateId);
                    var user = _userReadRepository.Get(candidate.RegistrationDetails.EmailAddress);
                    var activationCode = user.ActivationCode;

                    _sendActivationCodeStrategy.Send(templateName, candidate, activationCode);
                    break;

                case CandidateMessageTypes.SendPasswordCode:
                    // TODO: NOTIMPL: get candidate, invoke strategy to send forgotten password email to candidate
                    break;

                case CandidateMessageTypes.ApplicationSubmitted:
                    // TODO: NOTIMPL: get candidate, invoke strategy to send application acknowledgement email to candidate
                    break;

                case CandidateMessageTypes.PasswordChanged:
                    // TODO: NOTIMPL: get candidate, invoke strategy to send password changed email to candidate
                    break;

                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
        }

        private static string GetTemplateName(Enum messageType)
        {
            const string format = "{0}.{1}";
            var enumType = messageType.GetType();

            return string.Format(format, enumType.Name, Enum.GetName(enumType, messageType));
        }
    }
}
