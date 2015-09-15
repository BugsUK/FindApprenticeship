using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Provider
{
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            ProviderSiteViewModels = new List<ProviderSiteViewModel>();
        }

        public IEnumerable<ProviderSiteViewModel> ProviderSiteViewModels { get; set; }
    }
}