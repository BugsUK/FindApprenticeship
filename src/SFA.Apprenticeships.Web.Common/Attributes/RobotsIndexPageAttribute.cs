namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;

    public class RobotsIndexPageAttribute : ActionFilterAttribute
    {
        private readonly bool _shouldIndexPage;

        public RobotsIndexPageAttribute(bool shouldIndexPage = false)
        {
            _shouldIndexPage = shouldIndexPage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ShouldRobotsIndexPage = _shouldIndexPage;
        }
    }
}