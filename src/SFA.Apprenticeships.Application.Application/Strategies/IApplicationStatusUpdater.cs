namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using Application.Entities;
    using Domain.Entities.Candidates;
    using System.Collections.Generic;

    public interface IApplicationStatusUpdater
    {
        void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses);
    }
}
