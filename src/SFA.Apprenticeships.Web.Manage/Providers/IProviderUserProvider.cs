namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Collections.Generic;
    using ViewModels.ProviderUser;

    public interface IProviderUserProvider
    {
        ProviderUserViewModel GetUserProfileViewModel(string username);

        IEnumerable<ProviderUserViewModel> GetUserProfileViewModels(string ukprn);

        bool ValidateEmailVerificationCode(string username, string code);

        ProviderUserViewModel SaveProviderUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel);
    }
}