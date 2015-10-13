namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Frameworks;
    using Provider;

    public class NewVacancyViewModel
    {
        public long? VacancyReferenceNumber { get; set; }

        public string Ukprn { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string FrameworkCodeName { get; set; }

        public List<SectorSelectItemViewModel> SectorsAndFrameworks { get; set; }

        public ProviderSiteEmployerLinkViewModel ProviderSiteEmployerLink { get; set; }
    }
}
