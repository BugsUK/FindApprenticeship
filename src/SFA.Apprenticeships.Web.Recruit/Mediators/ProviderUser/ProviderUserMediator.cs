namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Constants;
    using Common.Mediators;
    using Constants.ViewModels;
    using Providers;
    using Validators.ProviderUser;
    using ViewModels.ProviderUser;

    public class ProviderUserMediator : MediatorBase, IProviderUserMediator
    {
        private readonly IProviderUserProvider _providerUserProvider;
        private readonly ProviderUserViewModelValidator _providerUserViewModelValidator;
        private readonly VerifyEmailViewModelValidator _verifyEmailViewModelValidator;

        public ProviderUserMediator(IProviderUserProvider providerUserProvider, 
                    ProviderUserViewModelValidator providerUserViewModelValidator, 
                    VerifyEmailViewModelValidator verifyEmailViewModelValidator)
        {
            _providerUserProvider = providerUserProvider;
            _providerUserViewModelValidator = providerUserViewModelValidator;
            _verifyEmailViewModelValidator = verifyEmailViewModelValidator;
        }

        public MediatorResponse<ProviderUserViewModel> GetProviderUserViewModel(string userName)
        {
            var viewModel = _providerUserProvider.GetUserProfileViewModel(userName);
            return GetMediatorResponse(ProviderUserMediatorCodes.GetProviderUserViewModel.Ok, viewModel);
        }

        public MediatorResponse UpdateUser(string userName, ProviderUserViewModel providerUserViewModel)
        {
            var result = _providerUserViewModelValidator.Validate(providerUserViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.FailedValidation, providerUserViewModel, result);
            }

            return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.Ok);
        }

        public MediatorResponse VerifyEmailAddress(string userName, VerifyEmailViewModel verifyEmailViewModel)
        {
            var result = _verifyEmailViewModelValidator.Validate(verifyEmailViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.FailedValidation, verifyEmailViewModel, result);
            }

            if (!_providerUserProvider.ValidateEmailVerificationCode(userName, verifyEmailViewModel.VerificationCode))
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.InvalidCode, VerifyEmailViewModelMessages.VerificationCodeEmailIncorrectMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(ProviderUserMediatorCodes.VerifyEmailAddress.Ok);
        }
    }
}
