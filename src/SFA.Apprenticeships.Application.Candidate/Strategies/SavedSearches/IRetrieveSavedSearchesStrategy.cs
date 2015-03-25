namespace SFA.Apprenticeships.Application.Candidate.Strategies.SavedSearches
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface IRetrieveSavedSearchesStrategy
    {
        IList<SavedSearch> RetrieveSavedSearches(Guid candidateId);
        SavedSearch RetrieveSavedSearch(Guid savedSearchId);
    }
}