namespace SFA.Apprenticeships.Application.Communications
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Interfaces.Communications;
    using Newtonsoft.Json;

    public static class CommunicationRequestFactory
    {
        public static CommunicationRequest GetDailyDigestCommunicationRequest(
            Candidate candidate,
            List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest,
            List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest)
        {
            var communicationTokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, candidate.RegistrationDetails.EmailAddress),
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
                new CommunicationToken(CommunicationTokens.CandidateSubscriberId, candidate.SubscriberId.ToString()),
                new CommunicationToken(CommunicationTokens.CandidateSubscriptionType, ((int)SubscriptionTypes.DailyDigestViaEmail).ToString(CultureInfo.InvariantCulture))
            };

            var draftsJson = candidateExpiringDraftsDailyDigest == null
                ? string.Empty
                : JsonConvert.SerializeObject(candidateExpiringDraftsDailyDigest.OrderBy(d => d.ClosingDate));

            communicationTokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, draftsJson));

            var alertsJson = candidateApplicationStatusAlertsDailyDigest == null
                ? string.Empty
                : JsonConvert.SerializeObject(candidateApplicationStatusAlertsDailyDigest.OrderBy(d => d.DateUpdated));

            communicationTokens.Add(new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, alertsJson));

            return new CommunicationRequest
            {
                EntityId = candidate.EntityId,
                MessageType = MessageTypes.DailyDigest,
                Tokens = communicationTokens
            };
        }

        public static CommunicationRequest GetSavedSearchAlertCommunicationRequest(
            Candidate candidate,
            List<SavedSearchAlert> candidateSavedSearchAlerts)
        {
            // Order by most recently added saved search alert.
            var sortedCandidateSavedSearchAlerts = candidateSavedSearchAlerts
                .OrderByDescending(each => each.DateCreated)
                .ToList();

            foreach (var candidateSavedSearchAlert in sortedCandidateSavedSearchAlerts)
            {
                // Order by most recently posted vacancy.
                candidateSavedSearchAlert.Results = candidateSavedSearchAlert.Results
                    .OrderByDescending(each => each.PostedDate)
                    .ToList();
            }

            var savedSearchAlertsJson = JsonConvert.SerializeObject(sortedCandidateSavedSearchAlerts);

            var communicationTokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, candidate.RegistrationDetails.EmailAddress),
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
                new CommunicationToken(CommunicationTokens.CandidateSubscriberId, candidate.SubscriberId.ToString()),
                new CommunicationToken(CommunicationTokens.CandidateSubscriptionType, ((int)SubscriptionTypes.SavedSearchAlertsViaEmail).ToString(CultureInfo.InvariantCulture)),
                new CommunicationToken(CommunicationTokens.SavedSearchAlerts, savedSearchAlertsJson)
            };

            return new CommunicationRequest
            {
                EntityId = candidate.EntityId,
                MessageType = MessageTypes.SavedSearchAlert,
                Tokens = communicationTokens
            };
        }
    }
}