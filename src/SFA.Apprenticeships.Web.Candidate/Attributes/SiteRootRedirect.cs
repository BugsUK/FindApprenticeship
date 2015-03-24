namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Web.Configuration;

    public class SiteRootRedirect : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectUrl = ConfigurationService.Get<WebConfiguration>(WebConfiguration.WebConfigurationName).SiteRootRedirectUrl;

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}