namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using Domain.Entities.Candidates;

    public interface ICreateSavedSearchStrategy
    {
        SavedSearch CreateSavedSearch(SavedSearch savedSearch);
    }
}