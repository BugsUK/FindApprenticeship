namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Domain.Entities.Raa.Reference;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Raa.Common.Factories;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class AgencyUserViewModel
    {
        public List<SelectListItem> Roles { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> RegionalTeams { get; set; }
        public RegionalTeam RegionalTeam { get; set; }
        public List<SelectListItem> SearchModes => SelectListItemsFactory.GetManageSearchModes(SearchMode);
        public ManageVacancySearchMode SearchMode { get; set; }
    }
}