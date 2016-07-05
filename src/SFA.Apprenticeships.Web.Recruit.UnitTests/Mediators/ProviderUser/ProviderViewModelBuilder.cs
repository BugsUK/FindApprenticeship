namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using System.Collections.Generic;
    using Raa.Common.ViewModels.Provider;

    public class ProviderViewModelBuilder
    {
        private int _providerId = 0;
        private List<ProviderSiteViewModel> _providerSites = new List<ProviderSiteViewModel>();
        private bool _isMigrated = true;

        public ProviderViewModelBuilder With(int providerId)
        {
            _providerId = providerId;
            return this;
        }

        public ProviderViewModelBuilder With(List<ProviderSiteViewModel> providerSites)
        {
            _providerSites = providerSites;
            return this;
        }

        public ProviderViewModelBuilder With(bool isMigrated)
        {
            _isMigrated = isMigrated;
            return this;
        }

        public ProviderViewModel Build()
        {
            return new ProviderViewModel
            {
                ProviderId = _providerId,
                ProviderSiteViewModels = _providerSites,
                IsMigrated = _isMigrated
            };
        }
    }
}