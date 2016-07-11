namespace SFA.Apprenticeships.Web.Raa.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Web.Common.Constants;
    using Web.Common.Framework;
    using Web.Common.Providers;

    public class CookiesEnabledAttribute : ActionFilterAttribute
    {
        public ICookieDetectionProvider CookieDetectionProvider { get; set; }

        public IRobotCrawlerProvider RobotCrawlerProvider { get; set; }

        public IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        public IUserDataProvider UserDataProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isCookiePresent = CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext);

            if (!isCookiePresent && RobotCrawlerProvider.IsRobot(filterContext.HttpContext))
            {
                return;
            }

            if (!isCookiePresent)
            {
                CookieDetectionProvider.SetCookie(filterContext.HttpContext);
                var request = filterContext.RequestContext.HttpContext.Request;
                var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";
                var helper = new UrlHelper(filterContext.RequestContext);
                var url = helper.Action("Cookies", "Home", new {ReturnUrl = returnUrl});
                filterContext.Result = !string.IsNullOrWhiteSpace(url) ? new RedirectResult(url) : new RedirectResult(returnUrl);
                return;
            }

            if (filterContext.ActionDescriptor.ActionName.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase) && 
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home", StringComparison.CurrentCultureIgnoreCase))
            {
                var url = filterContext.HttpContext.Request.QueryString.Get("returnUrl");

                if (url.IsValidReturnUrl())
                {
                    filterContext.Result = new RedirectResult(url);
                    return;
                }
            }
            else
            {
                filterContext.Controller.ViewBag.ShowEuCookieDirective =
                    EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var savedAndDraftCount = UserDataProvider.Get(UserDataItemNames.SavedAndDraftCount);
            if (!string.IsNullOrWhiteSpace(savedAndDraftCount))
            {
                filterContext.Controller.ViewBag.SavedAndDraftCount = savedAndDraftCount;
            }

            var applicationStatusChangeCount = UserDataProvider.Get(UserDataItemNames.ApplicationStatusChangeCount);
            if (!string.IsNullOrWhiteSpace(savedAndDraftCount))
            {
                filterContext.Controller.ViewBag.ApplicationStatusChangeCount = applicationStatusChangeCount;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}