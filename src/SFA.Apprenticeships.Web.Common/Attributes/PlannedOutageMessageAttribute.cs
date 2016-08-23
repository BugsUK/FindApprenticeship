namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using Configuration;
    using Providers;

    using SFA.Apprenticeships.Application.Interfaces;

    public class PlannedOutageMessageAttribute : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public IDismissPlannedOutageMessageCookieProvider DismissPlannedOutageMessageCookieProvider { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var plannedOutageMessage = ConfigurationService.Get<CommonWebConfiguration>().PlannedOutageMessage;
            if (!string.IsNullOrEmpty(plannedOutageMessage) && !DismissPlannedOutageMessageCookieProvider.IsCookiePresent(filterContext.HttpContext))
            {
                filterContext.Controller.ViewBag.PlannedOutageMessage = plannedOutageMessage;
            }
        }
    }
}