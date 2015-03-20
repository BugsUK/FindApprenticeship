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
            // TODO: AG: inclusive test here.
            return communicationRequest.MessageType != MessageTypes.CandidateContactMessage &&
                   communicationRequest.MessageType != MessageTypes.DailyDigest;
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            var candidateId = GetCandidateId(communicationRequest);
            var user = _userReadRepository.Get(candidateId);
            var candidate = _candidateReadRepository.Get(candidateId);

            if (!ShouldCommunicateWithCandidate(user))
            {
                return;
            }

            if ((IsMandatoryMessageType(communicationRequest) || candidate.CommunicationPreferences.AllowEmail) &&
                !IsSmsOnlyMessageType(communicationRequest))
            {
                QueueEmailMessage(communicationRequest);
            }

            if (IsSmsMessageType(communicationRequest) &&
                (IsMandatoryMessageType(communicationRequest) || candidate.CommunicationPreferences.AllowMobile) &&
                (IsSmsOnlyMessageType(communicationRequest) || candidate.CommunicationPreferences.VerifiedMobile))
            {
                QueueSmsMessages(communicationRequest);
            }
        }

        protected virtual void QueueSmsMessages(CommunicationRequest communicationRequest)
        {
            // Some candidate communication requests may result in more than one SMS, by default we send one.
            QueueSmsMessage(communicationRequest);
        }

        #region Helpers

        private static Guid GetCandidateId(CommunicationRequest communicationRequest)
        {
            if (!communicationRequest.EntityId.HasValue)
            {
                throw new InvalidOperationException("Candidate Id is null.");
            }

            return communicationRequest.EntityId.Value;
        }

        private static bool ShouldCommunicateWithCandidate(User user)
        {
            return user.IsActive() || user.Status == UserStatuses.PendingActivation;
        }

        private static bool IsSmsOnlyMessageType(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType == MessageTypes.SendMobileVerificationCode;
        }

        private static bool IsSmsMessageType(CommunicationRequest communicationRequest)
        {
            return !(communicationRequest.MessageType == MessageTypes.SendActivationCode ||
                     communicationRequest.MessageType == MessageTypes.SendPasswordResetCode ||
                     communicationRequest.MessageType == MessageTypes.PasswordChanged ||
                     communicationRequest.MessageType == MessageTypes.SendAccountUnlockCode);
        }

        private static bool IsMandatoryMessageType(CommunicationRequest communicationRequest)
        {
            return !(communicationRequest.MessageType == MessageTypes.TraineeshipApplicationSubmitted ||
                     communicationRequest.MessageType == MessageTypes.ApprenticeshipApplicationSubmitted);
        }

        #endregion
    }
}
