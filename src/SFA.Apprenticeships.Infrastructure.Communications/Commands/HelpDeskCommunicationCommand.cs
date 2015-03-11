namespace SFA.Apprenticeships.Infrastructure.Communications.Commands
{
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    //todo: 1.8: move to async processor
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