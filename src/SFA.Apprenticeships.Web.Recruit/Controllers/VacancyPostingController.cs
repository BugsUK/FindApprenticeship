namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using ViewModels.Vacancy;

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
            var viewModel = new SubmittedVacancyViewModel
            {
                ApproverEmail = "john.smith@salon-secrets.co.uk",
                PublishDate = new DateTime(2015, 10, 12)
            };

            return View(viewModel);
        }
    }
}