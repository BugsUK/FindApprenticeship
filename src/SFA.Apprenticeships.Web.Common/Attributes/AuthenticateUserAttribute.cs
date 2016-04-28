namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using System.Web.Security;
    using Application.Interfaces.Logging;
    using Constants;
    using Services;

    public class AuthenticateUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public ILogService LogService { get; set; }

        public IAuthenticationTicketService  AuthenticationTicketService { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var httpContext = filterContext.RequestContext.HttpContext;

            //Get the ticket as before
            var ticket = AuthenticationTicketService.GetTicket();

            //Get a local instance of the ticket and verify that they match
            var authenticationTicketService = new AuthenticationTicketService(httpContext, LogService);
            var localTicket = authenticationTicketService.GetTicket();

            if (ticket != null && localTicket != null && ticket.Name != localTicket.Name)
            {
                var message = string.Format(@"Session Issue: User id from AuthenticationTicketService property, {0} does not match User id from local AuthenticationTicketService instance, {1}. Throwing exception", ticket.Name, localTicket.Name);
                LogService.Warn(message);
            }

            if (ticket == null && localTicket != null)
            {
                var message = string.Format(@"Session Issue: No ticket found from AuthenticationTicketService property, but the user id from local AuthenticationTicketService instance, {0} suggests that the user should be logged in. Throwing exception", localTicket.Name);
                LogService.Warn(message);
            }

            if (ticket != null && localTicket == null)
            {
                var message = string.Format(@"Session Issue: User id retrieved from AuthenticationTicketService property, {0}, however no ticket was found from local AuthenticationTicketService instance suggesting that the user should not be logged in. Throwing exception", ticket.Name);
                LogService.Warn(message);
            }

            if (localTicket == null)
            {
                LogService.Debug("User is not logged in (no authentication ticket)");
                LogService.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);

                return;
            }

            if (IsCookieExpired(filterContext, localTicket))
            {
                LogService.Debug("User cookie is expired.");

                filterContext.Result = new RedirectToRouteResult(RouteNames.SignOut, new RouteValueDictionary());
            }

            var claims = AuthenticationTicketService.GetClaims(localTicket);

            httpContext.User = new GenericPrincipal(new FormsIdentity(localTicket), claims);

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
            var expirationTime = AuthenticationTicketService.GetExpirationTimeFrom(ticket);
            return (expirationTime < DateTime.UtcNow && !SigningOut(filterContext));
        }

        private static bool SigningOut(AuthenticationContext filterContext)
        {
            return filterContext.ActionDescriptor.ActionName == RouteNames.SignOut &&
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Login";
        }
    }
}
