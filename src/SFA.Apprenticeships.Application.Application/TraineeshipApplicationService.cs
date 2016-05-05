namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Traineeships;

    public class TraineeshipApplicationService : ITraineeshipApplicationService
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;

        public TraineeshipApplicationService(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
        }

        public IEnumerable<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return _traineeshipApplicationReadRepository.GetApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _traineeshipApplicationReadRepository.GetApplicationCount(vacancyId);
        }

        public int GetNewApplicationCount(int vacancyId)
        {
            return _traineeshipApplicationReadRepository.GetNewApplicationCount(vacancyId);
        }      

        public int GetNewApplicationsCount(List<int> liveVacancyIds)
        {
            return _traineeshipApplicationReadRepository.GetNewApplicationsCount(liveVacancyIds);
        }

        public TraineeshipApplicationDetail GetApplication(Guid applicationId)
        {
            return _traineeshipApplicationReadRepository.Get(applicationId);
        }

        public TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            return _getApplicationForReviewStrategy.GetApplicationForReview(applicationId);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, notes);
        }        
    }
}
