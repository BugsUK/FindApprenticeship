namespace SFA.Apprenticeships.Application.Communications.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class SendSavedSearchAlertsStrategy : ISendSavedSearchAlertsStrategy
    {
        private readonly ISavedSearchAlertRepository _savedSearchAlertRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;

        public SendSavedSearchAlertsStrategy(
            ISavedSearchAlertRepository savedSearchAlertRepository,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IMessageBus messageBus)
        {
            _savedSearchAlertRepository = savedSearchAlertRepository;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _messageBus = messageBus;
        }

        public void SendSavedSearchAlerts(Guid batchId)
        {
            var allCandidatesSavedSearchAlerts = _savedSearchAlertRepository.GetCandidatesSavedSearchAlerts();

            foreach (var candidateId in allCandidatesSavedSearchAlerts.Keys)
            {
                var candidate = _candidateReadRepository.Get(candidateId);
                var user = _userReadRepository.Get(candidateId);

                var candidateSavedSearchAlerts = allCandidatesSavedSearchAlerts[candidateId];

                if (candidate.AllowsCommunication() && user.IsActive())
                {
                    var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(candidate, candidateSavedSearchAlerts);

                    _messageBus.PublishMessage(communicationRequest);

                    // Update candidates saved search alerts to sent
                    candidateSavedSearchAlerts.ToList().ForEach(dd =>
                    {
                        dd.BatchId = batchId;
                        _savedSearchAlertRepository.Save(dd);
                    });
                }
                else
                {
                    // Delete candidates saved search status alerts
                    candidateSavedSearchAlerts.ToList().ForEach(_savedSearchAlertRepository.Delete);
                }
            }
        }
    }
}