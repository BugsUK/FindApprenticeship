namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class HelpDeskCommunicationCommand : CommunicationCommand
    {
        public HelpDeskCommunicationCommand(IMessageBus messageBus)
            : base(messageBus)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType == MessageTypes.CandidateContactMessage;
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {
            QueueEmailMessage(communicationRequest);
        }
    }
}