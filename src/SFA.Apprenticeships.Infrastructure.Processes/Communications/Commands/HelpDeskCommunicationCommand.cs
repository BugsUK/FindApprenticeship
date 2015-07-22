namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class HelpDeskCommunicationCommand : CommunicationCommand
    {
        public HelpDeskCommunicationCommand(IServiceBus serviceBus)
            : base(serviceBus)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return
                communicationRequest.MessageType == MessageTypes.CandidateContactUsMessage ||
                communicationRequest.MessageType == MessageTypes.CandidateFeedbackMessage;
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            QueueEmailMessage(communicationRequest);
        }
    }
}