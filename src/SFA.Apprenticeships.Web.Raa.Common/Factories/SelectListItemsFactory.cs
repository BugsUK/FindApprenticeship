namespace SFA.Apprenticeships.Web.Raa.Common.Factories
{
    using Domain.Raa.Interfaces.Repositories.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SelectListItemsFactory
    {
        public static List<SelectListItem> GetPageSizes(int pageSize)
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Value = "5", Text = "5 per page", Selected = pageSize == 5},
                new SelectListItem {Value = "10", Text = "10 per page", Selected = pageSize == 10},
                new SelectListItem {Value = "25", Text = "25 per page", Selected = pageSize == 25},
                new SelectListItem {Value = "50", Text = "50 per page", Selected = pageSize == 50}
            };
        }

        public static List<SelectListItem> GetVacancySearchModes(VacancySearchMode searchMode)
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Value = "All", Text = "All", Selected = searchMode == VacancySearchMode.All},
                new SelectListItem {Value = "ReferenceNumber", Text = "Reference Number", Selected = searchMode == VacancySearchMode.ReferenceNumber},
                new SelectListItem {Value = "VacancyTitle", Text = "Vacancy Title", Selected = searchMode == VacancySearchMode.VacancyTitle},
                new SelectListItem {Value = "EmployerName", Text = "Employer Name", Selected = searchMode == VacancySearchMode.EmployerName},
                new SelectListItem {Value = "Postcode", Text = "Postcode", Selected = searchMode == VacancySearchMode.Postcode},
            };
        }

        public static List<SelectListItem> GetManageSearchModes(ManageVacancySearchMode searchMode)
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Value = "All", Text = "All", Selected = searchMode == ManageVacancySearchMode.All},
                new SelectListItem {Value = "Provider", Text = "Provider", Selected = searchMode == ManageVacancySearchMode.Provider},
                new SelectListItem {Value = "VacancyPostcode", Text = "Vacancy postcode", Selected = searchMode == ManageVacancySearchMode.VacancyPostcode}
            };
        }
    }
}