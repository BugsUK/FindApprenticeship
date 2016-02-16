namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public interface ICandidateSearchService
    {
        List<CandidateSummary> SearchCandidates(CandidateSearchRequest request);
    }
}