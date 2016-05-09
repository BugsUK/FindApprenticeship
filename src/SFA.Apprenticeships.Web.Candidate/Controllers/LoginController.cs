using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using SFA.Infrastructure.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Framework;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Login;
    using Providers;
    using ViewModels.Login;
    using ViewModels.Register;

    public class LoginController : CandidateControllerBase
    {
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ILoginMediator _loginMediator;

        public LoginController(IAuthenticationTicketService authenticationTicketService,
            ICandidateServiceProvider candidateServiceProvider,
            ILoginMediator loginMediator,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _authenticationTicketService = authenticationTicketService; //todo: shouldn't be in here, move to Provider layer?
            _candidateServiceProvider = candidateServiceProvider; //todo: shouldn't be in here, move to Provider layer?
            _loginMediator = loginMediator;
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> Index(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                _authenticationTicketService.Clear();

                if (returnUrl.IsValidReturnUrl())
                {
                    UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
                }

                return View();
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                var response = _loginMediator.Index(model);
                var viewModel = response.ViewModel;

                if (viewModel != null && viewModel.IsAuthenticated)
                {
                    SetLoggedInUserMessages(viewModel);
                }

                switch (response.Code)
                {
                    case LoginMediatorCodes.Index.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case LoginMediatorCodes.Index.AccountLocked:
                        return RedirectToRoute(RouteNames.Unlock);

                    case LoginMediatorCodes.Index.ApprenticeshipApply:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipApply, new { id = response.Parameters.ToString() });

                    case LoginMediatorCodes.Index.ApprenticeshipDetails:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id = response.Parameters.ToString() });

                    case LoginMediatorCodes.Index.TraineeshipDetails:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id = response.Parameters.ToString() });

                    case LoginMediatorCodes.Index.ReturnUrl:
                        return Redirect(HttpUtility.UrlDecode(response.Parameters.ToString()));

                    case LoginMediatorCodes.Index.Ok:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);

                    case LoginMediatorCodes.Index.PendingActivation:
                        return RedirectToRoute(RouteNames.Activation);

                    case LoginMediatorCodes.Index.LoginFailed:
                        ModelState.AddModelError(string.Empty, response.Parameters.ToString());
                        return View(model);

                    case LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted:
                        if (response.Parameters != null)
                        {
                            var returnUrl = new { ReturnUrl = HttpUtility.UrlDecode(response.Parameters.ToString()) };
                            return RedirectToRoute(RouteNames.UpdatedTermsAndConditions, returnUrl);
                        }
                        return RedirectToRoute(RouteNames.UpdatedTermsAndConditions);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [SessionTimeout]
        public async Task<ActionResult> Unlock()
        {
            return await Task.Run(() =>
            {
                var emailAddress = UserData.Pop(UserDataItemNames.UnlockEmailAddress);
                return View(new AccountUnlockViewModel { EmailAddress = emailAddress });
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleFormActionsButton(SubmitButtonActionName = "LoginAction")]
        public async Task<ActionResult> Unlock(AccountUnlockViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.Unlock(model);

                switch (response.Code)
                {
                    case LoginMediatorCodes.Unlock.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case LoginMediatorCodes.Unlock.UnlockedSuccessfully:
                        UserData.Pop(UserDataItemNames.EmailAddress);
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockedText);
                        return RedirectToRoute(RouteNames.SignIn);

                    case LoginMediatorCodes.Unlock.UserInIncorrectState:
                        return RedirectToRoute(RouteNames.SignIn);

                    case LoginMediatorCodes.Unlock.AccountEmailAddressOrUnlockCodeInvalid:
                        SetUserMessage(AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                        return View(model);

                    case LoginMediatorCodes.Unlock.AccountUnlockCodeExpired:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                        return View(model);

                    case LoginMediatorCodes.Unlock.AccountUnlockFailed:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
                        return View(model);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [AllowReturnUrl(Allow = false)]
        [MultipleFormActionsButton(SubmitButtonActionName = "LoginAction")]
        public async Task<ActionResult> Resend(AccountUnlockViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.Resend(model);

                UserData.Push(UserDataItemNames.UnlockEmailAddress, model.EmailAddress);

                switch (response.Code)
                {
                    case LoginMediatorCodes.Resend.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case LoginMediatorCodes.Resend.ResendFailed:
                    case LoginMediatorCodes.Resend.ResentSuccessfully:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(RouteNames.Unlock);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        public ActionResult SignOut(string returnUrl)
        {
            //Get values to maintain between sessions
            var userJourneyValue = UserData.Get(UserJourneyContextAttribute.UserJourneyKey);
            var resultPerPageValue = UserData.Get(UserDataItemNames.ResultsPerPage);
            var loggingSessionIdValue = UserData.Get(UserDataItemNames.LoggingSessionId);
            var lastSearchedLocationValue = UserData.Get(UserDataItemNames.LastSearchedLocation);

            FormsAuthentication.SignOut();

            if (UserData.Get(UserMessageConstants.WarningMessage) == SignOutPageMessages.MustAcceptUpdatedTermsAndConditions)
            {
                UserData.Clear();
                SetUserMessage(SignOutPageMessages.MustAcceptUpdatedTermsAndConditions, UserMessageLevel.Warning);
            }
            else
            {
                UserData.Clear();
                var signOutMessage = string.Format(SignOutPageMessages.SignOutMessageText, ViewBag.FeedbackUrl);
                SetUserMessage(signOutMessage);
            }

            //Reset values to maintain between sessions
            UserData.Push(UserJourneyContextAttribute.UserJourneyKey, userJourneyValue);
            UserData.Push(UserDataItemNames.ResultsPerPage, resultPerPageValue);
            UserData.Push(UserDataItemNames.LoggingSessionId, loggingSessionIdValue);
            UserData.Push(UserDataItemNames.LastSearchedLocation, lastSearchedLocationValue);

            if (ViewBag.FeedbackUrl == returnUrl)
            {
                return Redirect(ViewBag.FeedbackUrl);
            }

            return returnUrl.IsValidReturnUrl()
                ? RedirectToRoute(RouteNames.SignIn, new { ReturnUrl = returnUrl })
                : RedirectToRoute(RouteNames.SignIn);
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        public ActionResult SessionTimeout(string returnUrl)
        {
            const string userJourneyKey = "UserJourney";
            var userJourneyValue = UserData.Get(userJourneyKey);

            var userContext = UserData.GetUserContext();

            FormsAuthentication.SignOut();
            UserData.Clear();

            UserData.Push(userJourneyKey, userJourneyValue);

            if (userContext == null)
            {
                _logService.Info("User session timeout");
            }
            else
            {
                _logService.Info("User {0} session timeout", userContext.UserName);
                //Only set the message if the user context was set by a previous login action.
                //This means that the session has timed out rather than becoming invalid after closing the browser.
                SetUserMessage(SignOutPageMessages.SessionTimeoutMessageText);
            }

            if (returnUrl.IsValidReturnUrl())
            {
                UserData.Push(UserDataItemNames.SessionReturnUrl, Server.UrlEncode(returnUrl));
            }

            return RedirectToRoute(RouteNames.SignIn);
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [SessionTimeout]
        public async Task<ActionResult> ForgottenCredentials()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgottenPassword(ForgottenCredentialsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.ForgottenPassword(model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case LoginMediatorCodes.ForgottenPassword.FailedValidation:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(RouteNames.ForgottenCredentials, response.ViewModel);
                    case LoginMediatorCodes.ForgottenPassword.FailedToSendResetCode:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(RouteNames.ForgottenCredentials, response.ViewModel);
                    case LoginMediatorCodes.ForgottenPassword.PasswordSent:
                        UserData.Push(UserDataItemNames.EmailAddress, model.ForgottenPasswordViewModel.EmailAddress);
                        return RedirectToRoute(RouteNames.ResetPassword);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgottenEmail(ForgottenCredentialsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.ForgottenEmail(model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case LoginMediatorCodes.ForgottenEmail.FailedValidation:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(RouteNames.ForgottenCredentials, response.ViewModel);
                    case LoginMediatorCodes.ForgottenEmail.FailedToSendEmail:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(RouteNames.SignIn);
                    case LoginMediatorCodes.ForgottenEmail.EmailSent:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(RouteNames.SignIn);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [SessionTimeout]
        public async Task<ActionResult> ResetPassword()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var emailAddress = UserData.Get(UserDataItemNames.EmailAddress);

                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    return RedirectToRoute(RouteNames.ForgottenCredentials);
                }

                var model = new PasswordResetViewModel { EmailAddress = emailAddress };
                return View(model);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowReturnUrl(Allow = false)]
        [ValidateInput(false)]
        public async Task<ActionResult> ResetPassword(PasswordResetViewModel model)
        {
            return await Task.Run(() =>
            {
                var response = _loginMediator.ResetPassword(model);

                switch (response.Code)
                {
                    case LoginMediatorCodes.ResetPassword.FailedValidation:
                    case LoginMediatorCodes.ResetPassword.InvalidResetCode:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);

                    case LoginMediatorCodes.ResetPassword.FailedToResetPassword:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(model);

                    case LoginMediatorCodes.ResetPassword.UserAccountLocked:
                        UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                        return RedirectToRoute(RouteNames.Unlock);

                    case LoginMediatorCodes.ResetPassword.SuccessfullyResetPassword:
                        SetUserMessage(response.Message.Text);
                        return SetUserContextAndRedirectToAction(model.EmailAddress);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);

                }
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [SessionTimeout]
        public async Task<ActionResult> ResendPasswordResetCode(string emailAddress)
        {
            return await Task.Run(() =>
            {
                var model = new ForgottenPasswordViewModel { EmailAddress = emailAddress };

                UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

                if (_candidateServiceProvider.RequestForgottenPasswordResetCode(model))
                {
                    SetUserMessage(string.Format(PasswordResetPageMessages.PasswordResetSent, emailAddress));
                }
                else
                {
                    SetUserMessage(PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning);
                }

                return RedirectToRoute(RouteNames.ResetPassword);
            });
        }

        #region Helpers

        private void SetLoggedInUserMessages(LoginResultViewModel viewModel)
        {
            var message = "";

            if (viewModel.PendingUsernameVerificationRequired)
            {
                message += string.Format(LoginPageMessages.PendingUsernameVerificationRequiredText,
                    Url.RouteUrl(RouteNames.VerifyUpdatedEmail));

                SetUserMessage(message, UserMessageLevel.Info);
            }

            if (viewModel.MobileVerificationRequired)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "<br/><br/>";
                }

                message += string.Format(LoginPageMessages.MobileVerificationRequiredText, viewModel.PhoneNumber,
                    Url.RouteUrl(CandidateRouteNames.VerifyMobile,
                        new RouteValueDictionary
                        {
                            {
                                "ReturnUrl", viewModel.ReturnUrl
                            }
                        }));

                SetUserMessage(message, UserMessageLevel.Info);
            }
        }

        private ActionResult SetUserContextAndRedirectToAction(string candidateEmail)
        {
            var candidate = _candidateServiceProvider.GetCandidate(candidateEmail);
            //todo: refactor - similar to stuff in login controller... move to ILoginServiceProvider
            //todo: test this
            UserData.SetUserContext(candidate.RegistrationDetails.EmailAddress,
                candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion);

            // ReturnUrl takes precedence over last view vacnacy id.
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            // Clear last viewed vacancy and distance (if any).
            var lastViewedVacancy = UserData.PopLastViewedVacancy();
            UserData.Pop(CandidateDataItemNames.VacancyDistance);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(Server.UrlDecode(returnUrl));
            }

            if (lastViewedVacancy != null)
            {
                switch (lastViewedVacancy.Type)
                {
                    case VacancyType.Apprenticeship:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id = lastViewedVacancy.Id });
                    case VacancyType.Traineeship:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id = lastViewedVacancy.Id });
                }
            }

            return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
        }

        #endregion
    }
}
