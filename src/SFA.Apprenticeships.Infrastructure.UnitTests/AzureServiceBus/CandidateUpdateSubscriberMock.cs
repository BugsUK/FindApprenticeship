namespace SFA.Apprenticeships.Infrastructure.UnitTests.AzureServiceBus
{
    using System;
    using Application.Candidate.Entities;
    using Domain.Interfaces.Messaging;

    public class CandidateUpdateSubscriberMock : IServiceBusSubscriber<CandidateUpdate>
    {
        private readonly ServiceBusMessageStates _stateToReturn;
        private readonly Exception _exceptionToThrow;

        public CandidateUpdateSubscriberMock(ServiceBusMessageStates stateToReturn)
        {
            _stateToReturn = stateToReturn;
        }

        public CandidateUpdateSubscriberMock(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow;
        }

        [ServiceBusTopicSubscription(TopicName = "CandidateUpdate")]
        public ServiceBusMessageStates Consume(CandidateUpdate message)
        {
            if (_exceptionToThrow != null)
            {
                throw _exceptionToThrow;
            }
            return _stateToReturn;
        }
    }
}