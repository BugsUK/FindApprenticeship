using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Providers;
    using Validators.Provider;
    using ViewModels.Provider;

    public class ProviderMediator : MediatorBase, IProviderMediator
    {
        private readonly IProviderProvider _providerProvider;
        private readonly ProviderViewModelValidator _providerViewModelValidator;

        public ProviderMediator(IProviderProvider providerProvider, ProviderViewModelValidator providerViewModelValidator, ProviderSiteViewModelValidator providerSiteViewModelValidator)
        {
            _providerProvider = providerProvider;
            _providerViewModelValidator = providerViewModelValidator;
        }

        public MediatorResponse<ProviderSiteSearchViewModel> AddSite()
        {
            return null;
        }

        public MediatorResponse<ProviderSiteSearchResponseViewModel> FindSite(ProviderSiteSearchViewModel searchViewModel)
        {
            return null;
        }

        public MediatorResponse<ProviderViewModel> Sites(string ukprn)
        {
            var providerProfile = _providerProvider.GetProviderViewModel(ukprn);
            return GetMediatorResponse(ProviderMediatorCodes.Sites.Ok, providerProfile);
        }

        public MediatorResponse<ProviderViewModel> UpdateSites(ProviderViewModel providerViewModel)
        {
            var result = _providerViewModelValidator.Validate(providerViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderMediatorCodes.UpdateSites.FailedValidation, providerViewModel, result);
            }

            return GetMediatorResponse(ProviderMediatorCodes.UpdateSites.Ok, providerViewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> GetSite(string ern)
        {
            var providerSite = _providerProvider.GetProviderSiteViewModel(ern);
            return GetMediatorResponse(ProviderMediatorCodes.GetSite.Ok, providerSite);
        }

        public MediatorResponse<ProviderSiteViewModel> UpdateSite(ProviderSiteViewModel providerSiteViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
