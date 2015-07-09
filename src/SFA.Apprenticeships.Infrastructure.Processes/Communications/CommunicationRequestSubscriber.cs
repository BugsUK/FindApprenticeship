namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Commands;
    using Domain.Interfaces.Messaging;

    public class CommunicationRequestSubscriber : IServiceBusSubscriber<CommunicationRequest>
    {
        private readonly List<CommunicationCommand> _communicationCommands = new List<CommunicationCommand>();

        public CommunicationRequestSubscriber(IEnumerable<CommunicationCommand> communicationCommands)
        {
            _communicationCommands = new List<CommunicationCommand>(communicationCommands);
        }

        [ServiceBusTopicSubscription(TopicName = "communication-request")]
        public ServiceBusMessageResult Consume(CommunicationRequest message)
        {
            _communicationCommands
                .First(communicationCommand => communicationCommand.CanHandle(message))
                .Handle(message);

            return ServiceBusMessageResult.Complete();
        }
    }
}
