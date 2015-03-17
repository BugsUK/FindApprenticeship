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

    public class SendDailyDigestsStrategy : ISendDailyDigestsStrategy
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;

        public SendDailyDigestsStrategy(
            IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository,
            IApplicationStatusAlertRepository applicationStatusAlertRepository,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IMessageBus messageBus)
        {
            _expiringDraftRepository = expiringDraftRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
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
                var user = _userReadRepository.Get(candidate.EntityId);

                List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest;
                var candidateHasExpiringDrafts = candidatesExpiringDrafts.TryGetValue(candidateId, out candidateExpiringDraftsDailyDigest);

                List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest;
                var candidateHasApplicationStatusAlerts = candidatesApplicationStatusAlerts.TryGetValue(candidateId, out candidateApplicationStatusAlertsDailyDigest);

                if (candidate.ShouldCommunicateWithCandidate() && user.IsActive())
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
    }
}
