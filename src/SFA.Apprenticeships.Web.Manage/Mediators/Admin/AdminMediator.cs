namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly ProviderSearchViewModelServerValidator _providerSearchViewModelServerValidator = new ProviderSearchViewModelServerValidator();
        private readonly ProviderViewModelServerValidator _providerViewModelServerValidator = new ProviderViewModelServerValidator();

        private readonly IProviderProvider _providerProvider;

        public AdminMediator(IProviderProvider providerProvider)
        {
            _providerProvider = providerProvider;
        }

        public MediatorResponse<ProviderSearchResultsViewModel> SearchProviders(ProviderSearchViewModel searchViewModel)
        {
            var validatonResult = _providerSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.SearchProviders.FailedValidation, new ProviderSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var viewModel = _providerProvider.SearchProviders(searchViewModel);

            return GetMediatorResponse(AdminMediatorCodes.SearchProviders.Ok, viewModel);
        }

        public MediatorResponse<ProviderViewModel> AddProvider(ProviderViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}