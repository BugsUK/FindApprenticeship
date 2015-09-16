using System.Collections.Generic;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public class ProviderProvider : IProviderProvider
    {
        public ProviderViewModel GetProviderViewModel(string ukprn)
        {
            //Stub code for removal
            if (ukprn == "hasproviderprofile")
            {
                return new ProviderViewModel();
            }
            if (ukprn == "onesite")
            {
                return new ProviderViewModel
                {
                    ProviderSiteViewModels = new List<ProviderSiteViewModel> {new ProviderSiteViewModel()}
                };
            }

            return null;
            //end stub code
        }
    }
}