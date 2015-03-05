namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public CommunicationProcessor(IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository, 
            IApplicationStatusAlertRepository applicationStatusAlertRepository,
            ICandidateReadRepository candidateReadRepository, 
            IMessageBus messageBus)
        {
            _expiringDraftRepository = expiringDraftRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
        }

        public void SendDailyCommunications(Guid batchId)
        {
            var candidatesExpiringDrafts = _expiringDraftRepository.GetCandidatesDailyDigest();
            var candidatesApplicationStatusAlerts = _applicationStatusAlertRepository.GetCandidatesDailyDigest();
            //var candidatesSavedSearchAlerts = _savedSearchAlertRepository.GetCandidatesDailyDigest();

            var candidateIds = candidatesExpiringDrafts.Keys.Union(candidatesApplicationStatusAlerts.Keys);

            foreach (var candidateId in candidateIds)
            {
                var candidate = _candidateReadRepository.Get(candidateId);

                List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest;
                var candidateHasExpiringDrafts = candidatesExpiringDrafts.TryGetValue(candidateId, out candidateExpiringDraftsDailyDigest);

                List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest;
                var candidateHasApplicationStatusAlerts = candidatesApplicationStatusAlerts.TryGetValue(candidateId, out candidateApplicationStatusAlertsDailyDigest);

                if (candidate.CommunicationPreferences.AllowEmail || candidate.CommunicationPreferences.AllowMobile)
                {
                    var communicationMessage = CommunicationRequestFactory.GetCommunicationMessage(candidate, candidateExpiringDraftsDailyDigest, candidateApplicationStatusAlertsDailyDigest);
                    _messageBus.PublishMessage(communicationMessage);

                    if (candidateHasExpiringDrafts)
                    {
                        // Update candidates expiring drafts to sent
                        candidateExpiringDraftsDailyDigest.ToList().ForEach(dd =>
                        {
                            dd.BatchId = batchId;
                            _expiringDraftRepository.Save(dd);
                        });
                    }

                    if (candidateHasApplicationStatusAlerts)
                    {
                        // Update candidates application status alerts to sent
                        candidateApplicationStatusAlertsDailyDigest.ToList().ForEach(dd =>
                        {
                            dd.BatchId = batchId;
                            _applicationStatusAlertRepository.Save(dd);
                        });
                    }
                }
                else
                {
                    if (candidateHasExpiringDrafts)
                    {
                        // Delete candidates expiring drafts
                        candidateExpiringDraftsDailyDigest.ToList().ForEach(_expiringDraftRepository.Delete);
                    }
                    if (candidateHasApplicationStatusAlerts)
                    {
                        // Delete candidates application status alerts
                        candidateApplicationStatusAlertsDailyDigest.ToList().ForEach(_applicationStatusAlertRepository.Delete);
                    }
                }
            }
        }
    }
}
