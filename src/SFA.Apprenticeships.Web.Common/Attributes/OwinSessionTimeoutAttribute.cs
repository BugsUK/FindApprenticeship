namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Configuration;
    using System.Web.Mvc;

    public class OwinSessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SetDefaultSessionTimeoutProperties(filterContext);
            base.OnActionExecuted(filterContext);
        }

        private static void SetDefaultSessionTimeoutProperties(ControllerContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request?.Url?.PathAndQuery ?? "/";

            filterContext.Controller.ViewBag.SessionTimeout = int.Parse(ConfigurationManager.AppSettings["ida:SessionTimeout"]) * 60;
            filterContext.Controller.ViewBag.SessionTimeoutUrl = returnUrl;
            filterContext.Controller.ViewBag.EnableSessionTimeout = true;
        }
    }
}
