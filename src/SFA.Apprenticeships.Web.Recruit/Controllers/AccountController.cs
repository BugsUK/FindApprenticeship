using SFA.Apprenticeships.Web.Recruit.Constants;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;

    public class AccountController : Controller
    {
        private const string DefaultScheme = "https";

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = "/authorize"
                };

                HttpContext.GetOwinContext().Authentication.Challenge(
                    properties, WsFederationAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            var callbackUrl = Url.Action("SignOutCallback", "Account", null, Request.Url?.Scheme ?? DefaultScheme);

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(
                properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToRoute(RecruitmentRouteNames.LandingPage);
            }

            return View();
        }
    }
}