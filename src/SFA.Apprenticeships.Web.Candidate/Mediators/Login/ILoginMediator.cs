using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Login
{
    using ViewModels.Login;
    using ViewModels.Register;

    public interface ILoginMediator
    {
        MediatorResponse<LoginResultViewModel> Index(LoginViewModel viewModel);

        MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView);

        MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel);

        MediatorResponse<ForgottenCredentialsViewModel> ForgottenPassword(ForgottenCredentialsViewModel forgottenCredentialsViewModel);

        MediatorResponse<ForgottenCredentialsViewModel> ForgottenEmail(ForgottenCredentialsViewModel forgottenCredentialsViewModel);

        MediatorResponse<PasswordResetViewModel> ResetPassword(PasswordResetViewModel resetViewModel);
    }
}