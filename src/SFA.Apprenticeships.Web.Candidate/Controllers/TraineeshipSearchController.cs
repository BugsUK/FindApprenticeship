﻿using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using ActionResults;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Domain.Entities.Vacancies;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators.Search;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.VacancySearch;

    [UserJourneyContext(UserJourney.Traineeship, Order = 2)]
    public class TraineeshipSearchController : CandidateControllerBase
    {
        private readonly ITraineeshipSearchMediator _traineeshipSearchMediator;

        public TraineeshipSearchController(ITraineeshipSearchMediator traineeshipSearchMediator,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _traineeshipSearchMediator = traineeshipSearchMediator;
        }

        [HttpGet]
        public async Task<ActionResult> Overview()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _traineeshipSearchMediator.Index(candidateId);

                return View(response.ViewModel);
            });
        }

        [HttpPost]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> Index(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = Request?.Url?.PathAndQuery;
                var response = _traineeshipSearchMediator.SearchValidation(model);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.SearchValidation.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Index", response.ViewModel);
                    case TraineeshipSearchMediatorCodes.SearchValidation.Ok:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipResults, model.RouteValues);
                }
                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [SessionTimeout]
        public async Task<ActionResult> Results(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = Request?.Url?.PathAndQuery;

                var response = _traineeshipSearchMediator.Results(model);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case TraineeshipSearchMediatorCodes.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case TraineeshipSearchMediatorCodes.Results.ExactMatchFound:
                        ViewBag.SearchReturnUrl = null;
                        return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, response.Parameters);
                    case TraineeshipSearchMediatorCodes.Results.Ok:
                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> DetailsWithDistance(int id, string distance)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Push(CandidateDataItemNames.VacancyDistance, distance);
                UserData.PushLastViewedVacancyId(id, VacancyType.Traineeship);

                return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [RobotsIndexPage(true)]
        [SessionTimeout]
        public async Task<ActionResult> Details(string id)
        {
            return await Task.Run(() =>
            {
                var candidateId = GetCandidateId();

                var response = _traineeshipSearchMediator.Details(id, candidateId);

                return GetDetailsResult(response);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [RobotsIndexPage(true)]
        [SessionTimeout]
        public async Task<ActionResult> DetailsByReferenceNumber(string vacancyReferenceNumber)
        {
            return await Task.Run(() =>
            {
                var candidateId = GetCandidateId();

                var response = _traineeshipSearchMediator.DetailsByReferenceNumber(vacancyReferenceNumber, candidateId);

                return GetDetailsResult(response);
            });
        }

        private ActionResult GetDetailsResult(MediatorResponse<TraineeshipVacancyDetailViewModel> response)
        {
            switch (response.Code)
            {
                case TraineeshipSearchMediatorCodes.Details.VacancyNotFound:
                    return new TraineeshipNotFoundResult();

                case TraineeshipSearchMediatorCodes.Details.VacancyHasError:
                    ModelState.Clear();
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View("Details", response.ViewModel);

                case TraineeshipSearchMediatorCodes.Details.Ok:
                    return View("Details", response.ViewModel);
            }

            throw new InvalidMediatorCodeException(response.Code);
        }
    }
}
