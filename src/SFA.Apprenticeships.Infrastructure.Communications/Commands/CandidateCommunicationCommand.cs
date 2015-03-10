namespace SFA.Apprenticeships.Infrastructure.Communications.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CandidateCommunicationCommand : CommunicationCommand
    {
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CandidateCommunicationCommand(IMessageBus messageBus, ICandidateReadRepository candidateReadRepository)
            : base(messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType != MessageTypes.CandidateContactMessage;
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            var candidateId = communicationRequest.EntityId.Value;
            var candidate = _candidateReadRepository.Get(candidateId);

            // note, some messages are mandatory - determined by type
            var isOptionalMessageType = communicationRequest.MessageType == MessageTypes.TraineeshipApplicationSubmitted ||
                                        communicationRequest.MessageType == MessageTypes.ApprenticeshipApplicationSubmitted;

            // note, some messages are channel specific
            var isSmsOnly = communicationRequest.MessageType == MessageTypes.SendMobileVerificationCode;

            if ((!isOptionalMessageType || candidate.CommunicationPreferences.AllowEmail) && !isSmsOnly)
            {
                QueueEmailMessage(communicationRequest);
            }

            if (!isOptionalMessageType || candidate.CommunicationPreferences.AllowMobile)
            {
                QueueSmsMessage(communicationRequest);
            }
        }
    }
}
