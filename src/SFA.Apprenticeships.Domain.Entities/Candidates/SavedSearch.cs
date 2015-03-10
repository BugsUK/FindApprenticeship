namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class SavedSearch : BaseEntity
    {
        //todo: 1.8: saved search definition (search parameters, alerts options, etc.)
        //note: need to store datetime last executed to ensure alerts only triggered if there are new results
        //note: not all parameters used to *execute* a search are saved as the definition of a saved search (e.g. ordering, page number, page size)
        // location, distance, level, [keyword | category, subcategories]
        public string Name { get; set; }
    }
}
