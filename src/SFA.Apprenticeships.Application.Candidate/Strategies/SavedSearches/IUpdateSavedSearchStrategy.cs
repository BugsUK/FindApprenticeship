namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using Domain.Entities.Candidates;

    public interface IUpdateSavedSearchStrategy
    {
        SavedSearch UpdateSavedSearch(SavedSearch savedSearch);
    }
}