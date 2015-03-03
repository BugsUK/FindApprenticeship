namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public CommunicationProcessor(IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository, 
            ICandidateReadRepository candidateReadRepository, 
            IMessageBus messageBus)
        {
            _expiringDraftRepository = expiringDraftRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
        }

        public void SendDailyCommunications(Guid batchId)
        {
            //todo: 1.7: this should also accommodate application status alerts. needs discussion as must queue 2 comm request messages to cover 3 message types
            var candidatesExpiringDrafts = _expiringDraftRepository.GetCandidatesDailyDigest();
            //var candidatesApplicationStatusAlerts = _applicationStatusAlertRepository.GetCandidatesDailyDigest();
            //var candidatesSavedSearchAlerts = _savedSearchAlertRepository.GetCandidatesDailyDigest();

            foreach (var candidateDailyDigest in candidatesExpiringDrafts)
            {
                var candidate = _candidateReadRepository.Get(candidateDailyDigest.Key);

                if (candidate.CommunicationPreferences.AllowEmail || candidate.CommunicationPreferences.AllowMobile)
                {
                    var communicationMessage = CommunicationRequestFactory.GetCommunicationMessage(candidate, candidateDailyDigest.Value);
                    _messageBus.PublishMessage(communicationMessage);

                    // Update candidates expiring drafts to sent
                    candidateDailyDigest.Value.ToList().ForEach(dd =>
                    {
                        dd.BatchId = batchId;
                        _expiringDraftRepository.Save(dd);
                    });
                }
                else
                {
                    // Delete candidates expiring drafts
                    candidateDailyDigest.Value.ToList().ForEach(_expiringDraftRepository.Delete);
                }
            }
        }
    }
}
