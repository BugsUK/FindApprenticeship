﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Constants;

    [ApplyWebTrends]
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
