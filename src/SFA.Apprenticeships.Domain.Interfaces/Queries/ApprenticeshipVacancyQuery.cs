namespace SFA.Apprenticeships.Domain.Interfaces.Queries
{
    using System;
    using System.Collections.Generic;
    using Entities.Vacancies.ProviderVacancies;

    public class ApprenticeshipVacancyQuery
    {
        public string FrameworkCodeName { get; set; }

        public DateTime? LiveDate { get; set; } // TODO: LiveTimeOnOrAfter??

        public DateTime? LatestClosingDate { get; set; }

        public List<ProviderVacancyStatuses> DesiredStatuses { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
