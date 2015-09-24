namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Mediators;
    using Constants.ViewModels;
    using Providers;
    using Validators.ProviderUser;
    using ViewModels;
    using ViewModels.ProviderUser;

    public class ProviderUserMediator : MediatorBase, IProviderUserMediator
    {
        private readonly IProviderUserProvider _providerUserProvider;
        private readonly IProviderProvider _providerProvider;
        private readonly ProviderUserViewModelValidator _providerUserViewModelValidator;
        private readonly VerifyEmailViewModelValidator _verifyEmailViewModelValidator;

        public ProviderUserMediator(IProviderUserProvider providerUserProvider, 
            IProviderProvider providerProvider,
                    ProviderUserViewModelValidator providerUserViewModelValidator, 
                    VerifyEmailViewModelValidator verifyEmailViewModelValidator)
        {
            _providerUserProvider = providerUserProvider;
            _providerProvider = providerProvider;
            _providerUserViewModelValidator = providerUserViewModelValidator;
            _verifyEmailViewModelValidator = verifyEmailViewModelValidator;
        }

        public MediatorResponse<ProviderUserViewModel> GetProviderUserViewModel(string username)
        {
            var providerUserViewModel = _providerUserProvider.GetUserProfileViewModel(username) ?? new ProviderUserViewModel();
            return GetMediatorResponse(ProviderUserMediatorCodes.GetProviderUserViewModel.Ok, providerUserViewModel);
        }

        public MediatorResponse<SettingsViewModel> GetSettingsViewModel(string username, string ukprn)
        {
            var providerUserViewModel = _providerUserProvider.GetUserProfileViewModel(username) ?? new ProviderUserViewModel();
            var providerSites = GetProviderSites(ukprn);
            var viewModel = new SettingsViewModel
            {
                ProviderUserViewModel = providerUserViewModel,
                ProviderSites = providerSites
            };

            return GetMediatorResponse(ProviderUserMediatorCodes.GetSettingsViewModel.Ok, viewModel);
        }

        private List<SelectListItem> GetProviderSites(string ukprn)
        {
            var providerSites = _providerProvider.GetProviderSiteViewModels(ukprn);

            var sites = providerSites.Select(ps => new SelectListItem {Value = ps.Ern, Text = ps.Name}).ToList();

            return sites;
        }

        public MediatorResponse<SettingsViewModel> UpdateUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel)
        {
            var result = _providerUserViewModelValidator.Validate(providerUserViewModel);

            var providerSites = GetProviderSites(ukprn);
            var viewModel = new SettingsViewModel
            {
                ProviderUserViewModel = providerUserViewModel,
                ProviderSites = providerSites
            };

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.FailedValidation, viewModel, result);
            }

            var existingUser = _providerUserProvider.GetUserProfileViewModel(username);

            viewModel.ProviderUserViewModel = _providerUserProvider.SaveProviderUser(username, ukprn, providerUserViewModel);

            if (existingUser != null && existingUser.EmailAddress != viewModel.ProviderUserViewModel.EmailAddress)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.EmailUpdated, viewModel);
            }

            return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.Ok, viewModel, ProviderUserViewModelMessages.AccountCreated, UserMessageLevel.Success);
        }

        public MediatorResponse VerifyEmailAddress(string username, VerifyEmailViewModel verifyEmailViewModel)
        {
            var result = _verifyEmailViewModelValidator.Validate(verifyEmailViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.FailedValidation, verifyEmailViewModel, result);
            }

            if (!_providerUserProvider.ValidateEmailVerificationCode(username, verifyEmailViewModel.VerificationCode))
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.InvalidCode, VerifyEmailViewModelMessages.VerificationCodeEmailIncorrectMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.Ok);
        }

        public MediatorResponse<VerifyEmailViewModel> ResendVerificationCode(string username)
        {
            var providerUserViewModel = _providerUserProvider.GetUserProfileViewModel(username);

            if (providerUserViewModel == null)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.ResendVerificationCode.Error, new VerifyEmailViewModel(), VerifyEmailViewModelMessages.VerificationCodeEmailResentFailedMessage, UserMessageLevel.Error);
            }

            var viewModel = new VerifyEmailViewModel
            {
                EmailAddress = providerUserViewModel.EmailAddress
            };

            _providerUserProvider.SendEmailVerificationCode(username);

            var message = string.Format(VerifyEmailViewModelMessages.VerificationCodeEmailResentMessage, viewModel.EmailAddress);
            return GetMediatorResponse(ProviderUserMediatorCodes.ResendVerificationCode.Ok, viewModel, message, UserMessageLevel.Success);
        }
    }
}
