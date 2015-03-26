namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;

    public class SavedSearchesToggle : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = ConfigurationService.Get<WebConfiguration>();
            if (!config.Features.SavedSearchesEnabled)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}