namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CandidateCommunicationCommand : CommunicationCommand
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public CandidateCommunicationCommand(
            IMessageBus messageBus,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
            : base(messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType != MessageTypes.CandidateContactMessage;
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            var candidateId = GetCandidateId(communicationRequest.EntityId);
            var user = _userReadRepository.Get(candidateId);

            if (user.Status == UserStatuses.Inactive || user.Status == UserStatuses.Dormant)
            {
                return;
            }

            var candidate = _candidateReadRepository.Get(candidateId);

            // Some messages are mandatory.
            var isMandatoryMessageType =
                !(communicationRequest.MessageType == MessageTypes.TraineeshipApplicationSubmitted ||
                communicationRequest.MessageType == MessageTypes.ApprenticeshipApplicationSubmitted);

            var isSmsMessageType =
                !(communicationRequest.MessageType == MessageTypes.SendActivationCode ||
                communicationRequest.MessageType == MessageTypes.SendPasswordResetCode ||
                communicationRequest.MessageType == MessageTypes.PasswordChanged ||
                communicationRequest.MessageType == MessageTypes.SendAccountUnlockCode);

            // Some messages are channel-specific.
            var isSmsOnlyMessageType = communicationRequest.MessageType == MessageTypes.SendMobileVerificationCode;

            if ((isMandatoryMessageType || candidate.CommunicationPreferences.AllowEmail) && !isSmsOnlyMessageType)
            {
                QueueEmailMessage(communicationRequest);
            }

            if (isSmsMessageType &&
                (isMandatoryMessageType || candidate.CommunicationPreferences.AllowMobile) &&
                (isSmsOnlyMessageType || candidate.CommunicationPreferences.VerifiedMobile))
            {
                QueueSmsMessage(communicationRequest);
            }
        }

        #region Helpers

        public Guid GetCandidateId(Guid? candidateId)
        {
            if (!candidateId.HasValue)
            {
                throw new ArgumentNullException("candidateId");
            }

            return candidateId.Value;
        }

        #endregion
    }
}
