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

            if (!isCookiePresent && !IsCookiePage(filterContext))
            {
                CookieDetectionProvider.SetCookie(filterContext.HttpContext);

                var request = filterContext.RequestContext.HttpContext.Request;
                var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";
                var helper = new UrlHelper(filterContext.RequestContext);
                var url = helper.Action("Cookies", "Home", new {ReturnUrl = returnUrl});

                filterContext.Result = new RedirectResult(url);

                return;
            }

            if (isCookiePresent)
            {
                if (IsCookiePage(filterContext))
                {
                    var url = filterContext.HttpContext.Request.QueryString.Get("returnUrl");

                    if (url.IsValidReturnUrlIncludingRoot())
                    {
                        filterContext.Result = new RedirectResult(url);
                        return;
                    }
                }
                else if (!IsAuthenticationPage(filterContext))
                {
                    filterContext.Controller.ViewBag.ShowEuCookieDirective =
                        EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private static bool IsCookiePage(ActionExecutingContext filterContext)
        {
            return filterContext.ActionDescriptor.ActionName.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase) &&
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home",
                       StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool IsAuthenticationPage(ActionExecutingContext filterContext)
        {
            return
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Account",
                    StringComparison.CurrentCultureIgnoreCase) ||
                filterContext.ActionDescriptor.ActionName.Equals("Authorize", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}