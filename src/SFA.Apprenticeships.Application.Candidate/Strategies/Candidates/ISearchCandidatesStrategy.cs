namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public interface ISearchCandidatesStrategy
    {
        List<CandidateSummary> SearchCandidates(CandidateSearchRequest request);
    }
}