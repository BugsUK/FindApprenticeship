namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Constants;
    using Configuration;

    [ApplyAnalytics(typeof(CandidateWebConfiguration))]
    [OutputCache(CacheProfile = CacheProfiles.Long)]
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
