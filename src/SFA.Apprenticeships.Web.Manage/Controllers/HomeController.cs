namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using Application.Interfaces;
    using Common.Constants;
    using Common.Framework;
    using Raa.Common.Constants.Pages;
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : ManagementControllerBase
    {
        public HomeController(IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
        }

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

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public ActionResult WebTrendsOptOut()
        {

            var webTrendsOptOutCookie = new HttpCookie("WTLOPTOUT", "yes") { Expires = DateTime.UtcNow.AddYears(5) };
            HttpContext.Response.Cookies.Add(webTrendsOptOutCookie);
            SetUserMessage(PrivacyPageMessages.WebTrendsOptOutSuccessful);
            return View("Privacy");
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl.IsValidReturnUrl() ? returnUrl : "/";
            return View();
        }
    }
}