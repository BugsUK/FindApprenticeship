namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using System.Web.Security;

    public class DefaultSessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SetDefaultSessionTimeoutProperties(filterContext);
            base.OnActionExecuted(filterContext);
        }

        #region Helpers

        private static void SetDefaultSessionTimeoutProperties(ControllerContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";

            filterContext.Controller.ViewBag.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds;
            filterContext.Controller.ViewBag.SessionTimeoutUrl = returnUrl;

            // NOTE: Session timeout is disabled by default. ViewBag.EnableSessionTimeout must be set to true
            // elsewhere to enable session timeout.
        }

        #endregion
    }
}
