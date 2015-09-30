namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;

    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateVacancy()
        {
            return RedirectToRoute(RecruitmentRouteNames.SubmitVacancy, new {id = Guid.NewGuid()});
        }

        [HttpGet]
        public ActionResult SubmitVacancy(Guid id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitVacancy()
        {
            return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new { id = Guid.NewGuid() });
        }

        [HttpGet]
        public ActionResult VacancySubmitted(Guid id)
        {
            return View();
        }
    }
}