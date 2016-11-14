namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;

    public interface IApprenticeshipApplicationService : ICommonApplicationService
    {
        IEnumerable<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        IEnumerable<ApprenticeshipApplicationSummary> GetApplicationSummaries(int vacancyId);

        ApprenticeshipApplicationDetail GetApplication(Guid applicationId);

        ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId);

        void SetSuccessfulDecision(Guid applicationId);

        void SetUnsuccessfulDecision(Guid applicationId, string candidateApplicationFeedback);

        void SetStateInProgress(Guid applicationId);

        void SetStateSubmitted(Guid applicationId);
    }
}
