namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Interfaces.Messaging;
    using Interfaces.Communications;

    public class QueueEmployerCommunicationStrategy : ISendEmployerCommunicationStrategy
    {
        private readonly IServiceBus _serviceBus;

        public QueueEmployerCommunicationStrategy(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            var request = new CommunicationRequest
            {
                //TODO: Might be an issue nulling this
                EntityId = null,
                MessageType = messageType,
                Tokens = tokens
            };

            _serviceBus.PublishMessage(request);
        }
    }
}