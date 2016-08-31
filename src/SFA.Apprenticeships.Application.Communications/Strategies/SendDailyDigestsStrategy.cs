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

    using SFA.Apprenticeships.Application.Interfaces;

    public class SendDailyDigestsStrategy : ISendDailyDigestsStrategy
    {
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IServiceBus _serviceBus;
        private readonly ILogService _logService;

        public SendDailyDigestsStrategy(
            ILogService logService,
            IServiceBus serviceBus,
            IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository,
            IApplicationStatusAlertRepository applicationStatusAlertRepository,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository)
        {
            _logService = logService;
            _serviceBus = serviceBus;
            _expiringDraftRepository = expiringDraftRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
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
                var user = _userReadRepository.Get(candidateId);

                if (candidate == null || user == null)
                {
                    _logService.Warn("Could not find a valid user or candidate for id: {0}. User is null: {1}, Candidate is null: {2}", candidateId, user == null, candidate == null);
                    return;
                }

                List<ExpiringApprenticeshipApplicationDraft> candidateExpiringDraftsDailyDigest;
                var candidateHasExpiringDrafts = candidatesExpiringDrafts.TryGetValue(candidateId, out candidateExpiringDraftsDailyDigest);

                List<ApplicationStatusAlert> candidateApplicationStatusAlertsDailyDigest;
                var candidateHasApplicationStatusAlerts = candidatesApplicationStatusAlerts.TryGetValue(candidateId, out candidateApplicationStatusAlertsDailyDigest);

                if (candidate.AllowsCommunication() && user.IsActive())
                {
                    var communicationRequest = CommunicationRequestFactory.GetDailyDigestCommunicationRequest(candidate, candidateExpiringDraftsDailyDigest, candidateApplicationStatusAlertsDailyDigest);

                    _serviceBus.PublishMessage(communicationRequest);

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
                        // Soft delete candidates expiring drafts by setting batch id to empty
                        candidateExpiringDraftsDailyDigest.ToList().ForEach(dd =>
                        {
                            dd.BatchId = Guid.Empty;
                            _expiringDraftRepository.Save(dd);
                        });
                    }

                    if (candidateHasApplicationStatusAlerts)
                    {
                        // Soft delete candidates saved application status alerts by setting batch id to empty
                        candidateApplicationStatusAlertsDailyDigest.ToList().ForEach(dd =>
                        {
                            dd.BatchId = Guid.Empty;
                            _applicationStatusAlertRepository.Save(dd);
                        });
                    }
                }
            }
        }
    }
}
