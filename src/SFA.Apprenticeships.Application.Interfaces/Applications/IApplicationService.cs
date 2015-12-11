namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface IApplicationService
    {
        IEnumerable<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId);

        int GetApplicationCount(string vacancyReference);

        int GetApplicationCount(int vacancyId);

        ApprenticeshipApplicationDetail GetApplication(Guid applicationId);
    }
}