﻿namespace SFA.Apprenticeships.Application.Application
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
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;

        public ApprenticeshipApplicationService(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IGetApplicationForReviewStrategy getApplicationForReviewStrategy,
            IUpdateApplicationNotesStrategy updateApplicationNotesStrategy,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
        }

        public IList<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetSubmittedApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationCount(vacancyId);
        }

        public int GetNewApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetNewApplicationCount(vacancyId);
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

        public void AppointCandidate(Guid applicationId)
        {
            var apprenticeshipApplication = GetApplication(applicationId);

            var applicationStatusSummary = new ApplicationStatusSummary
            {               
                ApplicationId = Guid.Empty, // CRITICAL: make the update look like it came from legacy AVMS application
                ApplicationStatus = ApplicationStatuses.Successful,
                LegacyApplicationId = apprenticeshipApplication.LegacyApplicationId,
                LegacyCandidateId = 0, // not required
                LegacyVacancyId = 1, // not required
                VacancyStatus = apprenticeshipApplication.VacancyStatus,
                ClosingDate = apprenticeshipApplication.Vacancy.ClosingDate
            };

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplication, applicationStatusSummary);
        }
    }
}
