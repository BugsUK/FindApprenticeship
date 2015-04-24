namespace SFA.Apprenticeships.Web.Candidate.Mediators.Unsubscribe
{
    using System;
    using Apprenticeships.Application.Interfaces.Communications;
    using Common.Constants;
    using Constants.Pages;
    using Providers;

    public class UnsubscribeMediator : MediatorBase, IUnsubscribeMediator
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;

        public UnsubscribeMediator(ICandidateServiceProvider candidateServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
        }

        public MediatorResponse Unsubscribe(Guid? candidateId, Guid subscriberId, int subscriptionTypeId)
        {
            SubscriptionTypes subscriptionType;

            var unsubscribed = _candidateServiceProvider.TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType);

            if (!unsubscribed)
            {
                return GetMediatorResponse(UnsubscribeMediatorCodes.Unsubscribe.Error, UnsubscribePageMessages.FailedToUnsubscribe, UserMessageLevel.Error);
            }

            var mediatorCode = UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedNotSignedIn;

            if (!candidateId.HasValue)
            {
                return GetMediatorResponse(mediatorCode, UnsubscribePageMessages.Unsubscribed, UserMessageLevel.Success);                
            }

            switch (subscriptionType)
            {
                case SubscriptionTypes.SavedSearchAlertsViaEmail:
                    mediatorCode = UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowSavedSearchesSettings;
                    break;
                default:
                    mediatorCode = UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowAccountSettings;
                    break;
            }

            return GetMediatorResponse(mediatorCode, UnsubscribePageMessages.Unsubscribed, UserMessageLevel.Success);
        }
    }
}