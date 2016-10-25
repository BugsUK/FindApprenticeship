namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Raa;
    using Domain.Raa.Interfaces.Repositories.Models;
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
        public ActionResult BulkDeclineCandidates(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            var response = _applicationMediator.GetBulkDeclineCandidatesViewModel(bulkDeclineCandidatesViewModel);
            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkSearchApplicationsAction")]
        public ActionResult BulkFilterApplicationsAll(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.FilterType = VacancyApplicationsFilterTypes.All;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkSearchApplicationsAction")]
        public ActionResult BulkFilterApplicationsNew(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.FilterType = VacancyApplicationsFilterTypes.New;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkSearchApplicationsAction")]
        public ActionResult BulkFilterApplicationsInProgress(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.FilterType = VacancyApplicationsFilterTypes.InProgress;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkOrderApplicationsAction")]
        public ActionResult BulkOrderApplicationsLastName(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.OrderByField = VacancyApplicationsSearchViewModel.OrderByFieldLastName;
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order = bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order == Order.Ascending ? Order.Descending : Order.Ascending; ;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkOrderApplicationsAction")]
        public ActionResult BulkOrderApplicationsFirstName(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.OrderByField = VacancyApplicationsSearchViewModel.OrderByFieldLastName;
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order = bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order == Order.Ascending ? Order.Descending : Order.Ascending; ;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkOrderApplicationsAction")]
        public ActionResult BulkOrderApplicationsManagerNotes(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.OrderByField = VacancyApplicationsSearchViewModel.OrderByFieldLastName;
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order = bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order == Order.Ascending ? Order.Descending : Order.Ascending; ;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkOrderApplicationsAction")]
        public ActionResult BulkOrderApplicationsSubmitted(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.OrderByField = VacancyApplicationsSearchViewModel.OrderByFieldLastName;
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order = bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order == Order.Ascending ? Order.Descending : Order.Ascending; ;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BulkOrderApplicationsAction")]
        public ActionResult BulkOrderApplicationsStatus(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.OrderByField = VacancyApplicationsSearchViewModel.OrderByFieldLastName;
            bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order = bulkDeclineCandidatesViewModel.VacancyApplicationsSearch.Order == Order.Ascending ? Order.Descending : Order.Ascending; ;
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "ConfirmBulkDeclineCandidatesAction")]
        public ActionResult ConfirmBulkDeclineCandidates(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            var response = _applicationMediator.ConfirmBulkDeclineCandidates(bulkDeclineCandidatesViewModel);
            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmBulkDeclineCandidates.Ok:
                    return View("ConfirmBulkUnsuccessfulDecision", response.ViewModel);
                case ApprenticeshipApplicationMediatorCodes.ConfirmBulkDeclineCandidates.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("BulkDeclineCandidates", response.ViewModel);
                default:
                    throw new InvalidOperationException();
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "EditBulkDeclineCandidatesAction")]
        public ActionResult EditBulkDeclineCandidates(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            return BulkDeclineCandidates(bulkDeclineCandidatesViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SendBulkUnsuccessfulDecisionAction")]
        public ActionResult SendBulkUnsuccessfulDecision(BulkDeclineCandidatesViewModel bulkDeclineCandidatesViewModel)
        {
            bulkDeclineCandidatesViewModel.UnSuccessfulReasonRequired = true;
            var response = _applicationMediator.SendBulkUnsuccessfulDecision(bulkDeclineCandidatesViewModel);
            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.SendBulkUnsuccessfulDecision.Ok:
                    {
                        return View("SentDecisionConfirmation", response.ViewModel);
                    }
                case ApprenticeshipApplicationMediatorCodes.SendBulkUnsuccessfulDecision.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("ConfirmBulkUnsuccessfulDecision", response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}