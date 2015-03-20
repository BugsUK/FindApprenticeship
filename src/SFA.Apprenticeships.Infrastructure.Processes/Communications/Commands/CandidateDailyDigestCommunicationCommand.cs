namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Newtonsoft.Json;

    public class CandidateDailyDigestCommunicationCommand : CandidateCommunicationCommand
    {
        public CandidateDailyDigestCommunicationCommand(
            IMessageBus messageBus,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
            : base(messageBus, candidateReadRepository, userReadRepository)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType == MessageTypes.DailyDigest;
        }

        protected override void QueueSmsMessages(CommunicationRequest communicationRequest)
        {
            var candidateMobileNumber = communicationRequest.GetToken(CommunicationTokens.CandidateMobileNumber);

            // Expiring drafts.
            var expiringDraftsJson = communicationRequest.GetToken(CommunicationTokens.ExpiringDrafts);
            var expiringDrafts = string.IsNullOrWhiteSpace(expiringDraftsJson) ? new List<ExpiringApprenticeshipApplicationDraft>() : JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(expiringDraftsJson);

            QueueExpiringDraftSmsMessages(candidateMobileNumber, expiringDrafts);

            // Application status alerts.
            var applicationStatusAlertsJson = communicationRequest.GetToken(CommunicationTokens.ApplicationStatusAlerts);
            var applicationStatusAlerts = string.IsNullOrWhiteSpace(applicationStatusAlertsJson) ? new List<ApplicationStatusAlert>() : JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(applicationStatusAlertsJson);

            QueueApplicationStatusAlertSmsMessages(candidateMobileNumber, applicationStatusAlerts);
        }

        private void QueueExpiringDraftSmsMessages(
            string candidateMobileNumber, List<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            if (expiringDrafts.Count == 1)
            {
                QueueApplicationExpiringDraftSmsMessage(candidateMobileNumber, expiringDrafts.First());
            }
            else if (expiringDrafts.Count > 1)
            {
                QueueApplicationExpiringDraftsSummarySmsMessage(candidateMobileNumber, expiringDrafts);
            }
        }

        private void QueueApplicationExpiringDraftSmsMessage(
            string candidateMobileNumber, ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            QueueSmsMessage(new CommunicationRequest
            {
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraft,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidateMobileNumber),
                        new CommunicationToken(CommunicationTokens.ExpiringDraft, JsonConvert.SerializeObject(expiringDraft))
                    }
            });
        }

        private void QueueApplicationExpiringDraftsSummarySmsMessage(
            string candidateMobileNumber, IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            QueueSmsMessage(new CommunicationRequest
            {
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidateMobileNumber),
                        new CommunicationToken(CommunicationTokens.ExpiringDrafts, JsonConvert.SerializeObject(expiringDrafts))
                    }
            });
        }

        private void QueueApplicationStatusAlertSmsMessages(
            string candidateMobileNumber, List<ApplicationStatusAlert> applicationStatusAlerts)
        {
            // Successful application status alerts.
            var successfulApplicationStatusAlerts = applicationStatusAlerts
                .Where(each => each.Status == ApplicationStatuses.Successful)
                .ToArray();

            foreach (var applicationStatusAlert in successfulApplicationStatusAlerts)
            {
                QueueApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationSuccessful, candidateMobileNumber, applicationStatusAlert);
            }

            // Other (unsuccessful) application status alerts.
            var otherApplicationStatusAlerts = applicationStatusAlerts
                .Where(each => each.Status != ApplicationStatuses.Successful)
                .ToArray();

            if (otherApplicationStatusAlerts.Length == 1)
            {
                QueueApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationUnsuccessful, candidateMobileNumber, otherApplicationStatusAlerts.First());
            }
            else if (otherApplicationStatusAlerts.Length > 1)
            {
                QueueSummaryApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, candidateMobileNumber, otherApplicationStatusAlerts);
            }
        }

        private void QueueSummaryApplicationStatusAlertSmsMessage(
            MessageTypes messageType, string candidateMobileNumber, IEnumerable<ApplicationStatusAlert> applicationStatusAlerts)
        {
            QueueSmsMessage(new CommunicationRequest
            {
                MessageType = messageType,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidateMobileNumber),
                        new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, JsonConvert.SerializeObject(applicationStatusAlerts))
                    }
            });
        }

        private void QueueApplicationStatusAlertSmsMessage(
            MessageTypes messageType, string candidateMobileNumber, ApplicationStatusAlert applicationStatusAlert)
        {
            QueueSmsMessage(new CommunicationRequest
            {
                MessageType = messageType,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, candidateMobileNumber),
                        new CommunicationToken(CommunicationTokens.ApplicationStatusAlert, JsonConvert.SerializeObject(applicationStatusAlert))
                    }
            });
        }
    }
}