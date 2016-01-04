using SFA.Apprenticeships.Web.Raa.Common.ViewModels;

namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;
    using Raa.Common.ViewModels.Vacancy;

    public class HomeViewModel
    {
        public AgencyUserViewModel AgencyUser { get; set; }

        public List<DashboardVacancySummaryViewModel> Vacancies { get; set; }
    }
}