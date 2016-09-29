namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using System;
    using Application.Interfaces;
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly ProviderSearchViewModelServerValidator _providerSearchViewModelServerValidator = new ProviderSearchViewModelServerValidator();
        private readonly ProviderViewModelServerValidator _providerViewModelServerValidator = new ProviderViewModelServerValidator();
        private readonly ProviderSiteSearchViewModelServerValidator _providerSiteSearchViewModelServerValidator = new ProviderSiteSearchViewModelServerValidator();
        private readonly ProviderSiteViewModelServerValidator _providerSiteViewModelServerValidator = new ProviderSiteViewModelServerValidator();

        private readonly IProviderProvider _providerProvider;
        private readonly ILogService _logService;

        public AdminMediator(IProviderProvider providerProvider, ILogService logService)
        {
            _providerProvider = providerProvider;
            _logService = logService;
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

        public MediatorResponse<ProviderViewModel> GetProvider(int providerId)
        {
            var viewModel = _providerProvider.GetProviderViewModel(providerId);

            return GetMediatorResponse(AdminMediatorCodes.GetProvider.Ok, viewModel);
        }

        public MediatorResponse<ProviderViewModel> CreateProvider(ProviderViewModel viewModel)
        {
            var validatonResult = _providerViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProvider.FailedValidation, viewModel, validatonResult);
            }

            var existingViewModel = _providerProvider.GetProviderViewModel(viewModel.Ukprn, false);
            if (existingViewModel != null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProvider.UkprnAlreadyExists, viewModel, ProviderViewModelMessages.UkprnAlreadyExists, UserMessageLevel.Error);
            }

            viewModel = _providerProvider.CreateProvider(viewModel);

            return GetMediatorResponse(AdminMediatorCodes.CreateProvider.Ok, viewModel, ProviderViewModelMessages.ProviderCreatedSuccessfully, UserMessageLevel.Info);
        }

        public MediatorResponse<ProviderSiteSearchResultsViewModel> SearchProviderSites(ProviderSiteSearchViewModel searchViewModel)
        {
            var validatonResult = _providerSiteSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.SearchProviderSites.FailedValidation, new ProviderSiteSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var viewModel = _providerProvider.SearchProviderSites(searchViewModel);

            return GetMediatorResponse(AdminMediatorCodes.SearchProviderSites.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> GetProviderSite(int providerSiteId)
        {
            var viewModel = _providerProvider.GetProviderSiteViewModel(providerSiteId);

            return GetMediatorResponse(AdminMediatorCodes.GetProviderSite.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> CreateProviderSite(ProviderSiteViewModel viewModel)
        {
            var validatonResult = _providerSiteViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.FailedValidation, viewModel, validatonResult);
            }

            var existingViewModel = _providerProvider.GetProviderSiteViewModel(viewModel.EdsUrn);
            if (existingViewModel != null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.EdsUrnAlreadyExists, viewModel, ProviderSiteViewModelMessages.EdsUrnAlreadyExists, UserMessageLevel.Error);
            }

            viewModel = _providerProvider.CreateProviderSite(viewModel);

            return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.Ok, viewModel, ProviderSiteViewModelMessages.ProviderSiteCreatedSuccessfully, UserMessageLevel.Info);
        }

        public MediatorResponse<ProviderSiteViewModel> SaveProviderSite(ProviderSiteViewModel viewModel)
        {
            try
            {
                viewModel = _providerProvider.SaveProviderSite(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.SaveProviderSite.Ok, viewModel, ProviderSiteViewModelMessages.ProviderSiteSavedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save provider site with id={viewModel.ProviderSiteId}", ex);
                viewModel = _providerProvider.GetProviderSiteViewModel(viewModel.ProviderSiteId);
                return GetMediatorResponse(AdminMediatorCodes.SaveProviderSite.Error, viewModel, ProviderSiteViewModelMessages.ProviderSiteSaveError, UserMessageLevel.Error);
            }
        }
    }
}