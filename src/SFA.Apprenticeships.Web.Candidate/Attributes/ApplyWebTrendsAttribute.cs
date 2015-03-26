namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;

    public class ApplyWebTrendsAttribute : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
            {
                return;
            }

            var webSettings = ConfigurationService.Get<WebConfiguration>(WebConfiguration.ConfigurationName);
            viewResult.ViewBag.EnableWebTrends = webSettings.EnableWebTrends;

            if (viewResult.ViewBag.EnableWebTrends == true)
            {
                viewResult.ViewBag.WebTrendsDscId = webSettings.WebTrendsDscId;
                viewResult.ViewBag.WebTrendsDomainName = webSettings.SiteDomainName;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}