using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Login
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Common.Configuration;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Extensions;
    using Providers;
    using Validators;
    using ViewModels.Login;
    using ViewModels.Register;

    public class LoginMediator : MediatorBase, ILoginMediator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IUserDataProvider _userDataProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ResendAccountUnlockCodeViewModelServerValidator _resendAccountUnlockCodeViewModelServerValidator;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        private readonly PasswordResetViewModelServerValidator _passwordResetViewModelServerValidator;
        private readonly ForgottenEmailViewModelServerValidator _forgottenEmailViewModelServerValidator;
        private readonly ILogService _logService;

        public LoginMediator(IUserDataProvider userDataProvider, 
            ICandidateServiceProvider candidateServiceProvider,
            IConfigurationService configurationService,
            LoginViewModelServerValidator loginViewModelServerValidator, 
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ResendAccountUnlockCodeViewModelServerValidator resendAccountUnlockCodeViewModelServerValidator,
            IAuthenticationTicketService authenticationTicketService,
            ForgottenPasswordViewModelServerValidator forgottenPasswordViewModelServerValidator,
            PasswordResetViewModelServerValidator passwordResetViewModelServerValidator,
            ForgottenEmailViewModelServerValidator forgottenEmailViewModelServerValidator,
            ILogService logService)
        {
            _userDataProvider = userDataProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _configurationService = configurationService;
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _resendAccountUnlockCodeViewModelServerValidator = resendAccountUnlockCodeViewModelServerValidator;
            _authenticationTicketService = authenticationTicketService;
            _forgottenPasswordViewModelServerValidator = forgottenPasswordViewModelServerValidator;
            _passwordResetViewModelServerValidator = passwordResetViewModelServerValidator;
            _forgottenEmailViewModelServerValidator = forgottenEmailViewModelServerValidator;
            _logService = logService;
        }

        public MediatorResponse<LoginResultViewModel> Index(LoginViewModel viewModel)
        {
            var validationResult = _loginViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse<LoginResultViewModel>(LoginMediatorCodes.Index.ValidationError, null, validationResult);
            }

            var result = _candidateServiceProvider.Login(viewModel);

            if (result.UserStatus.HasValue)
            {
                if (result.UserStatus == UserStatuses.Locked)
                {
                    _userDataProvider.Push(UserDataItemNames.UnlockEmailAddress, result.EmailAddress);

                    return GetMediatorResponse(LoginMediatorCodes.Index.AccountLocked, result);
                }

                if (result.IsAuthenticated)
                {
                    _logService.Info("User {0} successfully logged in. User Status: {1}", result.EmailAddress, result.UserStatus);

                    _userDataProvider.SetUserContext(result.EmailAddress, result.FullName, result.AcceptedTermsAndConditionsVersion);

                    if (result.UserStatus == UserStatuses.PendingActivation)
                    {
                        return GetMediatorResponse(LoginMediatorCodes.Index.PendingActivation, result);
                    }

                    var candidate = _candidateServiceProvider.GetCandidate(result.EmailAddress);
                    SetUsersApplicationContext(candidate.EntityId);

                    // Redirect to session return URL (if any).
                    var returnUrl = _userDataProvider.Pop(UserDataItemNames.SessionReturnUrl) ?? _userDataProvider.Pop(UserDataItemNames.ReturnUrl);
                    result.ReturnUrl = returnUrl;

                    if (result.AcceptedTermsAndConditionsVersion != _configurationService.Get<CommonWebConfiguration>().TermsAndConditionsVersion)
                    {
                        return returnUrl.IsValidReturnUrl()
                            ? GetMediatorResponse(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, result, parameters: returnUrl)
                            : GetMediatorResponse(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, result);
                    }

                    if (returnUrl.IsValidReturnUrl())
                    {
                        return GetMediatorResponse(LoginMediatorCodes.Index.ReturnUrl, result, parameters: returnUrl);
                    }

                    // Redirect to last viewed vacancy (if any).
                    var lastViewedVacancy = _userDataProvider.PopLastViewedVacancy();

                    if (lastViewedVacancy != null)
                    {
                        switch (lastViewedVacancy.Type)
                        {
                            case VacancyType.Apprenticeship:
                            {
                                var applicationStatus = _candidateServiceProvider.GetApplicationStatus(candidate.EntityId, lastViewedVacancy.Id);

                                if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                                {
                                    return GetMediatorResponse(LoginMediatorCodes.Index.ApprenticeshipApply, result, parameters: lastViewedVacancy.Id);
                                }

                                return GetMediatorResponse(LoginMediatorCodes.Index.ApprenticeshipDetails, result, parameters: lastViewedVacancy.Id);
                            }
                            case VacancyType.Traineeship:
                                return GetMediatorResponse(LoginMediatorCodes.Index.TraineeshipDetails, result, parameters: lastViewedVacancy.Id);
                        }
                    }

                    return GetMediatorResponse(LoginMediatorCodes.Index.Ok, result);
                }                
            }

            return GetMediatorResponse(LoginMediatorCodes.Index.LoginFailed, result, parameters: result.ViewModelMessage);
        }


        public MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView)
        {
            var validationResult = _accountUnlockViewModelServerValidator.Validate(accountUnlockView);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.Unlock.ValidationError, accountUnlockView, validationResult);
            }

            var accountUnlockViewModel = _candidateServiceProvider.VerifyAccountUnlockCode(accountUnlockView);
            switch (accountUnlockViewModel.Status)
            {
                case AccountUnlockState.Ok:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.UnlockedSuccessfully, accountUnlockView);
                case AccountUnlockState.UserInIncorrectState:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.UserInIncorrectState, accountUnlockView);
                case AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountEmailAddressOrUnlockCodeInvalid, accountUnlockView, AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                case AccountUnlockState.AccountUnlockCodeExpired:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountUnlockCodeExpired, accountUnlockView, AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountUnlockFailed, accountUnlockView, AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
            }
        }

        public MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel)
        {
            var validationResult = _resendAccountUnlockCodeViewModelServerValidator.Validate(accountUnlockViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.Resend.ValidationError, accountUnlockViewModel, validationResult);
            }

            accountUnlockViewModel = _candidateServiceProvider.RequestAccountUnlockCode(accountUnlockViewModel);
            _userDataProvider.Push(UserDataItemNames.EmailAddress, accountUnlockViewModel.EmailAddress);

            if (accountUnlockViewModel.HasError())
            {
                if (accountUnlockViewModel.Status == AccountUnlockState.UserInIncorrectState || accountUnlockViewModel.Status == AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid)
                {
                    return GetMediatorResponse(LoginMediatorCodes.Resend.ResentSuccessfully, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success);
                }
                return GetMediatorResponse(LoginMediatorCodes.Resend.ResendFailed, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(LoginMediatorCodes.Resend.ResentSuccessfully, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success);
        }

        public MediatorResponse<ForgottenCredentialsViewModel> ForgottenPassword(ForgottenCredentialsViewModel forgottenCredentialsViewModel)
        {
            var forgottenPasswordViewModel = forgottenCredentialsViewModel.ForgottenPasswordViewModel;
            var validationResult = _forgottenPasswordViewModelServerValidator.Validate(forgottenPasswordViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.ForgottenPassword.FailedValidation, forgottenCredentialsViewModel, validationResult);
            }

            if (_candidateServiceProvider.RequestForgottenPasswordResetCode(forgottenPasswordViewModel))
            {
                return GetMediatorResponse(LoginMediatorCodes.ForgottenPassword.PasswordSent, forgottenCredentialsViewModel);
            }

            return GetMediatorResponse(LoginMediatorCodes.ForgottenPassword.FailedToSendResetCode, forgottenCredentialsViewModel, PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning);
        }

        public MediatorResponse<ForgottenCredentialsViewModel> ForgottenEmail(ForgottenCredentialsViewModel forgottenCredentialsViewModel)
        {
            var forgottenEmailViewModel = forgottenCredentialsViewModel.ForgottenEmailViewModel;
            var validationResult = _forgottenEmailViewModelServerValidator.Validate(forgottenEmailViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.ForgottenEmail.FailedValidation, forgottenCredentialsViewModel, validationResult);
            }

            var message = string.Format(LoginPageMessages.ForgottenEmailSent, forgottenEmailViewModel.PhoneNumber);

            if (_candidateServiceProvider.RequestEmailReminder(forgottenEmailViewModel))
            {
                return GetMediatorResponse(LoginMediatorCodes.ForgottenEmail.EmailSent, forgottenCredentialsViewModel, message, UserMessageLevel.Success);
            }

            return GetMediatorResponse(LoginMediatorCodes.ForgottenEmail.FailedToSendEmail, forgottenCredentialsViewModel, message, UserMessageLevel.Success);
        }

        public MediatorResponse<PasswordResetViewModel> ResetPassword(PasswordResetViewModel resetViewModel)
        {
            //Password Reset Code is verified in VerifyPasswordReset. 
            //Initially assume the reset code is valid as a full check requires hitting the repo.
            resetViewModel.IsPasswordResetCodeValid = true;

            var validationResult = _passwordResetViewModelServerValidator.Validate(resetViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.ResetPassword.FailedValidation, resetViewModel, validationResult);
            }

            resetViewModel = _candidateServiceProvider.VerifyPasswordReset(resetViewModel);

            if (resetViewModel.HasError())
            {
                return GetMediatorResponse(LoginMediatorCodes.ResetPassword.FailedToResetPassword, resetViewModel, resetViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (resetViewModel.UserStatus == UserStatuses.Locked)
            {
                return GetMediatorResponse(LoginMediatorCodes.ResetPassword.UserAccountLocked, resetViewModel);
            }

            if (!resetViewModel.IsPasswordResetCodeValid)
            {
                validationResult = _passwordResetViewModelServerValidator.Validate(resetViewModel);
                return GetMediatorResponse(LoginMediatorCodes.ResetPassword.InvalidResetCode, resetViewModel, validationResult);
            }

            var candidate = _candidateServiceProvider.GetCandidate(resetViewModel.EmailAddress);
            SetUsersApplicationContext(candidate.EntityId);
            _authenticationTicketService.SetAuthenticationCookie(candidate.EntityId.ToString(), UserRoleNames.Activated);

            return GetMediatorResponse(LoginMediatorCodes.ResetPassword.SuccessfullyResetPassword, resetViewModel, PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success);
        }

        private void SetUsersApplicationContext(Guid candidateId)
        {
            var applications = _candidateServiceProvider.GetApprenticeshipApplications(candidateId).ToList();
            var savedAndDraftCount = applications.Count(each =>
                each.Status == ApplicationStatuses.Draft || each.Status == ApplicationStatuses.Saved);

            _userDataProvider.Push(UserDataItemNames.SavedAndDraftCount, savedAndDraftCount.ToString(CultureInfo.InvariantCulture));

            var lastApplicationStatusNotification = _userDataProvider.Get(UserDataItemNames.LastApplicationStatusNotification);

            if (!string.IsNullOrWhiteSpace(lastApplicationStatusNotification))
            {
                var applicationStatuses = new[]
                {
                    ApplicationStatuses.Successful,
                    ApplicationStatuses.Unsuccessful,
                    ApplicationStatuses.ExpiredOrWithdrawn
                };

                var lastApplicationStatusNotificationDate = new DateTime(long.Parse(lastApplicationStatusNotification), DateTimeKind.Utc);

                var applicationStatusChangeCount = applications.Count(each =>
                    each.DateUpdated > lastApplicationStatusNotificationDate &&
                    each.DateApplied.HasValue &&
                    applicationStatuses.Any(status => status == each.Status));

                if (applicationStatusChangeCount > 0)
                {
                    _userDataProvider.Push(UserDataItemNames.ApplicationStatusChangeCount, applicationStatusChangeCount.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}