namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    //todo: 1.8: DTO for a single candidate, contains their saved searches to be processed
    public class CandidateSavedSearches
    {
        public Guid CandidateId { get; set; }

        public IEnumerable<SavedSearch> SavedSearches { get; set; }
    }
}
