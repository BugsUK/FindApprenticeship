namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class QueueUsernameUpdateCommunicationStrategy : ISendUsernameUpdateCommunicationStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IServiceBus _serviceBus;

        public QueueUsernameUpdateCommunicationStrategy(
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository, 
            IServiceBus serviceBus)
        {
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _serviceBus = serviceBus;
        }

        public void Send(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            var user = _userReadRepository.Get(candidateId);
            var candidate = _candidateReadRepository.Get(candidateId);

            tokens = tokens.Union(new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName), 
                    new CommunicationToken(CommunicationTokens.RecipientEmailAddress, user.PendingUsername),
                    new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber)
                });

            var request = new CommunicationRequest
            {
                EntityId = candidateId,
                MessageType = messageType,
                Tokens = tokens
            };

            _serviceBus.PublishMessage(request);
        }
    }
}
