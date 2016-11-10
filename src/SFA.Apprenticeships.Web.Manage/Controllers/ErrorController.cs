
namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Common.Attributes;

    [ApplyAnalytics("ManageWebConfiguration")]
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        public ActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
    }
}