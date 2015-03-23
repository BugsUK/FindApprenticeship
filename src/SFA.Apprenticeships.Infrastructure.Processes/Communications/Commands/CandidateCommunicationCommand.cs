namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;
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
            var messageTypes = new[]
            {
                MessageTypes.SendActivationCode,
                MessageTypes.SendPasswordResetCode,
                MessageTypes.SendAccountUnlockCode,
                MessageTypes.SendMobileVerificationCode,
                MessageTypes.ApprenticeshipApplicationSubmitted,
                MessageTypes.TraineeshipApplicationSubmitted,
                MessageTypes.PasswordChanged,
                MessageTypes.SavedSearchAlert
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
                return;
            }

            HandleEmailMessages(candidate, communicationRequest);
            HandleSmsMessages(candidate, communicationRequest);
        }

        protected virtual void HandleEmailMessages(Candidate candidate, CommunicationRequest communicationRequest)
        {
            if (candidate.ShouldSendMessageViaChannel(CommunicationChannels.Email, communicationRequest.MessageType))
            {
                QueueEmailMessage(communicationRequest);
            }
        }

        protected virtual void HandleSmsMessages(Candidate candidate, CommunicationRequest communicationRequest)
        {
            if (candidate.ShouldSendMessageViaChannel(CommunicationChannels.Sms, communicationRequest.MessageType))
            {
                QueueSmsMessage(communicationRequest);
            }
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

        private static bool ShouldCommunicateWithUser(User user)
        {
            return user.IsActive() || user.Status == UserStatuses.PendingActivation;
        }

        #endregion
    }
}
