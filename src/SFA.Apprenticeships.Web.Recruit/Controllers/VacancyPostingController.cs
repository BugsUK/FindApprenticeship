namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Controllers;
    using Common.Mediators;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using Providers;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    [AuthorizationData]
    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : ControllerBase<RecruitmentUserContext>
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

        [HttpGet]
        public ActionResult SelectEmployer(EmployerFilterViewModel employerFilter)
        {
            return View(employerFilter);
        }

        [HttpGet]
        public ActionResult AddEmployer(EmployerSearchViewModel employerFilter)
        {
            return View(employerFilter);
        }

        [HttpGet]
        public ActionResult ConfirmEmployer(int employerid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmEmployer(int employerid, string employerText)
        {
            return RedirectToRoute(RecruitmentRouteNames.CreateVacancy);
        }

        [HttpGet]
        public ActionResult CreateVacancy()
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

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(vacancyViewModel);

                case VacancyPostingMediatorCodes.SubmitVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
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