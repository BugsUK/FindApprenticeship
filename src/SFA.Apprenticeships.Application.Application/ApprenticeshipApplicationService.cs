namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;
    using Strategies.Apprenticeships;

    public class ApprenticeshipApplicationService : IApprenticeshipApplicationService
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IGetApplicationForReviewStrategy _getApplicationForReviewStrategy;
        private readonly IUpdateApplicationNotesStrategy _updateApplicationNotesStrategy;

        public ApprenticeshipApplicationService(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IGetApplicationForReviewStrategy getApplicationForReviewStrategy, IUpdateApplicationNotesStrategy updateApplicationNotesStrategy)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _getApplicationForReviewStrategy = getApplicationForReviewStrategy;
            _updateApplicationNotesStrategy = updateApplicationNotesStrategy;
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
    }
}