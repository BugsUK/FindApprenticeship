namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Configuration;
    using Domain.Interfaces.Configuration;

    public class SiteRootRedirect : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectUrl = ConfigurationService.Get<WebConfiguration>(WebConfiguration.ConfigurationName).SiteRootRedirectUrl;

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}