// TODO: AG: remove deadcode.

/*
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

    public class CandidateApplicationStatusAlertCommunicationCommand : CandidateCommunicationCommand
    {
        private List<ApplicationStatusAlert> _applicationStatusAlerts;

        public CandidateApplicationStatusAlertCommunicationCommand(
            IMessageBus messageBus,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
            : base(messageBus, candidateReadRepository, userReadRepository)
        {
        }

        public override bool CanHandle(CommunicationRequest communicationRequest)
        {
            if (communicationRequest.MessageType != MessageTypes.DailyDigest)
            {
                return false;
            }

            var json = communicationRequest.GetToken(CommunicationTokens.ApplicationStatusAlerts);

            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            _applicationStatusAlerts = JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(json);

            return true;
        }

        protected override void QueueSmsMessages(CommunicationRequest communicationRequest)
        {
            var candidateMobileNumber = communicationRequest.GetToken(CommunicationTokens.CandidateMobileNumber);

            QueueApplicationStatusAlertSmsMessages(candidateMobileNumber);
        }

        private void QueueApplicationStatusAlertSmsMessages(string candidateMobileNumber)
        {
            var successfulApplicationStatusAlerts = _applicationStatusAlerts
                .Where(each => each.Status == ApplicationStatuses.Successful)
                .ToArray();

            foreach (var applicationStatusAlert in successfulApplicationStatusAlerts)
            {
                QueueApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationSuccessful, candidateMobileNumber, applicationStatusAlert);
            }

            var otherApplicationStatusAlerts = _applicationStatusAlerts
                .Where(each => each.Status != ApplicationStatuses.Successful)
                .ToArray();

            if (otherApplicationStatusAlerts.Length == 1)
            {
                QueueSummaryApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationStatusAlert, candidateMobileNumber, otherApplicationStatusAlerts);
            }
            else if (otherApplicationStatusAlerts.Length > 1)
            {
                QueueSummaryApplicationStatusAlertSmsMessage(
                    MessageTypes.ApprenticeshipApplicationStatusAlertsSummary, candidateMobileNumber, otherApplicationStatusAlerts);
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
*/
