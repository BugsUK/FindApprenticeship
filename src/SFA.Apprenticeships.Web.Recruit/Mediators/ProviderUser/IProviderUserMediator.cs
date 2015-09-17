namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Mediators;
    using ViewModels;
    using ViewModels.ProviderUser;

    public interface IProviderUserMediator
    {
        MediatorResponse<ProviderUserViewModel> GetProviderUserViewModel(string userName);

        MediatorResponse UpdateUser(string userName, ProviderUserViewModel providerUserViewModel);

        MediatorResponse VerifyEmailAddress(string userName, VerifyEmailViewModel verifyEmailViewModel);

    }
}
