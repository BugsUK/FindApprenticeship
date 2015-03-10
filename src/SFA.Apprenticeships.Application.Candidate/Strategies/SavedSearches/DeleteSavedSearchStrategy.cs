namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class DeleteSavedSearchStrategy : IDeleteSavedSearchStrategy
    {
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;

        public DeleteSavedSearchStrategy(ISavedSearchReadRepository savedSearchReadRepository, ISavedSearchWriteRepository savedSearchWriteRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            _savedSearchWriteRepository = savedSearchWriteRepository;
        }

        public SavedSearch DeleteSavedSearch(Guid savedSearchId)
        {
            var savedSearch = _savedSearchReadRepository.Get(savedSearchId);

            if (savedSearch == null) return null;

            _savedSearchWriteRepository.Delete(savedSearchId);

            return savedSearch;
        }
    }
}