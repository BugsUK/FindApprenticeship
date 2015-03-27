﻿namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;

    public class SmsEnabledToggle : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = ConfigurationService.Get<WebConfiguration>(WebConfiguration.ConfigurationName);
            if (!config.Features.SmsEnabled)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}