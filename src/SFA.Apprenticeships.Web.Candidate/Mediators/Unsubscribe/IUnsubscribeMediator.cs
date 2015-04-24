namespace SFA.Apprenticeships.Web.Candidate.Mediators.Unsubscribe
{
    using System;

    public interface IUnsubscribeMediator
    {
        MediatorResponse Unsubscribe(Guid? candidateId, Guid subscriberId, int subscriptionTypeId);
    }
}