namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    public class EmailCommunicationCommand : CommunicationCommand
    {
        public EmailCommunicationCommand(IServiceBus serviceBus) : base(serviceBus)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            var messageTypes = new[]
            {
                MessageTypes.SendEmployerApplicationLinks
            };

            return messageTypes.Contains(communicationRequest.MessageType);
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {            
            QueueEmailMessage(communicationRequest);
        }
    }
}
