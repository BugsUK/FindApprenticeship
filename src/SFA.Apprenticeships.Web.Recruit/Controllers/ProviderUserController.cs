namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Extensions;
    using Common.Framework;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.ProviderUser;
    using Providers;
    using ViewModels;
    using ViewModels.ProviderUser;
    using ClaimTypes = System.Security.Claims.ClaimTypes;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class ProviderUserController : ControllerBase<RecuitmentUserContext>
    {
        private readonly IProviderUserMediator _providerUserMediator;
        private readonly ICookieAuthorizationDataProvider _cookieAuthorizationDataProvider;

        public ProviderUserController(
            IProviderUserMediator providerUserMediator,
            ICookieAuthorizationDataProvider cookieAuthorizationDataProvider)
        {
            _providerUserMediator = providerUserMediator;
            _cookieAuthorizationDataProvider = cookieAuthorizationDataProvider;
        }

        public ActionResult Authorize()
        {
            //TODO: ACS Calls this action during signout. Need to suppress it in a cleaner manner
            if (!Request.IsAuthenticated)
            {
                return null;
            }

            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _providerUserMediator.Authorize(claimsPrincipal);
            var message = response.Message;
            var viewModel = response.ViewModel;

            //Clear existing claims
            _cookieAuthorizationDataProvider.Clear(HttpContext);

            //Add domain claims
            if (viewModel.EmailAddress != null)
            {
                AddClaim(ClaimTypes.Email, viewModel.EmailAddress, viewModel);
            }
            if (viewModel.EmailAddressVerified)
            {
                AddClaim(ClaimTypes.Role, Roles.VerifiedEmail, viewModel);
            }

            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }

            switch (response.Code)
            {
                case ProviderUserMediatorCodes.Authorize.EmptyUsername:
                case ProviderUserMediatorCodes.Authorize.MissingProviderIdentifier:
                case ProviderUserMediatorCodes.Authorize.MissingServicePermission:
                    _cookieAuthorizationDataProvider.Clear(HttpContext);

                    return RedirectToRoute(RecruitmentRouteNames.SignOut, new
                    {
                        returnRoute = RecruitmentRouteNames.LandingPage
                    });

                case ProviderUserMediatorCodes.Authorize.NoProviderProfile:
                case ProviderUserMediatorCodes.Authorize.FailedMinimumSitesCountCheck:
                case ProviderUserMediatorCodes.Authorize.FirstUser:
                    return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);

                case ProviderUserMediatorCodes.Authorize.NoUserProfile:
                    return RedirectToRoute(RecruitmentRouteNames.Settings);

                case ProviderUserMediatorCodes.Authorize.EmailAddressNotVerified:
                    return RedirectToRoute(RecruitmentRouteNames.VerifyEmail);

                case ProviderUserMediatorCodes.Authorize.Ok:
                    var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

                    if (returnUrl.IsValidReturnUrl())
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }

                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult AuthorizationError()
        {
            // This controller action is called when there is a serious ACS error (e.g. bad configuration, no claims etc.)
            var errorDetails = Request["ErrorDetails"];
            var viewModel = _providerUserMediator.AuthorizationError(errorDetails);

            return View(viewModel);
        }

        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Settings()
        {
            var response = _providerUserMediator.GetSettingsViewModel(User.Identity.Name, User.GetUkprn());

            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult Settings(SettingsViewModel settingsViewModel)
        {
            var response = _providerUserMediator.UpdateUser(User.Identity.Name, User.GetUkprn(), settingsViewModel.ProviderUserViewModel);

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case ProviderUserMediatorCodes.UpdateUser.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(settingsViewModel);

                case ProviderUserMediatorCodes.UpdateUser.EmailUpdated:
                    _cookieAuthorizationDataProvider.RemoveClaim(ClaimTypes.Role, Roles.VerifiedEmail, HttpContext, User.Identity.Name);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                case ProviderUserMediatorCodes.UpdateUser.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VerifyEmail()
        {
            var providerUserViewModel = _providerUserMediator.GetProviderUserViewModel(User.Identity.Name);
            var verifyEmailViewModel = new VerifyEmailViewModel
            {
                EmailAddress = providerUserViewModel.ViewModel.EmailAddress
            };

            return View(verifyEmailViewModel);
        }

        [HttpPost]
        public ActionResult VerifyEmail(VerifyEmailViewModel verifyEmailViewModel)
        {
            var response = _providerUserMediator.VerifyEmailAddress(User.Identity.Name, verifyEmailViewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case ProviderUserMediatorCodes.VerifyEmailAddress.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(verifyEmailViewModel);
                case ProviderUserMediatorCodes.VerifyEmailAddress.InvalidCode:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View(verifyEmailViewModel);
                case ProviderUserMediatorCodes.VerifyEmailAddress.Ok:
                    _cookieAuthorizationDataProvider.AddClaim(new Claim(ClaimTypes.Role, Roles.VerifiedEmail), HttpContext, User.Identity.Name);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult ResendVerificationCode()
        {
            var response = _providerUserMediator.ResendVerificationCode(User.Identity.Name);
            var verifyEmailViewModel = response.ViewModel;
            var message = response.Message;

            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }

            return View("VerifyEmail", verifyEmailViewModel);
        }

        #region Helpers

        private void AddClaim(string type, string value, AuthorizeResponseViewModel viewModel)
        {
            var claim = new Claim(type, value);

            _cookieAuthorizationDataProvider.AddClaim(claim, HttpContext, viewModel.Username);
        }

        #endregion
    }
}