namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using System.Web.Security;

    public class GlobalSessionTimeoutAttribute : ActionFilterAttribute
    {
        public GlobalSessionTimeoutAttribute()
        {
            Order = 0;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SetDefaultSessionTimeout(filterContext);
            base.OnActionExecuted(filterContext);
        }

        #region Helpers

        private static void SetDefaultSessionTimeout(ControllerContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";

            filterContext.Controller.ViewBag.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds;
            filterContext.Controller.ViewBag.SessionTimeoutUrl = returnUrl;

            // NOTE: EnableSessionTimeout may be set to true elsewhere.
            filterContext.Controller.ViewBag.EnableSessionTimeout = false;
        }

        #endregion
    }
}
