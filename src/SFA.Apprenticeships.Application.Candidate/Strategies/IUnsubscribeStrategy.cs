namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Interfaces.Communications;

    public interface IUnsubscribeStrategy
    {
        bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType, string subscriptionItemId = null);
    }
}
