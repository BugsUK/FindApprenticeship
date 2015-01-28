﻿namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Configuration;
    using StructureMap.Attributes;

    public class SavedSearchesToggle : ActionFilterAttribute
    {
        [SetterProperty]
        public IFeatureToggle FeatureToggle { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!FeatureToggle.IsActive(Feature.SavedSearches))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}