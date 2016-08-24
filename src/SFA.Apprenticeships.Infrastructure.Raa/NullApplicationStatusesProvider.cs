namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Application.Application;
    using Application.Application.Entities;
    using Domain.Entities.Candidates;
    using System.Collections.Generic;
    using System.Linq;

    public class NullApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            return Enumerable.Empty<ApplicationStatusSummary>();
        }

        public int GetApplicationStatusesPageCount(int applicationStatusExtractWindow)
        {
            return 0;
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber, int applicationStatusExtractWindow)
        {
            return Enumerable.Empty<ApplicationStatusSummary>();
        }
    }
}