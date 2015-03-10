namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;
    using System.Collections.Generic;
    using Candidates;
    using Vacancies.Apprenticeships;

    public class SavedSearchAlert : BaseEntity
    {
        //todo: 1.8: properties for a saved search alert item (apprenticeships only)
        //todo: don't automap entity properties!
        public SavedSearch Parameters { get; set; }

        public IEnumerable<ApprenticeshipSummary> Results { get; set; }

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
