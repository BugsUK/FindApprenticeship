namespace SFA.Apprenticeships.Application.Applications
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Entities;

    public interface IApplicationStatusUpdater
    {
        void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses);
    }
}
