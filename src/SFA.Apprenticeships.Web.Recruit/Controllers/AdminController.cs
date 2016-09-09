namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Common.Extensions;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Raa;
    using ViewModels.Admin;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.Admin)]
    public class AdminController : RecruitmentControllerBase
    {
        private readonly ICookieAuthorizationDataProvider _cookieAuthorizationDataProvider;

        public AdminController(ICookieAuthorizationDataProvider cookieAuthorizationDataProvider, IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
            _cookieAuthorizationDataProvider = cookieAuthorizationDataProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUkprn()
        {
            return View(new ChangeUkprnViewModel {Ukprn = User.GetUkprn()});
        }

        [HttpPost]
        public ActionResult ChangeUkprn(ChangeUkprnViewModel viewModel)
        {
            var claim = new Claim(Common.Constants.ClaimTypes.UkprnOverride, viewModel.Ukprn);

            _cookieAuthorizationDataProvider.AddClaim(claim, HttpContext, User.Identity.Name);

            SetUserMessage($"Your UKPRN has been changed to {viewModel.Ukprn}");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }

        [HttpGet]
        public ActionResult ResetUkprn()
        {
            _cookieAuthorizationDataProvider.RemoveClaim(Common.Constants.ClaimTypes.UkprnOverride, User.GetUkprn(), HttpContext, User.Identity.Name);
            
            SetUserMessage("Your UKPRN has been reset");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }
    }
}