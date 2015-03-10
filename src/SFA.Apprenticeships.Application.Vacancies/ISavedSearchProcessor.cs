namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    public interface ISavedSearchProcessor
    {
        //todo: 1.8: queue CandidateSavedSearches to rabbit for *active* candidates with comm prefs enabled and saved searches (that have alerts enabled)
        void QueueSavedSearchCandidates();

        //todo: 1.8: retrieve top 5 most recent search results for each saved search (use IVacancySearchProvider to execute)
        // write results to SavedSearchAlerts repo, upsert based on candidate+savedsearch+date (assuming for now only runs once per day)
        // note: should not write if no new results (check datetime of SavedSearch)
        void ProcessSavedSearchCandidate(CandidateSavedSearches candidateSavedSearches);
    }
}
