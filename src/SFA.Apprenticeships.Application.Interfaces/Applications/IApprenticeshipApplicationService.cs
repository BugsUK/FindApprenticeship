namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface IApprenticeshipApplicationService
    {
        IList<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        int GetApplicationCount(int vacancyId);

        int GetNewApplicationCount(int vacancyId);

        ApprenticeshipApplicationDetail GetApplication(Guid applicationId);

        ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId);

        void UpdateApplicationNotes(Guid applicationId, string notes);
    }
}