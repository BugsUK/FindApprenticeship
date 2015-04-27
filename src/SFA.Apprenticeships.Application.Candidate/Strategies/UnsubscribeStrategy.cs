namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using SavedSearches;

    public class UnsubscribeStrategy : IUnsubscribeStrategy
    {
        private readonly ILogService _logger;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly IRetrieveSavedSearchesStrategy _retrieveSavedSearchesStrategy;
        private readonly IUpdateSavedSearchStrategy _updateSavedSearchStrategy;

        public UnsubscribeStrategy(
            ILogService logger,
            ICandidateReadRepository candidateReadRepository,
            ISaveCandidateStrategy saveCandidateStrategy,
            IRetrieveSavedSearchesStrategy retrieveSavedSearchesStrategy,
            IUpdateSavedSearchStrategy updateSavedSearchStrategy)
        {
            _logger = logger;
            _candidateReadRepository = candidateReadRepository;
            _saveCandidateStrategy = saveCandidateStrategy;
            _retrieveSavedSearchesStrategy = retrieveSavedSearchesStrategy;
            _updateSavedSearchStrategy = updateSavedSearchStrategy;
        }

        public bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType, string subscriptionItemId = null)
        {
            var unsubscribed = false;

            try
            {
                var candidate = _candidateReadRepository.GetBySubscriberId(subscriberId);

                switch (subscriptionType)
                {
                    case SubscriptionTypes.DailyDigestViaEmail:
                        unsubscribed = UnsubscribeDailyDigestViaEmail(candidate);
                        break;

                    case SubscriptionTypes.SavedSearchAlertsViaEmail:
                        unsubscribed = UnsubscribeSavedSearchAlertsViaEmail(candidate, subscriptionItemId);
                        break;
                }

                if (unsubscribed)
                {
                    _logger.Info("Unsubscribed subscriptionType='{0}' for subscriberId='{1}', subscriptionItemId='{2}' (optional)",
                        subscriptionType, subscriberId, subscriptionItemId);                    
                }
                else
                {
                    _logger.Error("Failed to unsubscribe subscriptionType='{0}' for subscriberId='{1}', subscriptionItemId='{2}' (optional)",
                        subscriptionType, subscriberId, subscriptionItemId);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Error unsubscribing subscriptionType='{0}' for subscriberId='{0}', subscriptionItemId='{2}' (optional)",
                    e, subscriptionType, subscriberId, subscriptionItemId);

                unsubscribed = false;
            }

            return unsubscribed;
        }

        private bool UnsubscribeDailyDigestViaEmail(Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            // TODO: AG: US733: remove 'dead' field(s).
            communicationPreferences.SendApplicationStatusChanges =
                communicationPreferences.SendApplicationStatusChangesViaEmail =
                    communicationPreferences.SendApprenticeshipApplicationsExpiring =
                        communicationPreferences.SendApprenticeshipApplicationsExpiringViaEmail = false;

            _saveCandidateStrategy.SaveCandidate(candidate);
            return true;
        }

        private bool UnsubscribeSavedSearchAlertsViaEmail(Candidate candidate, string subscriptionItemId)
        {
            var savedSearchId = Guid.Parse(subscriptionItemId);

            var savedSearches = _retrieveSavedSearchesStrategy.RetrieveSavedSearches(candidate.EntityId);
            var savedSearch = savedSearches.SingleOrDefault(each => each.EntityId == savedSearchId);

            if (savedSearch == null)
            {
                return false;
            }

            // TODO: AG: US733: remove 'dead' field(s).
            savedSearch.AlertsEnabled =
                savedSearch.AlertsEnabledViaEmail = false;

            _updateSavedSearchStrategy.UpdateSavedSearch(savedSearch);
            return true;
        }
    }
}