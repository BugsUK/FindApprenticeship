﻿namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using Providers;

    public class PlannedOutageMessageAttribute : ActionFilterAttribute
    {
        private const string PlannedOutageMessageKey = "PlannedOutageMessage";

        public IConfigurationService ConfigurationService { get; set; }

        public IDismissPlannedOutageMessageCookieProvider DismissPlannedOutageMessageCookieProvider { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var plannedOutageMessage = ConfigurationService.GetCloudAppSetting<string>(PlannedOutageMessageKey);
            if (!string.IsNullOrEmpty(plannedOutageMessage) && !DismissPlannedOutageMessageCookieProvider.IsCookiePresent(filterContext.HttpContext))
            {
                filterContext.Controller.ViewBag.PlannedOutageMessage = plannedOutageMessage;
            }
        }
    }
}