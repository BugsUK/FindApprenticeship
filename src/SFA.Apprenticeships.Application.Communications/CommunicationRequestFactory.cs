namespace SFA.Apprenticeships.Application.Communications
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Interfaces.Communications;
    using Newtonsoft.Json;

    public class CommunicationRequestFactory
    {
        public static CommunicationRequest GetCommunicationMessage(Candidate candidate, List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest, List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest)
        {
            var communicationMessage = new CommunicationRequest
            {
                EntityId = candidate.EntityId,
                MessageType = MessageTypes.DailyDigest
            };

            var candidateDailyDigestCount = candidateExpiringDraftsDailyDigest == null ? 0 : candidateExpiringDraftsDailyDigest.Count();

            var commTokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, candidate.RegistrationDetails.EmailAddress),
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber),
            };

            var draftsJson = candidateExpiringDraftsDailyDigest == null ? string.Empty : JsonConvert.SerializeObject(candidateExpiringDraftsDailyDigest.OrderBy(d => d.ClosingDate));
            commTokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, draftsJson));

            var alertsJson = candidateApplicationStatusAlertsDailyDigest == null ? string.Empty : JsonConvert.SerializeObject(candidateApplicationStatusAlertsDailyDigest.OrderBy(d => d.DateUpdated));
            commTokens.Add(new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, alertsJson));

            communicationMessage.Tokens = commTokens;

            return communicationMessage;
        } 
    }
}
