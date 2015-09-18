using System.Collections.Generic;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.Providers;
    using Domain.Entities.Providers;

    public class ProviderProvider : IProviderProvider
    {
        private readonly IProviderService _providerService;

        public ProviderProvider(IProviderService providerService)
        {
            _providerService = providerService;
        }

        public ProviderViewModel GetProviderViewModel(string ukprn)
        {
            //Stub code for removal
            if (ukprn == "hasproviderprofile")
            {
                return new ProviderViewModel {ProviderName = "Key Training Ltd"};
            }
            //end stub code

            var provider = _providerService.GetProvider(ukprn);
            var providerSites = _providerService.GetProviderSites(ukprn);

            return Convert(provider, providerSites);
        }

        public ProviderSiteViewModel GetProviderSiteViewModel(string ern)
        {
            return new ProviderSiteViewModel
            {
                Ern = ern,
                Name = "Basing View",
                EmailAddress = "basing-view@keytraining.co.uk",
                PhoneNumber = "01256 320222"
            };
        }

        private static ProviderViewModel Convert(Provider provider, IEnumerable<ProviderSite> providerSites)
        {
            var viewModel = new ProviderViewModel
            {
                ProviderName = provider.Name,
                ProviderSiteViewModels = providerSites.Select(Convert).ToList()
            };

            return viewModel;
        }

        private static ProviderSiteViewModel Convert(ProviderSite providerSite)
        {
            return new ProviderSiteViewModel
            {
                Ern = providerSite.Ern,
                Name = providerSite.Name,
                EmailAddress = providerSite.EmailAddress,
                PhoneNumber = providerSite.PhoneNumber
            };
        }
    }
}