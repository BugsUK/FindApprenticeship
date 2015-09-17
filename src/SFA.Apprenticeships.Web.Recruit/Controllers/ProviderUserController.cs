namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using Constants.ViewModels;
    using FluentValidation.Mvc;
    using Mediators.ProviderUser;
    using Providers;
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
            LoadTestSites();
            return View(new ProviderUserViewModel());
        }

        private void LoadTestSites()
        {
            var sites = new List<SelectListItem>();
            sites.Add(new SelectListItem {Value = "1", Text = "Basing View, Basingstoke", Selected = true});
            sites.Add(new SelectListItem {Value = "2", Text = "Great Charles Street Queensway, Birmingham"});
            sites.Add(new SelectListItem {Value = "3", Text = "St. Helens Street, Ipswich"});
            sites.Add(new SelectListItem {Value = "4", Text = "South Parade, Leeds"});
            sites.Add(new SelectListItem {Value = "5", Text = "24-26 Baltic Street West, London"});
            sites.Add(new SelectListItem {Value = "6", Text = "Dean Street, Newcastle upon Tyne"});
            sites.Add(new SelectListItem {Value = "7", Text = "Judson Road, Peterlee"});
            sites.Add(new SelectListItem {Value = "8", Text = "David Murray John Tower, Swindon"});
            sites.Add(new SelectListItem {Value = "9", Text = "6-10 Gills Yard, Wakefield"});
            sites.Add(new SelectListItem {Value = "10", Text = "Sheep Street, Wellingborough"});
            sites.Add(new SelectListItem {Value = "11", Text = "Sheet Street, Windsor"});

            ViewBag.Sites = sites;
        }

        [HttpPost]
        public ActionResult Settings(ProviderUserViewModel providerUserViewModel)
        {
            var response = _providerUserMediator.UpdateUser(User.Identity.Name, providerUserViewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case ProviderUserMediatorCodes.UpdateUser.FailedValidation:
                    LoadTestSites();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(providerUserViewModel);
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