namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Mediators;
    using ViewModels;
    using ViewModels.ProviderUser;

    public interface IProviderUserMediator
    {
        MediatorResponse UpdateUser(ProviderUserViewModel providerUserViewModel);

    }
}
