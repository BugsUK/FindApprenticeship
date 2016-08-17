namespace SFA.Apprenticeships.Infrastructure.Processes.Communications.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Newtonsoft.Json;

    using SFA.Apprenticeships.Application.Interfaces;

    public class CandidateDailyDigestCommunicationCommand : CandidateCommunicationCommand
    {
        public CandidateDailyDigestCommunicationCommand(
            ILogService logService,
            IConfigurationService configurationService,
            IServiceBus serviceBus,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
            : base(logService, configurationService, serviceBus, candidateReadRepository, userReadRepository)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            return communicationRequest.MessageType == MessageTypes.DailyDigest;
        }

        protected override void HandleSmsMessage(Candidate candidate, CommunicationRequest communicationRequest)
        {
            var mobileNumber = communicationRequest.GetToken(CommunicationTokens.CandidateMobileNumber);

            HandleExpiringDraftSmsMessages(candidate, mobileNumber, communicationRequest);
            HandleApplicationStatusAlertSmsMessages(candidate, mobileNumber, communicationRequest);
        }

        #region Helpers

        private void HandleExpiringDraftSmsMessages(
            Candidate candidate, string mobileNumber, CommunicationRequest communicationRequest)
        {
            var expiringDraftsJson = communicationRequest.GetToken(CommunicationTokens.ExpiringDrafts);
            var expiringDrafts = string.IsNullOrWhiteSpace(expiringDraftsJson)
                ? new List<ExpiringApprenticeshipApplicationDraft>()
                : JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(expiringDraftsJson);

            if (expiringDrafts.Count == 1)
            {
                QueueApplicationExpiringDraftSmsMessage(candidate, mobileNumber, expiringDrafts.First());
            }
            else if (expiringDrafts.Count > 1)
            {
                QueueApplicationExpiringDraftsSummarySmsMessage(candidate, mobileNumber, expiringDrafts);
            }
        }

        private void HandleApplicationStatusAlertSmsMessages(
            Candidate candidate,  string mobileNumber, CommunicationRequest communicationRequest)
        {
            var applicationStatusAlertsJson = communicationRequest.GetToken(CommunicationTokens.ApplicationStatusAlerts);

            var applicationStatusAlerts = string.IsNullOrWhiteSpace(applicationStatusAlertsJson)
                ? new List<ApplicationStatusAlert>()
                : JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(applicationStatusAlertsJson);

            QueueApplicationStatusAlertSmsMessages(candidate, mobileNumber, applicationStatusAlerts);
        }

        private void QueueApplicationExpiringDraftSmsMessage(
            Candidate candidate, string mobileNumber, ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            base.HandleSmsMessage(candidate, new CommunicationRequest
            {
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraft,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, mobileNumber),
                        new CommunicationToken(CommunicationTokens.ExpiringDraft, JsonConvert.SerializeObject(expiringDraft))
                    }
            });
        }

        private void QueueApplicationExpiringDraftsSummarySmsMessage(
            Candidate candidate, string mobileNumber, IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            base.HandleSmsMessage(candidate, new CommunicationRequest
            {
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, mobileNumber),
                        new CommunicationToken(CommunicationTokens.ExpiringDrafts, JsonConvert.SerializeObject(expiringDrafts))
                    }
            });
        }

        private void QueueApplicationStatusAlertSmsMessages(
            Candidate candidate, string mobileNumber, List<ApplicationStatusAlert> applicationStatusAlerts)
        {
            // Successful application status alerts.
            var successfulApplicationStatusAlerts = applicationStatusAlerts
                .Where(each => each.Status == ApplicationStatuses.Successful)
                .ToArray();

            foreach (var applicationStatusAlert in successfulApplicationStatusAlerts)
            {
                QueueApplicationStatusAlertSmsMessage(
                    candidate, mobileNumber, MessageTypes.ApprenticeshipApplicationSuccessful, applicationStatusAlert);
            }

            // Other (unsuccessful) application status alerts.
            var otherApplicationStatusAlerts = applicationStatusAlerts
                .Where(each => each.Status != ApplicationStatuses.Successful)
                .ToArray();

            if (otherApplicationStatusAlerts.Length == 1)
            {
                QueueApplicationStatusAlertSmsMessage(
                    candidate, mobileNumber, MessageTypes.ApprenticeshipApplicationUnsuccessful, otherApplicationStatusAlerts.First());
            }
            else if (otherApplicationStatusAlerts.Length > 1)
            {
                QueueSummaryApplicationStatusAlertSmsMessage(
                    candidate, mobileNumber, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, otherApplicationStatusAlerts);
            }
        }

        private void QueueSummaryApplicationStatusAlertSmsMessage(
            Candidate candidate, string mobileNumber, MessageTypes messageType, IEnumerable<ApplicationStatusAlert> applicationStatusAlerts)
        {
            base.HandleSmsMessage(candidate, new CommunicationRequest
            {
                MessageType = messageType,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, mobileNumber),
                        new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, JsonConvert.SerializeObject(applicationStatusAlerts))
                    }
            });
        }

        private void QueueApplicationStatusAlertSmsMessage(
            Candidate candidate, string mobileNumber, MessageTypes messageType, ApplicationStatusAlert applicationStatusAlert)
        {
            base.HandleSmsMessage(candidate, new CommunicationRequest
            {
                MessageType = messageType,
                Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, mobileNumber),
                        new CommunicationToken(CommunicationTokens.ApplicationStatusAlert, JsonConvert.SerializeObject(applicationStatusAlert))
                    }
            });
        }

        #endregion
    }
}