namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface IApprenticeshipApplicationService : ICommonApplicationService
    {
        IEnumerable<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        ApprenticeshipApplicationDetail GetApplication(Guid applicationId);

        ApprenticeshipApplicationDetail GetApplicationForReview(Guid applicationId);

        void SetSuccessfulDecision(Guid applicationId);

        void SetUnsuccessfulDecision(Guid applicationId);
    }
}
