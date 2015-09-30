namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.VacancyPosting;

    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : Controller
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

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
            var response = _vacancyPostingMediator.GetSubmittedVacancyViewModel(id);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }
    }
}