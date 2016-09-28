namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderViewModelConverter
    {
        public static ProviderViewModel Convert(this Provider provider)
        {
            if (provider == null)
            {
                return new ProviderViewModel();
            }

            var viewModel = new ProviderViewModel
            {
                ProviderId = provider.ProviderId,
                Ukprn = provider.Ukprn,
                ProviderName = provider.TradingName,
                IsMigrated = provider.IsMigrated,
            };

            return viewModel;
        }

        public static ProviderViewModel Convert(this Provider provider, IEnumerable<ProviderSite> providerSites)
        {
            var viewModel = provider.Convert();
            viewModel.ProviderSiteViewModels = providerSites.Select(ps => ps.Convert()).ToList();
            return viewModel;
        }
    }
}