﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Common.Attributes;
    using Common.Constants;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : CandidateControllerBase
    {
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(LoginViewModelServerValidator loginViewModelServerValidator,
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService authenticationTicketService)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
            _authenticationTicketService = authenticationTicketService; //todo: shouldn't be in here, move to Provider layer
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        public ActionResult Index(string returnUrl)
        {
            _authenticationTicketService.Clear(HttpContext.Request.Cookies); //todo: yuk

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
            }

            return View();
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public ActionResult Index(LoginViewModel model)
        {
            // todo: refactor - too much going on here Provider layer
            // Validate view model.
            var validationResult = _loginViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            var result = _candidateServiceProvider.Login(model);

            if (result.UserStatus == UserStatuses.Locked)
            {
                UserData.Push(UserDataItemNames.EmailAddress, result.EmailAddress);

                return RedirectToAction("Unlock");
            }

            if (result.IsAuthenticated)
            {
                UserData.SetUserContext(result.EmailAddress, result.FullName);

                return RedirectOnAuthenticated(result.UserStatus, result.EmailAddress);
            }

            ModelState.Clear();
            ModelState.AddModelError(string.Empty, result.ViewModelMessage);

            return View(model);
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        public ActionResult Unlock()
        {
            var emailAddress = UserData.Get(UserDataItemNames.EmailAddress);
            var userStatusViewModel = _candidateServiceProvider.GetUserStatus(emailAddress);

            if (string.IsNullOrWhiteSpace(emailAddress) ||
                userStatusViewModel.UserStatus != UserStatuses.Locked)
            {
                return RedirectToAction("Index");
            }

            return View(new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public ActionResult Unlock(AccountUnlockViewModel model)
        {
            // todo: refactor - too much going on here Provider layer
            var validationResult = _accountUnlockViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            var accountUnlockViewModel = _candidateServiceProvider.VerifyAccountUnlockCode(model);
            switch (accountUnlockViewModel.Status)
            {
                case AccountUnlockState.Ok:
                    UserData.Pop(UserDataItemNames.EmailAddress);
                    SetUserMessage(AccountUnlockPageMessages.AccountUnlockedText);
                    return RedirectToAction("Index");
                case AccountUnlockState.UserInIncorrectState:
                    return RedirectToAction("Index");
                case AccountUnlockState.AccountUnlockCodeInvalid:
                    ModelState.Clear();
                    ModelState.AddModelError("AccountUnlockCode", AccountUnlockPageMessages.WrongAccountUnlockCodeErrorText);
                    return View(model);
                case AccountUnlockState.AccountUnlockCodeExpired:
                    SetUserMessage(AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                    return View(model);
                default:
                    SetUserMessage(AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
                    return View(model);
            }
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        public ActionResult ResendAccountUnlockCode(string emailAddress)
        {
            var model = new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            };

            _candidateServiceProvider.RequestAccountUnlockCode(model);

            UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

            SetUserMessage(string.Format(AccountUnlockPageMessages.AccountUnlockCodeResent, emailAddress));

            return RedirectToAction("Unlock");
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            UserData.Clear();

            SetUserMessage(SignOutPageMessages.SignOutMessageText);

            return RedirectToAction("Index");
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        public ActionResult SessionTimeout(string returnUrl)
        {
            FormsAuthentication.SignOut();
            UserData.Clear();

            SetUserMessage(SignOutPageMessages.SessionTimeoutMessageText);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserData.Push(UserDataItemNames.SessionReturnUrl, Server.UrlEncode(returnUrl));
            }

            return RedirectToAction("Index");
        }

        #region Helpers

        private ActionResult RedirectOnAuthenticated(UserStatuses userStatus, string username)
        {
            // todo: refactor - too much going on here Provider layer
            if (userStatus == UserStatuses.PendingActivation)
            {
                return RedirectToAction("Activation", "Register");
            }

            // Redirect to session return URL (if any).
            var sessionReturnUrl = UserData.Pop(UserDataItemNames.SessionReturnUrl);

            if (!string.IsNullOrWhiteSpace(sessionReturnUrl))
            {
                return Redirect(Server.UrlDecode(sessionReturnUrl));
            }

            // Redirect to return URL (if any).
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(Server.UrlDecode(returnUrl));
            }

            // Redirect to last viewed vacancy (if any).
            var lastViewedVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

            if (lastViewedVacancyId != null)
            {
                var candidate = _candidateServiceProvider.GetCandidate(username);

                var applicationStatus = _candidateServiceProvider.GetApplicationStatus(
                    candidate.EntityId, int.Parse(lastViewedVacancyId));

                if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                {
                    return RedirectToAction("Apply", "Application", new { id = lastViewedVacancyId });
                }

                return RedirectToAction("Details", "VacancySearch", new { id = lastViewedVacancyId });
            }

            return RedirectToRoute(CandidateRouteNames.MyApplications);
        }

        #endregion
    }
}
