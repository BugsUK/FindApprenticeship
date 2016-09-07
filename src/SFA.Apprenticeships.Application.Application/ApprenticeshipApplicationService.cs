namespace SFA.Apprenticeships.Application.Application
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces.Applications;
    using Strategies;
    using Strategies.Apprenticeships;
    using System;
    using System.Collections.Generic;

    public class ApprenticeshipApplicationService : IApprenticeshipApplicationService
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;

        public ApprenticeshipApplicationService(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
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
            return _apprenticeshipApplicationReadRepository.GetCountsForVacancyIds(new[] { vacancyId })[vacancyId].AllApplications;
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
            SetDecision(applicationId, ApplicationStatuses.Successful);
        }

        public void SetUnsuccessfulDecision(Guid applicationId)
        {
            SetDecision(applicationId, ApplicationStatuses.Unsuccessful);
        }

        public IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return _apprenticeshipApplicationReadRepository.GetCountsForVacancyIds(vacancyIds);
        }

        #region Helpers

        private void SetDecision(Guid applicationId, ApplicationStatuses applicationStatus)
        {
            var apprenticeshipApplication = GetApplication(applicationId);
            var legacyApplicationId = apprenticeshipApplication.LegacyApplicationId;
            if (legacyApplicationId == 0)
            {
                legacyApplicationId = _referenceNumberRepository.GetNextLegacyApplicationId();
            }

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                // CRITICAL: make the update look like it came from legacy AVMS application
                ApplicationId = Guid.Empty,
                ApplicationStatus = applicationStatus,
                LegacyApplicationId = legacyApplicationId,
                LegacyCandidateId = 0, // not required
                LegacyVacancyId = 0, // not required
                VacancyStatus = apprenticeshipApplication.VacancyStatus,
                ClosingDate = apprenticeshipApplication.Vacancy.ClosingDate,
                UpdateSource = ApplicationStatusSummary.Source.Raa //Ensure this update is from RAA so ownership of the application is verified
            };

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplication, applicationStatusSummary);
        }

        #endregion
    }
}
