namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Users;
    using ViewModels.ProviderUser;

    public interface IProviderUserProvider
    {
        ProviderUserViewModel GetUserProviderViewModel(int providerUserId);

        ProviderUserViewModel GetUserProfileViewModel(string username);

        IEnumerable<ProviderUserViewModel> GetProviderUsers(string ukprn);

        ProviderUserSearchResultsViewModel SearchProviderUsers(ProviderUserSearchViewModel searchViewModel);

        bool ValidateEmailVerificationCode(string username, string code);

        ProviderUserViewModel SaveProviderUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel);

        void ResendEmailVerificationCode(string username);

        ProviderUser GetProviderUser(string username);

        void DismissReleaseNotes(string username, int version);
    }
}