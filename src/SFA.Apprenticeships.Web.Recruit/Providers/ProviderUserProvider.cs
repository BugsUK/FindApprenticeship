namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using Application.UserProfile;
    using Domain.Entities.Users;
    using ViewModels.ProviderUser;

    public class ProviderUserProvider : IProviderUserProvider
    {
        private readonly IProviderUserService _providerUserService;

        public ProviderUserProvider(IProviderUserService providerUserService)
        {
            _providerUserService = providerUserService;
        }

        public ProviderUserViewModel GetUserProfileViewModel(string username)
        {
            var providerUser = _providerUserService.GetProviderUser(username);
            if (providerUser != null)
            {
                return Convert(providerUser);
            }

            //Stub code for removal
            if (username == "user.profile@naspread.onmicrosoft.com")
            {
                return new ProviderUserViewModel {EmailAddress = username};
            }
            if (username == "verified.email@naspread.onmicrosoft.com")
            {
                return new ProviderUserViewModel {EmailAddress = username, EmailAddressVerified = true};
            }

            return null;
            //end stub code
        }

        public bool ValidateEmailVerificationCode(string username, string code)
        {
            //Stub

            if (code == "ABC123")
            {
                return true;
            }

            return false;

            //End Stub
        }

        private ProviderUserViewModel Convert(ProviderUser providerUser)
        {
            var viewModel = new ProviderUserViewModel
            {
                DefaultTrainingSiteId = providerUser.PreferredSiteId,
                EmailAddress = providerUser.Email,
                //TODO: Check with Krister how we're storing EmailAddressVerified
                EmailAddressVerified = providerUser.VerificationCode == null,
                Fullname = providerUser.Fullname,
                //PhoneNumber = providerUser.PhoneNumber
            };

            return viewModel;
        }
    }
}