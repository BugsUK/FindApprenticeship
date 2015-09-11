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

        public HomeController(IAuthenticationTicketService authenticationTicketService)
        {
            _authenticationTicketService = authenticationTicketService;
        }

        public ActionResult Index()
        {
            return View();
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