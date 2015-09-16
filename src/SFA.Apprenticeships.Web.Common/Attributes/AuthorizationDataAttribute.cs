namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using Providers;

    public class AuthorizationDataAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public ICookieAuthorizationDataProvider CookieAuthorizationDataProvider { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated) return;

            var claimsPrincipal = filterContext.Principal as ClaimsPrincipal;
            if (claimsPrincipal == null) return;

            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return;

            var httpContext = filterContext.RequestContext.HttpContext;

            var username = claimsPrincipal.Identity.Name;

            //Copy claims from our domain into the identity
            var claims = CookieAuthorizationDataProvider.GetClaims(httpContext, username);
            claimsIdentity.AddClaims(claims);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

        }
    }
}