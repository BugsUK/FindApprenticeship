namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.ViewModels;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Mediators.Candidate;
    using Raa.Common.ViewModels.Candidate;

    [OwinSessionTimeout]
    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class CandidateController : RecruitmentControllerBase
    {
        private readonly ICandidateMediator _candidateMediator;

        public CandidateController(IConfigurationService configurationService, ILogService loggingService, ICandidateMediator candidateMediator) : base(configurationService, loggingService)
        {
            _candidateMediator = candidateMediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel()
            };
            return View("Search", viewModel);
        }

        [HttpGet]
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
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchCandidatesAction")]
        public ActionResult SearchCandidates(CandidateSearchResultsViewModel viewModel)
        {
            return RedirectToRoute(RecruitmentRouteNames.SearchCandidates, viewModel.SearchViewModel);
        }

        [HttpGet]
        public ActionResult Candidate(CandidateApplicationsSearchViewModel searchViewModel)
        {
            var response = _candidateMediator.GetCandidateApplications(searchViewModel, User.GetUkprn());

            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult SortCandidate(CandidateApplicationSummariesViewModel viewModel)
        {
            return RedirectToRoute(RecruitmentRouteNames.ViewCandidate, viewModel.CandidateApplicationsSearch.RouteValues);
        }
    }
}