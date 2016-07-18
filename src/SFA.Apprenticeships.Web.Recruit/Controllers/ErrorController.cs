﻿namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;

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