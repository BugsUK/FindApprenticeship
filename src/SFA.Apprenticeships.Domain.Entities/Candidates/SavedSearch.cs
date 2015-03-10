namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Vacancies.Apprenticeships;

    public class SavedSearch : BaseEntity
    {
        //todo: 1.8: saved search definition (search parameters, alerts options, etc.)
        //note: not all parameters used to *execute* a search are saved as the definition of a saved search (e.g. ordering, page number, page size)
        // location, distance, level, [keyword | category, subcategories]
        public Guid CandidateId { get; set; }

        public string Name { get; set; }

        public ApprenticeshipSearchMode SearchMode { get; set; }

        public string Keywords { get; set; }

        public string Location { get; set; }

        public ApprenticeshipLocationType LocationType { get; set; }

        public int SearchRadius { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Category { get; set; }

        public string[] SubCategories { get; set; }

        public string LastResultsHash { get; set; }

        public DateTime? DateProcessed { get; set; }
    }
}
