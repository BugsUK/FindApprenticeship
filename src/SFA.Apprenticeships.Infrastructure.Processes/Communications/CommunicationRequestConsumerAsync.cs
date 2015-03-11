namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Communications;
    using EasyNetQ.AutoSubscribe;
    using Infrastructure.Communications.Commands;

    public class CommunicationRequestConsumerAsync : IConsumeAsync<CommunicationRequest>
    {
        private readonly List<CommunicationCommand> _communicationCommands = new List<CommunicationCommand>();

        public CommunicationRequestConsumerAsync(IEnumerable<CommunicationCommand> communicationCommands)
        {
            _communicationCommands = new List<CommunicationCommand>(communicationCommands);
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CommunicationRequestConsumerAsync")]
        public Task Consume(CommunicationRequest message)
        {
            return Task.Run(() => _communicationCommands
                .First(communicationCommand => communicationCommand.CanHandle(message))
                .Handle(message));
        }
    }
}
