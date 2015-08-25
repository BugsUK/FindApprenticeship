namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using Constants;
    using Controllers;

    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as IUserController;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (controller == null)
            {
                throw new ConfigurationErrorsException(string.Format("Controller {0} must inherit from IUserController", controllerName));
            }

            if (!controllerName.Equals("Login", StringComparison.InvariantCultureIgnoreCase))
            {
                controller.UserData.Pop(UserDataItemNames.SessionReturnUrl);
            }

            var httpContext = filterContext.Controller.ControllerContext.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                // They are logged in, set the timeout.
                SetSessionTimeout(filterContext);

                // And refresh authentication ticket if required
                controller.AuthenticationTicketService.RefreshTicket();
            }

            base.OnActionExecuted(filterContext);
        }

        #region Helpers

        private static void SetSessionTimeout(ControllerContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";

            filterContext.Controller.ViewBag.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds;
            filterContext.Controller.ViewBag.SessionTimeoutUrl = returnUrl;
            filterContext.Controller.ViewBag.EnableSessionTimeout = true;
        }

        #endregion
    }
}
