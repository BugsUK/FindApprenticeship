namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using System.Collections.Generic;
    using Applications.Entities;
    using Applications.Strategies;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Apprenticeships;

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

        public int GetApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetCountsForVacancyIds(new int[] { vacancyId })[vacancyId].AllApplications;
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
            var nextLegacyApplicationId = _referenceNumberRepository.GetNextLegacyApplicationId();

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                // CRITICAL: make the update look like it came from legacy AVMS application
                ApplicationId = Guid.Empty,
                ApplicationStatus = applicationStatus,
                LegacyApplicationId = nextLegacyApplicationId,
                LegacyCandidateId = 0, // not required
                LegacyVacancyId = 0, // not required
                VacancyStatus = apprenticeshipApplication.VacancyStatus,
                ClosingDate = apprenticeshipApplication.Vacancy.ClosingDate
            };

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplication, applicationStatusSummary);
        }

        #endregion
    }
}
