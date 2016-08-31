namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Framework;
    using Common.Mediators;
    using Common.Models.Azure.AccessControlService;
    using Constants;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Reference;
    using Mediators.AgencyUser;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using Raa.Common.ViewModels.Vacancy;

    using SFA.Apprenticeships.Application.Interfaces;

    using ViewModels;
    using SFA.Infrastructure.Interfaces;
    public class AgencyUserController : ManagementControllerBase
    {
        private readonly IAgencyUserMediator _agencyUserMediator;

        public AgencyUserController(IAgencyUserMediator agencyUserMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _agencyUserMediator = agencyUserMediator;
        }

        public ActionResult Authorize()
        {
            //TODO: ACS Calls this action during signout. Need to suppress it in a cleaner manner
            if (!Request.IsAuthenticated)
            {
                return null;
            }

            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _agencyUserMediator.Authorize(claimsPrincipal);
            var message = response.Message;

            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }

            switch (response.Code)
            {
                case AgencyUserMediatorCodes.Authorize.EmptyUsername:
                case AgencyUserMediatorCodes.Authorize.MissingServicePermission:
                case AgencyUserMediatorCodes.Authorize.MissingRoleListClaim:
                    return SignOut();
                case AgencyUserMediatorCodes.Authorize.ReturnUrl:
                    return Redirect(HttpUtility.UrlDecode(response.Parameters.ToString()));
                case AgencyUserMediatorCodes.Authorize.Ok:
                    var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);
                    if (returnUrl.IsValidReturnUrl())
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult SignOut()
        {
            var callbackUrl = Url.RouteUrl(ManagementRouteNames.LandingPage, null, string.Copy(Request.Url?.Scheme ?? Constants.Url.DefaultScheme));

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);

            return null;
        }

        [HttpPost]
        public ActionResult AuthorizationError()
        {
            // This controller action is called when there is a serious ACS error (e.g. bad configuration, no claims etc.)
            var errorDetails = Request["ErrorDetails"];
            AuthorizationErrorDetailsViewModel viewModel = _agencyUserMediator.AuthorizationError(errorDetails);

            return View(viewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        [OwinSessionTimeout]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Dashboard(DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _agencyUserMediator.GetHomeViewModel(claimsPrincipal, searchViewModel);

            return View(response.ViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        [MultipleFormActionsButton(SubmitButtonActionName = "DashboardAction")]
        public ActionResult ChangeRole(HomeViewModel viewModel)
        {
            var claimsPrincipal = (ClaimsPrincipal)User;
            _agencyUserMediator.SaveAgencyUser(claimsPrincipal, viewModel.AgencyUser);

            return RedirectToRoute(ManagementRouteNames.Dashboard);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        [MultipleFormActionsButton(SubmitButtonActionName = "DashboardAction")]
        public ActionResult ChangeTeam(HomeViewModel viewModel)
        {
            var claimsPrincipal = (ClaimsPrincipal)User;
            _agencyUserMediator.SaveAgencyUser(claimsPrincipal, viewModel.AgencyUser);

            viewModel.VacancySummaries.SearchViewModel.Mode = DashboardVacancySummariesMode.Review;

            return RedirectToRoute(ManagementRouteNames.Dashboard, viewModel.VacancySummaries.SearchViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        [MultipleFormActionsButton(SubmitButtonActionName = "DashboardAction")]
        public ActionResult SearchVacancies(HomeViewModel viewModel)
        {
            return RedirectToRoute(ManagementRouteNames.Dashboard, viewModel.VacancySummaries.SearchViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult ChangeTeam(RegionalTeam regionalTeam, DashboardVacancySummaryFilterTypes filterType, string provider)
        {
            var claimsPrincipal = (ClaimsPrincipal)User;
            _agencyUserMediator.SaveAgencyUser(claimsPrincipal, new AgencyUserViewModel {RegionalTeam = regionalTeam});

            return RedirectToRoute(ManagementRouteNames.Dashboard, new DashboardVacancySummariesSearchViewModel {FilterType = filterType, Mode = DashboardVacancySummariesMode.Review, Provider = provider });
        }
    }
}