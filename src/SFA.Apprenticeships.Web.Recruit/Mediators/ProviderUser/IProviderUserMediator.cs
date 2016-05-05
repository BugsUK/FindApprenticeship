using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser
{
    using System.Security.Claims;
    using Common.Mediators;
    using Common.Models.Azure.AccessControlService;

    using SFA.Apprenticeships.Web.Recruit.ViewModels.Home;

    using ViewModels;

    public interface IProviderUserMediator
    {
        MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal);

        AuthorizationErrorDetailsViewModel AuthorizationError(string errorDetails);

        MediatorResponse<VerifyEmailViewModel> GetVerifyEmailViewModel(string username);

        MediatorResponse<SettingsViewModel> GetSettingsViewModel(string username, string ukprn);

        MediatorResponse<SettingsViewModel> UpdateUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel);

        MediatorResponse VerifyEmailAddress(string username, VerifyEmailViewModel verifyEmailViewModel);

        MediatorResponse<VerifyEmailViewModel> ResendVerificationCode(string username);

        MediatorResponse<HomeViewModel> GetHomeViewModel(string username, string ukprn, VacanciesSummarySearchViewModel vacanciesSummarySearch);

        MediatorResponse<HomeViewModel> ChangeProviderSite(string username, string ukprn, HomeViewModel viewModel);

        bool SendContactMessage(ContactMessageViewModel contactMessageViewModel);
    }
}
