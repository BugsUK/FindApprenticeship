namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using System.Web.Security;
    using SFA.Infrastructure.Interfaces;
    using Constants;
    using Services;

    public class AuthenticateUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public ILogService LogService { get; set; }

        public IAuthenticationTicketService  AuthenticationTicketService { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var httpContext = filterContext.RequestContext.HttpContext;

            var authService = new AuthenticationTicketService(httpContext, LogService);
            var ticket = authService.GetTicket();

            if (ticket == null)
            {
                LogService.Debug("User is not logged in (no authentication ticket)");
                LogService.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);

                return;
            }

            if (IsCookieExpired(filterContext, ticket))
            {
                LogService.Debug("User cookie is expired.");

                filterContext.Result = new RedirectToRouteResult(RouteNames.SignOut, new RouteValueDictionary());
            }

            var claims = authService.GetClaims(ticket);

            httpContext.User = new GenericPrincipal(new FormsIdentity(ticket), claims);

            LogService.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);
            LogService.Debug("Claims {0}", string.Join(",", claims));

            LogService.Debug("Activated: {0}", httpContext.User.IsInRole(UserRoleNames.Activated));
            LogService.Debug("Unactivated: {0}", httpContext.User.IsInRole(UserRoleNames.Unactivated));
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }

        private bool IsCookieExpired(AuthenticationContext filterContext, FormsAuthenticationTicket ticket)
        {
            var expirationTime = new AuthenticationTicketService(filterContext.RequestContext.HttpContext, LogService).GetExpirationTimeFrom(ticket);
            return (expirationTime < DateTime.UtcNow && !SigningOut(filterContext));
        }

        private static bool SigningOut(AuthenticationContext filterContext)
        {
            return filterContext.ActionDescriptor.ActionName == RouteNames.SignOut &&
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Login";
        }
    }
}
