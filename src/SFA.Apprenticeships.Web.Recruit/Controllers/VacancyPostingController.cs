namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.VacancyPosting;
    using ViewModels.Vacancy;

    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : Controller
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var response = _vacancyPostingMediator.GetNewVacancyModel(User.Identity.Name);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateVacancy(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);
            var vacancyViewModel = response.ViewModel;

            return RedirectToRoute(RecruitmentRouteNames.SubmitVacancy, new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
        }

        [HttpGet]
        public ActionResult SubmitVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SubmitVacancy(VacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SubmitVacancy(viewModel);
            var vacancyViewModel = response.ViewModel;

            return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
        }

        [HttpGet]
        public ActionResult VacancySubmitted(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetSubmittedVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }
    }
}