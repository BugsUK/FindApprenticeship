namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.AzureServiceBus
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class CreateCandidateRequestSubscriberMock : IServiceBusSubscriber<CreateCandidateRequest>
    {
        private readonly ServiceBusMessageStates _stateToReturn;
        private readonly Exception _exceptionToThrow;

        public CreateCandidateRequestSubscriberMock(ServiceBusMessageStates stateToReturn)
        {
            _stateToReturn = stateToReturn;
        }

        public CreateCandidateRequestSubscriberMock(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow;
        }

        [ServiceBusTopicSubscription(TopicName = "CreateCandidate")]
        public ServiceBusMessageStates Consume(CreateCandidateRequest message)
        {
            if (_exceptionToThrow != null)
            {
                throw _exceptionToThrow;
            }
            return _stateToReturn;
        }
    }
}