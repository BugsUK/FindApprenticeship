namespace SFA.Apprenticeships.Application.Application
{
    using Application.Entities;
    using Domain.Entities.Candidates;
    using System.Collections.Generic;

    public interface ILegacyApplicationStatusesProvider
    {
        IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate);

        int GetApplicationStatusesPageCount(int applicationStatusExtractWindow);

        IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber, int applicationStatusExtractWindow);
    }
}
