namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using SFA.Infrastructure.Interfaces;
    using Configuration;

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

            var webSettings = ConfigurationService.Get<CommonWebConfiguration>();
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