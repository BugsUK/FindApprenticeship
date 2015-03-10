namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using System;
    using Domain.Entities.Candidates;

    public interface IDeleteSavedSearchStrategy
    {
        SavedSearch DeleteSavedSearch(Guid savedSearchId);
    }
}