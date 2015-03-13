namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ISavedSearchAlertRepository _savedSearchAlertRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;

        public CommunicationProcessor(
            IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository,
            IApplicationStatusAlertRepository applicationStatusAlertRepository,
            ISavedSearchAlertRepository savedSearchAlertRepository,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IMessageBus messageBus)
        {
            _expiringDraftRepository = expiringDraftRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            _savedSearchAlertRepository = savedSearchAlertRepository;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _messageBus = messageBus;
        }

        public void SendDailyDigests(Guid batchId)
        {
            var candidatesExpiringDrafts = _expiringDraftRepository.GetCandidatesDailyDigest();
            var candidatesApplicationStatusAlerts = _applicationStatusAlertRepository.GetCandidatesDailyDigest();

            var candidateIds = candidatesExpiringDrafts.Keys
                .Union(candidatesApplicationStatusAlerts.Keys);

            foreach (var candidateId in candidateIds)
            {
                var candidate = _candidateReadRepository.Get(candidateId);

                List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest;
                var candidateHasExpiringDrafts = candidatesExpiringDrafts.TryGetValue(candidateId, out candidateExpiringDraftsDailyDigest);

                List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest;
                var candidateHasApplicationStatusAlerts = candidatesApplicationStatusAlerts.TryGetValue(candidateId, out candidateApplicationStatusAlertsDailyDigest);

                if (ShouldCommunicateWithCandidate(candidate))
                {
                        var communicationRequest = CommunicationRequestFactory.GetDailyDigestCommunicationRequest(candidate, candidateExpiringDraftsDailyDigest, candidateApplicationStatusAlertsDailyDigest);

                        _messageBus.PublishMessage(communicationRequest);

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

        public void SendSavedSearchAlerts(Guid batchId)
        {
            var candidatesSavedSearchAlerts = _savedSearchAlertRepository.GetCandidatesSavedSearchAlerts();
            var candidateIds = candidatesSavedSearchAlerts.Keys;

            foreach (var candidateId in candidateIds)
            {
                var candidate = _candidateReadRepository.Get(candidateId);

                List<SavedSearchAlert> candidateSavedSearchAlerts;
                var candidateHasSavedSearchAlerts = candidatesSavedSearchAlerts.TryGetValue(candidateId, out candidateSavedSearchAlerts);

                if (ShouldCommunicateWithCandidate(candidate))
                {
                    if (candidateHasSavedSearchAlerts)
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
                }
                else
                {
                    if (candidateHasSavedSearchAlerts)
                    {
                        // Delete candidates saved search status alerts
                        candidateSavedSearchAlerts.ToList().ForEach(_savedSearchAlertRepository.Delete);
                    }
                }
            }
        }

        private bool ShouldCommunicateWithCandidate(Candidate candidate)
        {
            var user = _userReadRepository.Get(candidate.EntityId);

            return candidate.ShouldCommunicateWithCandidate() && user.IsActive();
        }
    }
}
