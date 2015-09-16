using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Common.Controllers;
    using Constants;
    using Providers;

    public class HomeController : ControllerBase<RecuitmentUserContext>
    {
        // private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly IHomeMediator _homeMediator;

        public HomeController(IHomeMediator homeMediator)
        {
            // _authenticationTicketService = authenticationTicketService;
            _homeMediator = homeMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _homeMediator.Authorize(claimsPrincipal);

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case HomeMediatorCodes.Authorize.EmptyUsername:
                case HomeMediatorCodes.Authorize.MissingProviderIdentifier:
                case HomeMediatorCodes.Authorize.MissingServicePermission:
                case HomeMediatorCodes.Authorize.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.LandingPage);
                case HomeMediatorCodes.Authorize.NoProviderProfile:
                case HomeMediatorCodes.Authorize.FailedMinimumSitesCountCheck:
                    return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);
                case HomeMediatorCodes.Authorize.NoUserProfile:
                    return RedirectToRoute(RecruitmentRouteNames.UserInfo);
                case HomeMediatorCodes.Authorize.EmailAddressNotVerified:
                    return RedirectToRoute(RecruitmentRouteNames.VerifyEmail);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}