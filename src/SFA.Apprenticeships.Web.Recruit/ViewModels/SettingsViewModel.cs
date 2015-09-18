namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ProviderUser;

    public class SettingsViewModel
    {
        public ProviderUserViewModel ProviderUserViewModel { get; set; }

        public List<SelectListItem> ProviderSites { get; set; } 
    }
}