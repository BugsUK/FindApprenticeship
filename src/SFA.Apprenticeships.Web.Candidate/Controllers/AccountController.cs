using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Mediators.Account;
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using ViewModels.Account;
    using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

    public class AccountController : CandidateControllerBase
    {
        private readonly IAccountMediator _accountMediator;
        private readonly IDismissPlannedOutageMessageCookieProvider _dismissPlannedOutageMessageCookieProvider;
        private readonly IUserDataProvider _userDataProvider;

        public AccountController(IAccountMediator accountMediator,
            IDismissPlannedOutageMessageCookieProvider dismissPlannedOutageMessageCookieProvider,
            IConfigurationService configurationService,
            IUserDataProvider userDataProvider,
            ILogService logService)
            : base(configurationService, logService)
        {
            _accountMediator = accountMediator;
            _dismissPlannedOutageMessageCookieProvider = dismissPlannedOutageMessageCookieProvider;
            _userDataProvider = userDataProvider;
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Index(UserContext.CandidateId, UserData.Pop(CandidateDataItemNames.DeletedVacancyId), UserData.Pop(CandidateDataItemNames.DeletedVacancyTitle));
                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> DismissApplicationNotifications(long lastupdated)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var utcDateTime = new DateTime(lastupdated + 1, DateTimeKind.Utc);
                _userDataProvider.Push(UserDataItemNames.LastApplicationStatusNotification, utcDateTime.Ticks.ToString(CultureInfo.InvariantCulture));
                _userDataProvider.Pop(UserDataItemNames.ApplicationStatusChangeCount);
                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> Settings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId, SettingsViewModel.SettingsMode.YourAccount);
                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> SavedSearchesSettings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId, SettingsViewModel.SettingsMode.SavedSearches);
                return View("Settings", response.ViewModel);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> DeleteAccountSettings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId, SettingsViewModel.SettingsMode.DeleteAccount);
                return View("Settings", response.ViewModel);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> ConfirmAccountSettings(SettingsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                DeleteAccountSettingsViewModel deleteAccountSettingsViewModel = new DeleteAccountSettingsViewModel()
                {
                    EmailAddress = model.EmailAddress,
                    Password = model.Password
                };
                var response = _accountMediator.VerifyAccountSettings(UserContext.CandidateId, deleteAccountSettingsViewModel);
                ModelState.Clear();

                switch (response.Code)
                {
                    case AccountMediatorCodes.ValidateUserAccountBeforeDelete.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Settings", model);
                    case AccountMediatorCodes.ValidateUserAccountBeforeDelete.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View("Settings", model);
                    case AccountMediatorCodes.ValidateUserAccountBeforeDelete.Ok:
                        return View("ConfirmAccountDeletion", model);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Settings(SettingsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (model.Mode == SettingsViewModel.SettingsMode.DeleteAccount)
                    return RedirectToAction("ConfirmAccountSettings", model);
                var response = _accountMediator.SaveSettings(UserContext.CandidateId, model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case AccountMediatorCodes.Settings.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.Settings.SaveError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.Settings.MobileVerificationRequired:
                        return RedirectToRoute(CandidateRouteNames.VerifyMobile);
                    case AccountMediatorCodes.Settings.Success:
                    case AccountMediatorCodes.Settings.SuccessWithWarning:
                        UserData.SetUserContext(UserContext.UserName, response.ViewModel.Firstname + " " + response.ViewModel.Lastname, UserContext.AcceptedTermsAndConditionsVersion);
                        if (response.Code == AccountMediatorCodes.Settings.SuccessWithWarning)
                            SetUserMessage(response.Message.Text, response.Message.Level);
                        else
                            SetUserMessage(AccountPageMessages.SettingsUpdated);
                        return RedirectToRoute(response.ViewModel.Mode == SettingsViewModel.SettingsMode.SavedSearches ? CandidateRouteNames.SavedSearchesSettings : CandidateRouteNames.Settings);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> DeleteSavedSearch(Guid id, bool isJavascript)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.DeleteSavedSearch(UserContext.CandidateId, id);

                if (isJavascript)
                {
                    if (response.Code == AccountMediatorCodes.DeleteSavedSearch.Ok)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                switch (response.Code)
                {
                    case AccountMediatorCodes.DeleteSavedSearch.Ok:
                        return RedirectToRoute(CandidateRouteNames.SavedSearchesSettings);
                    case AccountMediatorCodes.DeleteSavedSearch.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.SavedSearchesSettings);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [SmsEnabledToggle]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> VerifyMobile(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.VerifyMobile(UserContext.CandidateId, returnUrl);

                switch (response.Code)
                {
                    case AccountMediatorCodes.VerifyMobile.Success:
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.VerificationNotRequired:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.Error:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error);
                        return View(response.ViewModel);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });


        }

        [HttpPost]
        [SmsEnabledToggle]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "VerifyMobileAction")]
        public async Task<ActionResult> VerifyMobile(VerifyMobileViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.VerifyMobile(UserContext.CandidateId, model);
                if (response.ViewModel.TraineeshipFeature == null)
                    _logService.Error("response.ViewModel.TraineeshipFeature == null. Relates to issue with Logstash _id:QWlNNMzdRM2j8q6681rZTg");
                ModelState.Clear();

                switch (response.Code)
                {
                    case AccountMediatorCodes.VerifyMobile.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.InvalidCode:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationCodeInvalid, UserMessageLevel.Error);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.VerificationNotRequired:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.Error:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.Success:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationSuccessText);
                        if (model.ReturnUrl.IsValidReturnUrl())
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToRoute(CandidateRouteNames.Settings);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [SmsEnabledToggle]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "VerifyMobileAction")]
        public async Task<ActionResult> Resend(VerifyMobileViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Resend(UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Resend.Error:
                    case AccountMediatorCodes.Resend.ResendNotRequired:
                    case AccountMediatorCodes.Resend.ResentSuccessfully:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
                return RedirectToRoute(CandidateRouteNames.VerifyMobile);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> UpdateEmailAddress()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> UpdateEmailAddress(EmailViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.UpdateEmailAddress(UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case AccountMediatorCodes.UpdateEmailAddress.Ok:
                        SetUserMessage(response.Message.Text);
                        return RedirectToRoute(RouteNames.VerifyUpdatedEmail);
                    case AccountMediatorCodes.UpdateEmailAddress.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> ResendUpdateEmailAddressCode()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.ResendUpdateEmailAddressCode(UserContext.CandidateId);
                SetUserMessage(response.Message.Text, response.Message.Level);
                return RedirectToRoute(RouteNames.VerifyUpdatedEmail);
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> VerifyUpdatedEmailAddress()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult VerifyUpdatedEmailAddress(VerifyUpdatedEmailViewModel model)
        {
            var response = _accountMediator.VerifyUpdatedEmailAddress(UserContext.CandidateId, model);

            switch (response.Code)
            {
                case AccountMediatorCodes.VerifyUpdatedEmailAddress.Ok:
                    SetUserMessage(response.Message.Text);
                    FormsAuthentication.SignOut();
                    return RedirectToRoute(RouteNames.SignIn);
                case AccountMediatorCodes.VerifyUpdatedEmailAddress.ValidationError:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    break;
                case AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    break;
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

            return View(response.ViewModel);
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Archive(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Archive(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Archive.SuccessfullyArchived:
                        SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);
                        break;
                    case AccountMediatorCodes.Archive.ErrorArchiving:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Delete(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Delete(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Delete.SuccessfullyDeleted:
                        UserData.Push(CandidateDataItemNames.DeletedVacancyId, id.ToString(CultureInfo.InvariantCulture));
                        UserData.Push(CandidateDataItemNames.DeletedVacancyTitle, response.Message.Text);
                        break;
                    case AccountMediatorCodes.Delete.AlreadyDeleted:
                    case AccountMediatorCodes.Delete.ErrorDeleting:
                    case AccountMediatorCodes.Delete.SuccessfullyDeletedExpiredOrWithdrawn:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        public async Task<ActionResult> DismissPlannedOutageMessage(bool isJavascript)
        {
            //todo: move to home controller as not related to the user account views
            return await Task.Run<ActionResult>(() =>
            {
                _dismissPlannedOutageMessageCookieProvider.SetCookie(HttpContext);

                if (isJavascript)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }

                return RedirectToRoute(RouteNames.SignIn);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> DismissTraineeshipPrompts()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.DismissTraineeshipPrompts(UserContext.CandidateId);

                switch (response.Code)
                {
                    case AccountMediatorCodes.DismissTraineeshipPrompts.SuccessfullyDismissed:
                        break;
                    case AccountMediatorCodes.DismissTraineeshipPrompts.ErrorDismissing:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Track(int id)
        {
            // called when a user clicks "Track application status" from vacancy details for a live vacancy previously applied for
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Track(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Track.SuccessfullyTracked:
                    case AccountMediatorCodes.Track.ErrorTracking:
                        // Tracking an application is 'best efforts'. Errors are not reported to the user.
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> UpdatedTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (returnUrl.IsValidReturnUrl())
                {
                    var routeValueDictionary = new { ReturnUrl = returnUrl };
                    return View("Terms", routeValueDictionary);
                }

                return View("Terms");
            });
        }

        [HttpGet]
        public async Task<ActionResult> AcceptTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (UserContext == null)
                {
                    //todo: check needed as AuthorizeCandidate attribute not on action
                    return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
                }

                var response = _accountMediator.AcceptTermsAndConditions(UserContext.CandidateId);

                switch (response.Code)
                {
                    case AccountMediatorCodes.AcceptTermsAndConditions.SuccessfullyAccepted:
                    case AccountMediatorCodes.AcceptTermsAndConditions.AlreadyAccepted:
                        break;
                    case AccountMediatorCodes.AcceptTermsAndConditions.ErrorAccepting:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                if (returnUrl.IsValidReturnUrl())
                {
                    return Redirect(returnUrl);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        public async Task<ActionResult> DeclineTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                SetUserMessage(SignOutPageMessages.MustAcceptUpdatedTermsAndConditions, UserMessageLevel.Warning);

                return returnUrl.IsValidReturnUrl()
                    ? RedirectToRoute(RouteNames.SignOut, new { ReturnUrl = returnUrl })
                    : RedirectToRoute(RouteNames.SignOut);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> ApprenticeshipVacancyDetails(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.ApprenticeshipVacancyDetails(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.VacancyDetails.Available:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });

                    case AccountMediatorCodes.VacancyDetails.Unavailable:
                    case AccountMediatorCodes.VacancyDetails.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> TraineeshipVacancyDetails(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.TraineeshipVacancyDetails(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.VacancyDetails.Available:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });

                    case AccountMediatorCodes.VacancyDetails.Unavailable:
                    case AccountMediatorCodes.VacancyDetails.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }
    }
}
