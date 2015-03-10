namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface ICandidateDiagnosticsRepository
    {
        IEnumerable<Candidate> GetActivatedCandidatesWithUnsetLegacyId();
    }
}