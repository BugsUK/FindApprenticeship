﻿namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Configuration;
    using Application.Interfaces;

    public class ApplyAnalyticsAttribute : ActionFilterAttribute
    {
        private readonly Type _analyticsConfigurationType;

        public ApplyAnalyticsAttribute(Type analyticsConfigurationType)
        {
            _analyticsConfigurationType = analyticsConfigurationType;
        }

        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
            {
                return;
            }

            var settings = ConfigurationService.Get(_analyticsConfigurationType);
            var webSettings = (AnalyticsConfiguration)settings;

            viewResult.ViewBag.EnableWebTrends = webSettings.EnableWebTrends;
            if (webSettings.EnableWebTrends)
            {
                viewResult.ViewBag.WebTrendsDscId = webSettings.WebTrendsDscId;
                viewResult.ViewBag.WebTrendsDomainName = webSettings.WebTrendsDomainName;
            }

            viewResult.ViewBag.EnableGoogleTagManager = webSettings.EnableGoogleTagManager;
            if (webSettings.EnableGoogleTagManager)
            {
                viewResult.ViewBag.GoogleContainerId = webSettings.GoogleContainerId;
            }

            viewResult.ViewBag.EnableAppInsights = webSettings.EnableAppInsights;
            if (webSettings.EnableAppInsights)
            {
                viewResult.ViewBag.AppInsightsInstrumentationKey = webSettings.AppInsightsInstrumentationKey;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}