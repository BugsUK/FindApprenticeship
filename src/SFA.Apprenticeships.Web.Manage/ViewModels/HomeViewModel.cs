namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public AgencyUserViewModel AgencyUser { get; set; }

        public List<VacancySummaryViewModel> Vacancies { get; set; }
    }
}