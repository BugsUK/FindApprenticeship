using System.Collections.Generic;
using System.Web.Mvc;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using Provider;

    public class HomeViewModel
    {
        public ProviderUserViewModel ProviderUserViewModel { get; set; }

        public ProviderViewModel ProviderViewModel { get; set; }

        public List<SelectListItem> ProviderSites { get; set; }

        public List<VacancyViewModel> Vacancies { get; set; }
    }
}