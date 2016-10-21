namespace SFA.Apprenticeships.Application.Application
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Traineeships;
    using System;
    using System.Collections.Generic;

    public class TraineeshipApplicationService : ITraineeshipApplicationService
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationStatsRepository _traineeshipApplicationStatsRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;

        public TraineeshipApplicationService(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationStatsRepository traineeshipApplicationStatsRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationStatsRepository = traineeshipApplicationStatsRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
        }

        public IEnumerable<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return _traineeshipApplicationReadRepository.GetApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _traineeshipApplicationStatsRepository.GetCountsForVacancyIds(new int[] { vacancyId })[vacancyId].AllApplications;
        }

        public IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return _traineeshipApplicationStatsRepository.GetCountsForVacancyIds(vacancyIds);
        }

        public TraineeshipApplicationDetail GetApplication(Guid applicationId)
        {
            return _traineeshipApplicationReadRepository.Get(applicationId);
        }

        public TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            return _getApplicationForReviewStrategy.GetApplicationForReview(applicationId);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, notes, publishUpdate);
        }
    }
}
