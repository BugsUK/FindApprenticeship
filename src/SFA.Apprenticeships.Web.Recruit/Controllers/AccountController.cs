namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Constants;
    using System.Web;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants.Messages;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using SFA.Infrastructure.Interfaces;
    using Attributes;

    public class AccountController : RecruitmentControllerBase
    {
        private const string DefaultScheme = "https";

        private readonly ICookieAuthorizationDataProvider _authorizationDataProvider;

        public AccountController(ICookieAuthorizationDataProvider authorizationDataProvider, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _authorizationDataProvider = authorizationDataProvider;
        }

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

        [AuthorizeUser]
        public void SignOut(string returnRoute)
        {
            var callbackUrl = Url.RouteUrl(returnRoute ?? RecruitmentRouteNames.SignOutCallback, new {timeout = false}, Request.Url?.Scheme ?? DefaultScheme);

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(
                properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public void SessionTimeout()
        {
            var callbackUrl = Url.RouteUrl(RecruitmentRouteNames.SignOutCallback, new {timeout = true}, Request.Url?.Scheme ?? DefaultScheme);

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(
                properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback(bool timeout)
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToRoute(RecruitmentRouteNames.LandingPage);
            }

            _authorizationDataProvider.Clear(HttpContext);

            if (timeout)
            {
                SetUserMessage(AuthorizeMessages.SignedOutTimeout, UserMessageLevel.Info);
            }
            else
            {
                SetUserMessage(AuthorizeMessages.SignedOut, UserMessageLevel.Info);
            }

            return RedirectToRoute(RecruitmentRouteNames.LandingPage);
        }
    }
}