using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Unsubscribe
{
    using System;
    using Apprenticeships.Application.Interfaces.Communications;

    public interface IUnsubscribeMediator
    {
        MediatorResponse Unsubscribe(Guid? candidateId, Guid subscriberId, SubscriptionTypes subscriptionType);
    }
}