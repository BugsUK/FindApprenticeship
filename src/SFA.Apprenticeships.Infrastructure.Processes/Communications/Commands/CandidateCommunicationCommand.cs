namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Communication.Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Newtonsoft.Json;

    using SFA.Apprenticeships.Application.Interfaces;

    public class CandidateCommunicationCommand : CommunicationCommand
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public CandidateCommunicationCommand(
            ILogService logService,
            IConfigurationService configurationService,
            IServiceBus serviceBus,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
            : base(serviceBus)
        {
            _logService = logService;
            _configurationService = configurationService;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            var messageTypes = new[]
            {
                MessageTypes.SendActivationCode,
                MessageTypes.SendPasswordResetCode,
                MessageTypes.SendAccountUnlockCode,
                MessageTypes.SendMobileVerificationCode,
                MessageTypes.SendMobileVerificationCodeReminder,
                MessageTypes.ApprenticeshipApplicationSubmitted,
                MessageTypes.TraineeshipApplicationSubmitted,
                MessageTypes.PasswordChanged,
                MessageTypes.SavedSearchAlert,
                MessageTypes.SendPendingUsernameCode,
                MessageTypes.SendEmailReminder,
                MessageTypes.SendActivationCodeReminder,
                MessageTypes.SendDormantAccountReminder
            };

            return messageTypes.Contains(communicationRequest.MessageType);
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            var candidateId = GetCandidateId(communicationRequest);
            var user = _userReadRepository.Get(candidateId);
            var candidate = _candidateReadRepository.Get(candidateId);

            if (!ShouldCommunicateWithUser(user))
            {
                _logService.Info("Will NOT send any messages to user '{0}' email '{1}' in state '{2}'",
                    user.EntityId, user.Username, user.Status);
                return;
            }

            HandleEmailMessage(candidate, communicationRequest);
            HandleSmsMessage(candidate, communicationRequest);
        }

        protected virtual void HandleEmailMessage(Candidate candidate, CommunicationRequest communicationRequest)
        {
            var sendMessage = candidate.ShouldSendMessageViaChannel(CommunicationChannels.Email, communicationRequest.MessageType);

            LogSendMessageResult(sendMessage, CommunicationChannels.Email, candidate, communicationRequest);

            if (sendMessage)
            {
                AddCommonTokens(communicationRequest);
                QueueEmailMessage(communicationRequest);
            }
        }

        protected virtual void HandleSmsMessage(Candidate candidate, CommunicationRequest communicationRequest)
        {
            var sendMessage = candidate.ShouldSendMessageViaChannel(CommunicationChannels.Sms, communicationRequest.MessageType);

            LogSendMessageResult(sendMessage, CommunicationChannels.Sms, candidate, communicationRequest);

            if (sendMessage)
            {
                AddCommonTokens(communicationRequest);
                QueueSmsMessage(communicationRequest);
            }
        }

        #region Helpers

        private void LogSendMessageResult(bool sendMessage, CommunicationChannels communicationChannel, Candidate candidate, CommunicationRequest communicationRequest)
        {
            _logService.Info(
                "{0} send message type '{1}' via channel '{2}' to candidate '{3}' email '{4}' mobile number '{5}' with preferences '{6}'",
                sendMessage ? "Will" : "Will NOT",
                communicationRequest.MessageType,
                communicationChannel,
                candidate.EntityId,
                candidate.RegistrationDetails.EmailAddress,
                candidate.RegistrationDetails.PhoneNumber,
                JsonConvert.SerializeObject(candidate.CommunicationPreferences));
        }

        private static Guid GetCandidateId(CommunicationRequest communicationRequest)
        {
            if (!communicationRequest.EntityId.HasValue)
            {
                throw new InvalidOperationException("Candidate Id is null.");
            }

            return communicationRequest.EntityId.Value;
        }

        private static bool ShouldCommunicateWithUser(User user)
        {
            return user.IsActive() || user.Status == UserStatuses.PendingActivation || user.Status == UserStatuses.Dormant;
        }


        private void AddCommonTokens(CommunicationRequest communicationRequest)
        {
            var tokens = communicationRequest.Tokens.ToList();

            AddCandidateSiteDomainNameToken(tokens);

            communicationRequest.Tokens = tokens;
        }

        private void AddCandidateSiteDomainNameToken(ICollection<CommunicationToken> tokens)
        {
            if (tokens.Any(each => each.Key == CommunicationTokens.CandidateSiteDomainName))
            {
                return;
            }

            tokens.Add(new CommunicationToken(
                CommunicationTokens.CandidateSiteDomainName,
                _configurationService.Get<CommunicationConfiguration>().SiteDomainName));
        }

        #endregion
    }
}
