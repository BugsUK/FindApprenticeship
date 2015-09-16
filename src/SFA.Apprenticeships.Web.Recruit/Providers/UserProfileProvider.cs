using SFA.Apprenticeships.Web.Recruit.ViewModels;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public class UserProfileProvider : IUserProfileProvider
    {
        public UserProfileViewModel GetUserProfileViewModel(string username)
        {
            //Stub code for removal
            if (username == "user.profile@naspread.onmicrosoft.com")
            {
                return new UserProfileViewModel {EmailAddress = username};
            }
            if (username == "verified.email@naspread.onmicrosoft.com")
            {
                return new UserProfileViewModel {EmailAddress = username, EmailAddressVerified = true};
            }

            return null;
            //end stub code
        }
    }
}