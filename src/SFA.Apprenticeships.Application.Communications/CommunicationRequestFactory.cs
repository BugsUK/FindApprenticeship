namespace SFA.Apprenticeships.Application.Communications
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Interfaces.Communications;
    using Newtonsoft.Json;

    public class CommunicationRequestFactory
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
            var communicationTokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, candidate.RegistrationDetails.EmailAddress),
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
            };

            // TODO: AG: ordering of saved search alerts.
            var savedSearchAlertsJson = candidateSavedSearchAlerts == null
                ? string.Empty
                : JsonConvert.SerializeObject(candidateSavedSearchAlerts);

            communicationTokens.Add(new CommunicationToken(CommunicationTokens.SavedSearchAlerts, savedSearchAlertsJson));

            return new CommunicationRequest
            {
                EntityId = candidate.EntityId,
                MessageType = MessageTypes.SavedSearchAlert,
                Tokens = communicationTokens
            };
        }
    }
}
