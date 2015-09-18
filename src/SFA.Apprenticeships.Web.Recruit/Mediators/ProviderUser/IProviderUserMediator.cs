namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using Common.Mediators;
    using ViewModels;
    using ViewModels.ProviderUser;

    public interface IProviderUserMediator
    {
        MediatorResponse<ProviderUserViewModel> GetProviderUserViewModel(string username);

        MediatorResponse<SettingsViewModel> GetSettingsViewModel(string username, string ukprn);

        MediatorResponse<SettingsViewModel> UpdateUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel);

        MediatorResponse VerifyEmailAddress(string username, VerifyEmailViewModel verifyEmailViewModel);
    }
}
