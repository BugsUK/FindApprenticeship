namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Vacancies;

    public class SavedSearch : BaseEntity
    {
        public SavedSearch()
        {
            AlertsEnabled = true;
            ApprenticeshipLevel = "All";
            SearchField = "All";
            DisplaySubCategory = true;
            DisplayDescription = true;
            DisplayDistance = true;
            DisplayClosingDate = true;
            DisplayStartDate = true;
        }

        public Guid CandidateId { get; set; }

        public bool AlertsEnabled { get; set; }

        public ApprenticeshipSearchMode SearchMode { get; set; }

        public string Location { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public int Hash { get; set; }

        public string Keywords { get; set; }

        public int WithinDistance { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Category { get; set; }

        public string CategoryFullName { get; set; }

        public string[] SubCategories { get; set; }

        public string SubCategoriesFullName { get; set; }

        public string SearchField { get; set; }

        public string LastResultsHash { get; set; }

        public DateTime? DateProcessed { get; set; }

        public bool DisplaySubCategory { get; set; }

        public bool DisplayDescription { get; set; }

        public bool DisplayDistance { get; set; }

        public bool DisplayClosingDate { get; set; }

        public bool DisplayStartDate { get; set; }

        public bool DisplayApprenticeshipLevel { get; set; }

        public bool DisplayWage { get; set; }

    }
}
