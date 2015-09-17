using SFA.Apprenticeships.Web.Recruit.Constants;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Framework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using ControllerBase = Common.Controllers.ControllerBase;

    public class AccountController : ControllerBase
    {
        private const string DefaultScheme = "https";

        public void SignIn(string returnUrl)
        {
            if (returnUrl.IsValidReturnUrl())
            {
                UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.RouteUrl(RecruitmentRouteNames.Authorize)
            };

            HttpContext.GetOwinContext().Authentication.Challenge(
                properties, WsFederationAuthenticationDefaults.AuthenticationType);
        }

        public void SignOut(string returnRoute)
        {
            var callbackUrl = Url.RouteUrl(returnRoute ?? RecruitmentRouteNames.SignOutCallback, null, Request.Url?.Scheme ?? DefaultScheme);

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