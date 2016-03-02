namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Mediators.Candidate;
    using ViewModels;

    public class CandidateController : ManagementControllerBase
    {
        private readonly ICandidateMediator _candidateMediator;

        public CandidateController(ICandidateMediator candidateMediator)
        {
            _candidateMediator = candidateMediator;
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Index()
        {
            var response = _candidateMediator.Search();

            return View("Search", response.ViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Search(CandidateSearchViewModel viewModel)
        {
            var response = _candidateMediator.Search(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case CandidateMediatorCodes.Search.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case CandidateMediatorCodes.Search.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchCandidatesAction")]
        public ActionResult SearchCandidates(CandidateSearchResultsViewModel viewModel)
        {
            return RedirectToRoute(ManagementRouteNames.SearchCandidates, viewModel.SearchViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Candidate(Guid id)
        {
            var response = _candidateMediator.GetCandidateApplications(id);

            return View(response.ViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Apprenticeship(Guid applicationId)
        {
            var response = _candidateMediator.GetCandidateApprenticeshipApplication(applicationId);

            return View(response.ViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Traineeship(Guid applicationId)
        {
            var response = _candidateMediator.GetCandidateTraineeshipApplication(applicationId);

            return View(response.ViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult ApprenticeshipVacancy(int vacancyId, Guid applicationId)
        {
            var response = _candidateMediator.GetCandidateApprenticeshipVacancyViewModel(vacancyId, applicationId);
            var candidateVacancyViewModel = response.ViewModel;

            SetLinks(candidateVacancyViewModel.Vacancy);

            return View("Vacancy", candidateVacancyViewModel);
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult TraineeshipVacancy(int vacancyId, Guid applicationId)
        {
            var response = _candidateMediator.GetCandidateTraineeshipVacancyViewModel(vacancyId, applicationId);
            var candidateVacancyViewModel = response.ViewModel;

            SetLinks(candidateVacancyViewModel.Vacancy);

            return View("Vacancy", candidateVacancyViewModel);
        }
    }
}