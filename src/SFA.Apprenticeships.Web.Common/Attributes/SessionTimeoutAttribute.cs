namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Constants;
    using Controllers;

    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public SessionTimeoutAttribute()
        {
            Order = 1;
        }

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
                // They are logged in, enable the timeout.
                EnableSessionTimeout(filterContext);

                // And refresh authentication ticket if required
                controller.AuthenticationTicketService.RefreshTicket();
            }

            base.OnActionExecuted(filterContext);
        }

        #region Helpers

        private static void EnableSessionTimeout(ControllerContext filterContext)
        {
            filterContext.Controller.ViewBag.EnableSessionTimeout = true;
        }

        #endregion
    }
}
