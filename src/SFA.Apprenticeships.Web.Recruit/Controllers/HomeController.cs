﻿namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;

    public class HomeController : RecruitmentControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}