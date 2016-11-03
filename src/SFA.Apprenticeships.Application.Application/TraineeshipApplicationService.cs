namespace SFA.Apprenticeships.Application.Application
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Traineeships;
    using System;
    using System.Collections.Generic;
    using ISetApplicationStatusStrategy = Strategies.Traineeships.ISetApplicationStatusStrategy;

    public class TraineeshipApplicationService : ITraineeshipApplicationService
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationStatsRepository _traineeshipApplicationStatsRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;
        private readonly ISetApplicationStatusStrategy _setApplicationStatusStrategy;

        public TraineeshipApplicationService(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationStatsRepository traineeshipApplicationStatsRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy,
            ISetApplicationStatusStrategy setApplicationStatusStrategy)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationStatsRepository = traineeshipApplicationStatsRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
            _setApplicationStatusStrategy = setApplicationStatusStrategy;
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

        public void SetStateInProgress(Guid applicationId)
        {
            _setApplicationStatusStrategy.SetStateInProgress(applicationId);
        }

        public void SetStateSubmitted(Guid applicationId)
        {
            _setApplicationStatusStrategy.SetStateSubmitted(applicationId);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, notes, publishUpdate);
        }
    }
}
