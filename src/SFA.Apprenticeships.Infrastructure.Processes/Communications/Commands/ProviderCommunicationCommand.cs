namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;

    // TODO: AG: US824: unit test.

    public class ProviderCommunicationCommand : CommunicationCommand
    {
        public ProviderCommunicationCommand(IServiceBus serviceBus) : base(serviceBus)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            var messageTypes = new[]
            {
                MessageTypes.SendProviderUserEmailVerificationCode,
                MessageTypes.ProviderContactUsMessage
            };

            return messageTypes.Contains(communicationRequest.MessageType);
        }

        public override void Handle(CommunicationRequest communicationRequest)
        {            
            QueueEmailMessage(communicationRequest);
        }
    }
}
