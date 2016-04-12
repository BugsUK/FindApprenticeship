namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Repositories;

    public class QueueProviderCommunicationStrategy : ISendProviderCommunicationStrategy
    {
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IServiceBus _serviceBus;

        public QueueProviderCommunicationStrategy(
            IProviderUserReadRepository providerUserReadRepository,
            IServiceBus serviceBus)
        {
            _providerUserReadRepository = providerUserReadRepository;
            _serviceBus = serviceBus;
        }

        public void Send(string username, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            var providerUser = _providerUserReadRepository.GetByUsername(username);

            if (providerUser != null)
            {
                tokens = tokens.Union(new[]
                                          {
                                              new CommunicationToken(CommunicationTokens.ProviderUserFullName, providerUser.Fullname), 
                                              new CommunicationToken(CommunicationTokens.RecipientEmailAddress, providerUser.Email),
                                          });
            }            

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
