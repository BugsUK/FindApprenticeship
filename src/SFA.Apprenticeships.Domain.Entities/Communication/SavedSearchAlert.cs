namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;
    using System.Collections.Generic;
    using Candidates;
    using Vacancies.Apprenticeships;

    public class SavedSearchAlert : BaseEntity<Guid>
    {
        public SavedSearch Parameters { get; set; }

        public IList<ApprenticeshipSearchResponse> Results { get; set; }

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
