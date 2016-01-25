namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;

    public class UnsubscribeStrategy : IUnsubscribeStrategy
    {
        private readonly ILogService _logger;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;

        public UnsubscribeStrategy(
            ILogService logger,
            ICandidateReadRepository candidateReadRepository,
            ISaveCandidateStrategy saveCandidateStrategy)
        {
            _logger = logger;
            _candidateReadRepository = candidateReadRepository;
            _saveCandidateStrategy = saveCandidateStrategy;
        }

        public bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType)
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
                        unsubscribed = UnsubscribeSavedSearchAlertsViaEmail(candidate);
                        break;

                    case SubscriptionTypes.MarketingViaEmail:
                        unsubscribed = UnsubscribeMarketingViaEmail(candidate);
                        break;
                }

                if (unsubscribed)
                {
                    _logger.Info("Unsubscribed subscriptionType='{0}' for subscriberId='{1}'", subscriptionType, subscriberId);                    
                }
                else
                {
                    _logger.Error("Failed to unsubscribe subscriptionType='{0}' for subscriberId='{1}'", subscriptionType, subscriberId);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Error unsubscribing subscriptionType='{0}' for subscriberId='{1}'", e, subscriptionType, subscriberId);
                unsubscribed = false;
            }

            return unsubscribed;
        }

        private bool UnsubscribeDailyDigestViaEmail(Candidate candidate)
        {
            candidate.CommunicationPreferences.ApplicationStatusChangePreferences.EnableEmail = false;
            candidate.CommunicationPreferences.ExpiringApplicationPreferences.EnableEmail = false;

            _saveCandidateStrategy.SaveCandidate(candidate);
            return true;
        }

        private bool UnsubscribeSavedSearchAlertsViaEmail(Candidate candidate)
        {
            candidate.CommunicationPreferences.SavedSearchPreferences.EnableEmail = false;

            _saveCandidateStrategy.SaveCandidate(candidate);
            return true;
        }

        private bool UnsubscribeMarketingViaEmail(Candidate candidate)
        {
            candidate.CommunicationPreferences.MarketingPreferences.EnableEmail = false;

            _saveCandidateStrategy.SaveCandidate(candidate);
            return true;
        }
    }
}