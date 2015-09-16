namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Mediators;
    using ViewModels;

    public interface IProviderUserMediator
    {
        MediatorResponse UpdateUser(UserProfileViewModel userProfileView);

    }
}
