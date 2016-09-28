namespace SFA.Apprenticeships.Application.Application
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies;
    using Strategies.Apprenticeships;
    using System;
    using System.Collections.Generic;

    public class ApprenticeshipApplicationService : IApprenticeshipApplicationService
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationStatsRepository _apprenticeshipApplicationStatsRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;
        private readonly ISetApplicationStatusStrategy _setApplicationStatusStrategy;

        public ApprenticeshipApplicationService(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationStatsRepository apprenticeshipApplicationStatsRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy,
            ISetApplicationStatusStrategy setApplicationStatusStrategy)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationStatsRepository = apprenticeshipApplicationStatsRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
            _setApplicationStatusStrategy = setApplicationStatusStrategy;
        }

        public IEnumerable<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetSubmittedApplicationSummaries(vacancyId);
        }

        public IEnumerable<ApprenticeshipApplicationSummary> GetApplicationSummaries(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationStatsRepository.GetCountsForVacancyIds(new[] { vacancyId })[vacancyId].AllApplications;
        }

        public ApprenticeshipApplicationDetail GetApplication(Guid applicationId)
        {
            return _apprenticeshipApplicationReadRepository.Get(applicationId);
        }

        public ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            return _getApplicationForReviewStrategy.GetApplicationForReview(applicationId);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, notes);
        }

        public void SetSuccessfulDecision(Guid applicationId)
        {
            _setApplicationStatusStrategy.SetSuccessfulDecision(applicationId);
        }

        public void SetUnsuccessfulDecision(Guid applicationId)
        {
            _setApplicationStatusStrategy.SetUnsuccessfulDecision(applicationId);
        }

        public void RevertToInProgress(Guid applicationId)
        {
            _setApplicationStatusStrategy.RevertToInProgress(applicationId);
        }

        public IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return _apprenticeshipApplicationStatsRepository.GetCountsForVacancyIds(vacancyIds);
        }
    }
}
