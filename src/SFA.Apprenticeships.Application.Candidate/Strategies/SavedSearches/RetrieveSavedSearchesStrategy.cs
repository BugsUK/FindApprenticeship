namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class RetrieveSavedSearchesStrategy : IRetrieveSavedSearchesStrategy
    {
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;

        public RetrieveSavedSearchesStrategy(ISavedSearchReadRepository savedSearchReadRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
        }

        public IList<SavedSearch> RetrieveSavedSearches(Guid candidateId)
        {
            var savedSearches = _savedSearchReadRepository.GetForCandidate(candidateId);
            return savedSearches;
        }
    }
}