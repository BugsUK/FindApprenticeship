namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Raa;
    using Mediators.Admin;
    using System.Security.Claims;
    using System.Web.Mvc;
    using ViewModels.Admin;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.Admin)]
    public class AdminController : RecruitmentControllerBase
    {
        private readonly ICookieAuthorizationDataProvider _cookieAuthorizationDataProvider;
        private readonly IAdminMediator _adminMediator;

        public AdminController(ICookieAuthorizationDataProvider cookieAuthorizationDataProvider,
            IConfigurationService configurationService, ILogService loggingService, IAdminMediator adminMediator)
            : base(configurationService, loggingService)
        {
            _cookieAuthorizationDataProvider = cookieAuthorizationDataProvider;
            _adminMediator = adminMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUkprn()
        {
            return View(new ChangeUkprnViewModel { Ukprn = User.GetUkprn() });
        }

        [HttpPost]
        public ActionResult ChangeUkprn(ChangeUkprnViewModel viewModel)
        {
            var claim = new Claim(Common.Constants.ClaimTypes.UkprnOverride, viewModel.Ukprn);

            RemoveUkprnOverride();
            _cookieAuthorizationDataProvider.AddClaim(claim, HttpContext, User.Identity.Name);

            SetUserMessage($"Your UKPRN has been changed to {viewModel.Ukprn}");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }

        [HttpGet]
        public ActionResult ResetUkprn()
        {
            RemoveUkprnOverride();

            SetUserMessage("Your UKPRN has been reset");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }

        private void RemoveUkprnOverride()
        {
            _cookieAuthorizationDataProvider.RemoveClaim(Common.Constants.ClaimTypes.UkprnOverride, User.GetUkprn(), HttpContext,
                User.Identity.Name);
        }

        public ActionResult TransferVacancies()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferVacancies(TransferVacanciesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = _adminMediator.GetVacancyDetails(viewModel);
                ModelState.Clear();

                switch (response.Code)
                {
                    case AdminMediatorCodes.GetVacancyDetails.Ok:
                        return View(response.ViewModel);
                    case AdminMediatorCodes.GetVacancyDetails.FailedAuthorisation:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

            }
            return View();
        }
    }
}