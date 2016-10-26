namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Linq;
    using Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using UserAccount.Entities;

    public class SaveCandidateStrategy : ISaveCandidateStrategy
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IServiceBus _serviceBus;

        public SaveCandidateStrategy(ICandidateWriteRepository candidateWriteRepository,
            IGetCandidateApprenticeshipApplicationsStrategy getCandidateApplicationsStrategy,
            ICandidateReadRepository candidateReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ILogService logger, IServiceBus serviceBus)
        {
            _candidateWriteRepository = candidateWriteRepository;
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            _candidateReadRepository = candidateReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            var result = _candidateWriteRepository.Save(candidate);
            _serviceBus.PublishMessage(new CandidateUserUpdate(candidate.EntityId, CandidateUserUpdateType.Update));
            var reloadedCandidate = _candidateReadRepository.Get(candidate.EntityId);

            var candidateApplications = _getCandidateApplicationsStrategy
                .GetApplications(candidate.EntityId)
                .Where(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved)
                .ToList();

            candidateApplications.ForEach(candidateApplication =>
            {
                try
                {
                    UpdateApprenticeshipApplicationDetail(reloadedCandidate, candidateApplication.LegacyVacancyId);
                }
                catch (Exception e)
                {
                    // try updating the next one
                    var message = string.Format(
                        "Error while updating a draft application with the updated user personal details for user {0}",
                        candidate.EntityId);
                    _logger.Warn(message, e);
                }
            });

            return result;
        }

        private void UpdateApprenticeshipApplicationDetail(Candidate candidate, int vacancyId)
        {
            var apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, vacancyId);

            if (apprenticeshipApplicationDetail != null)
            {
                apprenticeshipApplicationDetail.CandidateDetails = candidate.RegistrationDetails;
                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
            }
        }
    }
}
