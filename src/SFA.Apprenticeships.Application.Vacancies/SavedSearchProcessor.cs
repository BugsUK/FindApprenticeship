namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    public class SavedSearchProcessor : ISavedSearchProcessor
    {
        public void QueueCandidateSavedSearches()
        {
            //todo: 1.8: queue CandidateSavedSearches to rabbit for *active* candidates with comm prefs enabled and saved searches (that have alerts enabled)
        }

        public void ProcessCandidateSavedSearches(CandidateSavedSearches candidateSavedSearches)
        {
            //todo: 1.8: retrieve top 5 most recent search results for each saved search (use IVacancySearchProvider to execute)
            // write results to SavedSearchAlerts repo, upsert based on candidate+savedsearch+date (assuming for now only runs once per day)
            // note: should not write if no new results (check LastResultsHash if any) since last *sent* search
            // note: update SavedSearch.LastResultsHash to ensure alerts are only triggered if there are new results
            // note: update SavedSearch.DateProcessed with datetime processed (if changes)

            //todo: once we have the vacancy posted date (March 2015) we may store this instead of the processed date
        }
    }
}
