namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Application.Organisation;
    using Common.Constants;
    using Constants.Messages;
    using Constants.Pages.Provider;
    using Providers;
    using Validators.Provider;
    using ViewModels.Provider;
    using Common.Mediators;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;

    public class ProviderMediator : MediatorBase, IProviderMediator
    {
        private readonly IProviderProvider _providerProvider;
        private readonly IVerifiedOrganisationProvider _verifiedOrganisationProvider;
        private readonly IProviderUserProvider _providerUserProvider;

        private readonly ProviderSiteSearchViewModelValidator _providerSiteSearchViewModelValidator;
        private readonly ProviderViewModelValidator _providerViewModelValidator;
        private readonly ProviderSiteViewModelValidator _providerSiteViewModelValidator;

        public ProviderMediator(IProviderProvider providerProvider, IVerifiedOrganisationProvider verifiedOrganisationProvider, IProviderUserProvider providerUserProvider, ProviderViewModelValidator providerViewModelValidator, ProviderSiteViewModelValidator providerSiteViewModelValidator, ProviderSiteSearchViewModelValidator providerSiteSearchViewModelValidator)
        {
            _providerProvider = providerProvider;
            _verifiedOrganisationProvider = verifiedOrganisationProvider;
            _providerUserProvider = providerUserProvider;
            _providerViewModelValidator = providerViewModelValidator;
            _providerSiteViewModelValidator = providerSiteViewModelValidator;
            _providerSiteSearchViewModelValidator = providerSiteSearchViewModelValidator;
        }
        public MediatorResponse<ProviderSiteSearchViewModel> AddSite()
        {
            var viewModel = new ProviderSiteSearchViewModel();

            return GetMediatorResponse(ProviderMediatorCodes.AddSite.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteSearchViewModel> AddSite(ProviderSiteSearchViewModel viewModel)
        {
            var validationResult = _providerSiteSearchViewModelValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ProviderMediatorCodes.AddSite.ValidationError, viewModel, validationResult);
            }

            var organisation = _verifiedOrganisationProvider.GetByReferenceNumber(viewModel.EmployerReferenceNumber);

            if (organisation == null)
            {
                return GetMediatorResponse(ProviderMediatorCodes.AddSite.SiteNotFoundByEmployerReferenceNumber, viewModel,
                    AddSitePageMessages.SiteNotFoundByEmployerReferenceNumber, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(ProviderMediatorCodes.AddSite.SiteNotFoundByEmployerReferenceNumber, viewModel,
                "TODO: found", UserMessageLevel.Success);
        }
        public MediatorResponse<ProviderViewModel> Sites(string ukprn)
        {
            var providerProfile = _providerProvider.GetProviderViewModel(ukprn);
            return GetMediatorResponse(ProviderMediatorCodes.Sites.Ok, providerProfile);
        }

        public MediatorResponse<ProviderViewModel> UpdateSites(string ukprn, string username, ProviderViewModel providerViewModel)
        {
            var result = _providerViewModelValidator.Validate(providerViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderMediatorCodes.UpdateSites.FailedValidation, providerViewModel, result);
            }

            providerViewModel = _providerProvider.SaveProviderViewModel(ukprn, providerViewModel);

            if (_providerUserProvider.GetUserProfileViewModel(username) == null)
            {
                return GetMediatorResponse(ProviderMediatorCodes.UpdateSites.NoUserProfile, providerViewModel, AuthorizeMessages.NoUserProfile, UserMessageLevel.Info);
            }

            return GetMediatorResponse(ProviderMediatorCodes.UpdateSites.Ok, providerViewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> GetSite(string ukprn, string ern)
        {
            var providerSite = _providerProvider.GetProviderSiteViewModel(ukprn, ern);

            return GetMediatorResponse(ProviderMediatorCodes.GetSite.Ok, providerSite);
        }

        public MediatorResponse<ProviderSiteViewModel> UpdateSite(ProviderSiteViewModel providerSiteViewModel)
        {
            var result = _providerSiteViewModelValidator.Validate(providerSiteViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderMediatorCodes.UpdateSite.FailedValidation, providerSiteViewModel, result);
            }

            return GetMediatorResponse(ProviderMediatorCodes.UpdateSite.Ok, providerSiteViewModel);
        }
    }
}