namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Applications;
    using Application.Applications.Entities;
    using Domain.Entities.Candidates;

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