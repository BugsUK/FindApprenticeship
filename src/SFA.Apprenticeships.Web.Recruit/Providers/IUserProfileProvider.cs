using SFA.Apprenticeships.Web.Recruit.ViewModels;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public interface IUserProfileProvider
    {
        UserProfileViewModel GetUserProfileViewModel(string username);
    }
}