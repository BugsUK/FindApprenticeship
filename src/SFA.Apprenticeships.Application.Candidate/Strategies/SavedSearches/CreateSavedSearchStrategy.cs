namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class CreateSavedSearchStrategy : ICreateSavedSearchStrategy
    {
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;

        public CreateSavedSearchStrategy(ISavedSearchReadRepository savedSearchReadRepository, ISavedSearchWriteRepository savedSearchWriteRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            _savedSearchWriteRepository = savedSearchWriteRepository;
        }

        public SavedSearch CreateSavedSearch(SavedSearch savedSearch)
        {
            if (savedSearch.EntityId != Guid.Empty)
            {
                var existingSavedSearch = _savedSearchReadRepository.Get(savedSearch.EntityId);

                if (existingSavedSearch != null)
                {
                    return existingSavedSearch;
                }
            }

            return _savedSearchWriteRepository.Save(savedSearch);
        }
    }
}