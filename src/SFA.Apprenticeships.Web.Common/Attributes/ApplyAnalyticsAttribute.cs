namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using Configuration;
    using Application.Interfaces;

    public class ApplyAnalyticsAttribute : ActionFilterAttribute
    {
        private readonly string _configurationName;

        public ApplyAnalyticsAttribute(string configurationName)
        {
            _configurationName = configurationName;
        }

        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
            {
                return;
            }

            var webSettings = ConfigurationService.Get<AnalyticsConfiguration>(_configurationName);

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