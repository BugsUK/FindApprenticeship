namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class QueueProviderUserCommunicationStrategy : ISendProviderUserCommunicationStrategy
    {
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IServiceBus _serviceBus;

        public QueueProviderUserCommunicationStrategy(
            IProviderUserReadRepository providerUserReadRepository,
            IServiceBus serviceBus)
        {
            _providerUserReadRepository = providerUserReadRepository;
            _serviceBus = serviceBus;
        }

        public void Send(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            var providerUser = _providerUserReadRepository.Get(username);

            // TODO: AG: US824: fix email address.
            tokens = tokens.Union(new[]
                {
                    new CommunicationToken(CommunicationTokens.ProviderUserFullName, providerUser.Fullname), 
                    new CommunicationToken(CommunicationTokens.RecipientEmailAddress, "valtechnas@gmail.com"),
                });

            var request = new CommunicationRequest
            {
                EntityId = providerUser.EntityId,
                MessageType = messageType,
                Tokens = tokens
            };

            _serviceBus.PublishMessage(request);
        }
    }
}
