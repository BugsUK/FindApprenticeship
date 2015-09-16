namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Mediators;
    using Validators.ProviderUser;
    using ViewModels.ProviderUser;

    public class ProviderUserMediator : MediatorBase, IProviderUserMediator
    {
        private readonly ProviderUserViewModelValidator _providerUserViewModelValidator;

        public ProviderUserMediator(ProviderUserViewModelValidator providerUserViewModelValidator)
        {
            _providerUserViewModelValidator = providerUserViewModelValidator;
        }

        public MediatorResponse UpdateUser(ProviderUserViewModel providerUserViewModel)
        {
            var result = _providerUserViewModelValidator.Validate(providerUserViewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.FailedValidation, providerUserViewModel, result);
            }

            return GetMediatorResponse(ProviderUserMediatorCodes.UpdateUser.Ok);
        }
    }
}
