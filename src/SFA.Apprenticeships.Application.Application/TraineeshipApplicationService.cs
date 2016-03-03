namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using System.Collections.Generic;
    using Applications.Entities;
    using Applications.Strategies;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Traineeships;

    public class TraineeshipApplicationService : ITraineeshipApplicationService
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;

        public TraineeshipApplicationService(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
        }

        public IList<TraineeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return new List<TraineeshipApplicationSummary>();
            //return _traineeshipApplicationReadRepository.GetSubmittedApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return 0;
            //return _traineeshipApplicationReadRepository.GetApplicationCount(vacancyId);
        }

        public int GetNewApplicationCount(int vacancyId)
        {
            return 0;
            //return _traineeshipApplicationReadRepository.GetNewApplicationCount(vacancyId);
        }

        public TraineeshipApplicationDetail GetApplication(Guid applicationId)
        {
            return _traineeshipApplicationReadRepository.Get(applicationId);
        }

        public TraineeshipApplicationDetail GetApplicationForReview(Guid applicationId)
        {
            return null;
            //return _getApplicationForReviewStrategy.GetApplicationForReview(applicationId);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, notes);
        }

        public void SetSuccessfulDecision(Guid applicationId)
        {
            SetDecision(applicationId, ApplicationStatuses.Successful);
        }

        public void SetUnsuccessfulDecision(Guid applicationId)
        {
            SetDecision(applicationId, ApplicationStatuses.Unsuccessful);
        }

        #region Helpers

        private void SetDecision(Guid applicationId, ApplicationStatuses applicationStatus)
        {
            var traineeshipApplication = GetApplication(applicationId);
            var nextLegacyApplicationId = _referenceNumberRepository.GetNextLegacyApplicationId();

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                // CRITICAL: make the update look like it came from legacy AVMS application
                ApplicationId = Guid.Empty,
                ApplicationStatus = applicationStatus,
                LegacyApplicationId = nextLegacyApplicationId,
                LegacyCandidateId = 0, // not required
                LegacyVacancyId = 0, // not required
                VacancyStatus = traineeshipApplication.VacancyStatus,
                ClosingDate = traineeshipApplication.Vacancy.ClosingDate
            };

            _applicationStatusUpdateStrategy.Update(traineeshipApplication, applicationStatusSummary);
        }

        #endregion
    }
}
