namespace SFA.Apprenticeships.Infrastructure.UnitTests.AzureServiceBus
{
    using System;
    using Application.UserAccount.Entities;
    using Domain.Interfaces.Messaging;

    public class CandidateUserUpdateSubscriberMock : IServiceBusSubscriber<CandidateUserUpdate>
    {
        private readonly ServiceBusMessageStates _stateToReturn;
        private readonly Exception _exceptionToThrow;

        public CandidateUserUpdateSubscriberMock(ServiceBusMessageStates stateToReturn)
        {
            _stateToReturn = stateToReturn;
        }

        public CandidateUserUpdateSubscriberMock(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow;
        }

        [ServiceBusTopicSubscription(TopicName = "CandidateUserUpdate")]
        public ServiceBusMessageStates Consume(CandidateUserUpdate message)
        {
            if (_exceptionToThrow != null)
            {
                throw _exceptionToThrow;
            }
            return _stateToReturn;
        }
    }
}