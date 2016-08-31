namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface IGetCandidateSummariesStrategy
    {
        IList<CandidateSummary> GetCandidateSummaries(IEnumerable<Guid> candidateIds);
    }
}