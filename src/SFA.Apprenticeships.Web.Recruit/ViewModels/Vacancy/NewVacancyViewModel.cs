namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Frameworks;

    public class NewVacancyViewModel
    {
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string FrameworkCodeName { get; set; }

        public List<SectorSelectItemViewModel> Sectors { get; set; }

        public string TrainingSiteErn { get; set; }

        public List<SelectListItem> ProviderSites { get; set; }
    }
}
