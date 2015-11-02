using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Manage.Controllers
{
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