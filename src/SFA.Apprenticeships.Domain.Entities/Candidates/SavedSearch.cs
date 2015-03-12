namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Vacancies.Apprenticeships;

    public class SavedSearch : BaseEntity
    {
        public SavedSearch()
        {
            AlertsEnabled = true;
            ApprenticeshipLevel = "All";
            SearchField = "All";
        }

        public Guid CandidateId { get; set; }

        public bool AlertsEnabled { get; set; }

        public ApprenticeshipSearchMode SearchMode { get; set; }

        public string Location { get; set; }

        public string Keywords { get; set; }

        //TODO: Verify that we are not saving this search parameter
        //public ApprenticeshipLocationType LocationType { get; set; }

        public int WithinDistance { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Category { get; set; }

        public string CategoryFullName { get; set; }

        public string[] SubCategories { get; set; }

        public string SearchField { get; set; }

        public string LastResultsHash { get; set; }

        public DateTime? DateProcessed { get; set; }
    }
}
