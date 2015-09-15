using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Services;
    using Constants;
    using Providers;

    public class HomeController : ControllerBase<RecuitmentUserContext>
    {
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly IHomeMediator _homeMediator;

        public HomeController(IAuthenticationTicketService authenticationTicketService, IHomeMediator homeMediator)
        {
            _authenticationTicketService = authenticationTicketService;
            _homeMediator = homeMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            var claimsPrincipal = (ClaimsPrincipal) User;
            var response = _homeMediator.Authorize(claimsPrincipal);
            switch (response.Code)
            {
                case HomeMediatorCodes.Authorize.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.LandingPage);
            }

            return RedirectToRoute(RecruitmentRouteNames.LandingPage);
        }

        public ActionResult LoginDummy(string loginType)
        {
            switch (loginType)
            {
                case "validProvider":
                    //We need to extend SetAuthenticationCookie and the cookie to add claims.
                    _authenticationTicketService.SetAuthenticationCookie("userid123", "ProviderId=123", "RAA");
                    break;
                case "invalidProvider":
                    _authenticationTicketService.SetAuthenticationCookie("userid234");
                    break;
                default:
                    return View();
            }

            return RedirectToRoute(RecruitmentRouteNames.LandingPage);
        }
    }
}