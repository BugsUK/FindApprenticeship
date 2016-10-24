namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Mediators.Application;
    using Raa.Common.ViewModels.Application;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class ApplicationController : RecruitmentControllerBase
    {
        private readonly IApplicationMediator _applicationMediator;

        public ApplicationController(IApplicationMediator applicationMediator, IConfigurationService configurationService,
            ILogService logService) : base(configurationService, logService)
        {
            _applicationMediator = applicationMediator;
        }

        [HttpGet]
        public ActionResult VacancyApplications(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var response = _applicationMediator.GetVacancyApplicationsViewModel(vacancyApplicationsSearch);

            switch (response.Code)
            {
                case ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult VacancyApplications(VacancyApplicationsViewModel vacancyApplications)
        {
            return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, vacancyApplications.VacancyApplicationsSearch.RouteValues);
        }

        [HttpGet]
        public ActionResult ShareApplications(int vacancyReferenceNumber)
        {
            var response = _applicationMediator.ShareApplications(vacancyReferenceNumber);

            switch (response.Code)
            {
                case ApplicationMediatorCodes.GetShareApplicationsViewModel.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult ShareApplications(ShareApplicationsViewModel viewModel)
        {

            var response = _applicationMediator.ShareApplications(viewModel, Url);

            switch (response.Code)
            {
                case ApplicationMediatorCodes.ShareApplications.Ok:
                    SetUserMessage($"You have shared {response.ViewModel.SelectedApplicationIds.Count()} applications with {viewModel.RecipientEmailAddress}");
                    return View(response.ViewModel);
                case ApplicationMediatorCodes.ShareApplications.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult BulkDeclineCandidates(int vacancyReferenceNumber)
        {
            var response = _applicationMediator.GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult ConfirmBulkDeclineCandidates(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel = new BulkApplicationsRejectViewModel
            {
                ApplicationIds = bulkDeclineCandidatesViewModel.SelectedApplicationIds,
                VacancyReferenceNumber = bulkDeclineCandidatesViewModel.VacancyReferenceNumber
            };
            var response = _applicationMediator.BulkResponseApplications(bulkApplicationsRejectViewModel);
            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok:
                    var bulkResponseViewModel = _applicationMediator.GetApplicationViewModel(bulkApplicationsRejectViewModel);
                    return View("ConfirmBulkUnsuccessfulDecision", bulkResponseViewModel);
                case ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("BulkDeclineCandidates", response.ViewModel);
                default:
                    throw new InvalidOperationException();
            }
        }

        [HttpGet]
        public ActionResult BulkDeclineCandidatesSearch(VacancyApplicationsSearchViewModel vacancyApplicationsSearchViewModel)
        {
            var response = _applicationMediator.GetBulkDeclineCandidatesViewModel(vacancyApplicationsSearchViewModel);
            return View("BulkDeclineCandidates", response.ViewModel);
        }
    }
}