using System.Collections.Generic;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    public class SettingsViewModel
    {
        public ProviderUserViewModel ProviderUserViewModel { get; set; }

        public List<SelectListItem> ProviderSites { get; set; } 
    }
}