namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.ProviderUser;

    public class ProviderUserProvider : IProviderUserProvider
    {
        public ProviderUserViewModel GetUserProfileViewModel(string username)
        {
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
    }
}