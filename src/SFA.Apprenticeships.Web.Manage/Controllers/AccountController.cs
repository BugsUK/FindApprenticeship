using SFA.Apprenticeships.Web.Manage.Constants;

namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants.Messages;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using ControllerBase = Common.Controllers.ControllerBase;

    public class AccountController : ControllerBase
    {
        private const string DefaultScheme = "https";

        private readonly ICookieAuthorizationDataProvider _authorizationDataProvider;

        public AccountController(ICookieAuthorizationDataProvider authorizationDataProvider)
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
                RedirectUri = Url.RouteUrl(ManagementRouteNames.Authorize)
            };

            HttpContext.GetOwinContext().Authentication.Challenge(
                properties, WsFederationAuthenticationDefaults.AuthenticationType);
        }

        public void SignOut(string returnRoute)
        {
            var callbackUrl = Url.RouteUrl(returnRoute ?? ManagementRouteNames.SignOutCallback, new {timeout = false}, String.Copy(Request.Url?.Scheme ?? DefaultScheme));

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(
                properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public void SessionTimeout()
        {
            var callbackUrl = Url.RouteUrl(ManagementRouteNames.SignOutCallback, new {timeout = true}, Request.Url?.Scheme ?? DefaultScheme);

            var properties = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            HttpContext.GetOwinContext().Authentication.SignOut(
                properties, WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback(bool timeout = false)
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToRoute(ManagementRouteNames.LandingPage);
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

            return RedirectToRoute(ManagementRouteNames.LandingPage);
        }
    }
}