namespace SFA.Apprenticeships.Web.Recruit.ViewModels.ProviderUser
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SettingsViewModel
    {
        public ProviderUserViewModel ProviderUserViewModel { get; set; }

        public List<SelectListItem> ProviderSites { get; set; } 
    }
}