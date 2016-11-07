namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Domain.Entities.Raa.Reference;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class AgencyUserViewModel
    {
        public List<SelectListItem> Roles { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> RegionalTeams { get; set; }
        public RegionalTeam RegionalTeam { get; set; }
    }
}