namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Attributes;
    using Common.Controllers;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using Constants.ViewModels;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators.ProviderUser;
    using Providers;
    using ViewModels;
    using ViewModels.ProviderUser;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class ProviderUserController : ControllerBase<RecuitmentUserContext>
    {
        private readonly IProviderUserMediator _providerUserMediator;
        private readonly ICookieAuthorizationDataProvider _cookieAuthorizationDataProvider;

        public ProviderUserController(IProviderUserMediator providerUserMediator, ICookieAuthorizationDataProvider cookieAuthorizationDataProvider)
        {
            _providerUserMediator = providerUserMediator;
            _cookieAuthorizationDataProvider = cookieAuthorizationDataProvider;
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
                    _cookieAuthorizationDataProvider.RemoveClaim(System.Security.Claims.ClaimTypes.Role, Roles.VerifiedEmail, HttpContext, User.Identity.Name);
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
                    _cookieAuthorizationDataProvider.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Role, Roles.VerifiedEmail), HttpContext, User.Identity.Name);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult ResendVertificationCode()
        {
            var providerUserViewModel = _providerUserMediator.GetProviderUserViewModel(User.Identity.Name);
            SetUserMessage(string.Format(VerifyEmailViewModelMessages.VerificationCodeEmailResentMessage, providerUserViewModel.ViewModel.EmailAddress));
            return View("VerifyEmail");
        }
    }
}