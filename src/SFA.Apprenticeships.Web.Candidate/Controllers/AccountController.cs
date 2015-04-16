﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Account;
    using ViewModels.Account;

    public class AccountController : CandidateControllerBase
    {
        private readonly IAccountMediator _accountMediator;
        private readonly IDismissPlannedOutageMessageCookieProvider _dismissPlannedOutageMessageCookieProvider;

        public AccountController(IAccountMediator accountMediator, 
            IDismissPlannedOutageMessageCookieProvider dismissPlannedOutageMessageCookieProvider,
            IConfigurationService configurationService) : base(configurationService)
        {
            _accountMediator = accountMediator;
            _dismissPlannedOutageMessageCookieProvider = dismissPlannedOutageMessageCookieProvider;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Index(UserContext.CandidateId, UserData.Pop(CandidateDataItemNames.DeletedVacancyId), UserData.Pop(CandidateDataItemNames.DeletedVacancyTitle));
                return View(response.ViewModel);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Settings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId, SettingsViewModel.SettingsMode.YourAccount);
                return View(response.ViewModel);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> SavedSearchesSettings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId, SettingsViewModel.SettingsMode.SavedSearches);
                return View("Settings", response.ViewModel);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Settings(SettingsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
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
                        return RedirectToAction("VerifyMobile");
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

        [SmsEnabledToggle]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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
        [MultipleFormActionsButton(Name = "VerifyMobileAction", Argument = "VerifyMobile")]
        public async Task<ActionResult> VerifyMobile(VerifyMobileViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.VerifyMobile(UserContext.CandidateId, model);
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
        [MultipleFormActionsButton(Name = "VerifyMobileAction", Argument = "Resend")]
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
                return RedirectToAction("VerifyMobile");
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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
                        return RedirectToRoute(RouteNames.VertifyUpdatedEmail);
                    case AccountMediatorCodes.UpdateEmailAddress.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return View(response.ViewModel);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> ResendUpdateEmailAddressCode()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.ResendUpdateEmailAddressCode(UserContext.CandidateId);
                SetUserMessage(response.Message.Text, response.Message.Level);
                return RedirectToRoute(RouteNames.VertifyUpdatedEmail);
            });
        }

        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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

        public async Task<ActionResult> DismissPlannedOutageMessage(bool isJavascript)
        {
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

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Track(int id)
        {
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

        public async Task<ActionResult> AcceptTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (UserContext == null)
                {
                    //Check needed as AuthorizeCandidate attribute not on action
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
