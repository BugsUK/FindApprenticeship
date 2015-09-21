namespace SFA.Apprenticeships.Web.Recruit.Attributes
{
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using Constants;

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                var user = filterContext.RequestContext.HttpContext.User;

                if (user.Identity.IsAuthenticated)
                {
                    var requireVerifiedEmail = Roles.Contains(Constants.Roles.VerifiedEmail);
                    if (requireVerifiedEmail && !user.IsInRole(Constants.Roles.VerifiedEmail))
                    {
                        filterContext.Result = new RedirectToRouteResult(RecruitmentRouteNames.VerifyEmail, null);
                    }
                }
                else
                {
                    var routeValues = new RouteValueDictionary {{"ReturnUrl", GetReturnUrl(filterContext)}};

                    filterContext.Result = new RedirectToRouteResult(RecruitmentRouteNames.SignIn, routeValues);
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

        }

        private static string GetReturnUrl(AuthorizationContext filterContext)
        {
            var url = filterContext.RequestContext.HttpContext.Request.Url;

            if (url == null)
            {
                return string.Empty;
            }

            return url.PathAndQuery;
        }
    }
}