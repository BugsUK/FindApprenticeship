namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class UpdateSavedSearchStrategy : IUpdateSavedSearchStrategy
    {
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;

        public UpdateSavedSearchStrategy(ISavedSearchWriteRepository savedSearchWriteRepository)
        {
            _savedSearchWriteRepository = savedSearchWriteRepository;
        }

        public SavedSearch UpdateSavedSearch(SavedSearch savedSearch)
        {
            return _savedSearchWriteRepository.Save(savedSearch);
        }
    }
}