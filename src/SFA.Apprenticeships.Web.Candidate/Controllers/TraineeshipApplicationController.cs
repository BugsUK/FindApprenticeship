﻿using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using ActionResults;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.Application;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Applications;

    [UserJourneyContext(UserJourney.Traineeship, Order = 2)]
    public class TraineeshipApplicationController : CandidateControllerBase
    {
        private readonly ITraineeshipApplicationMediator _traineeshipApplicationMediator;

        public TraineeshipApplicationController(ITraineeshipApplicationMediator traineeshipApplicationMediator,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _traineeshipApplicationMediator = traineeshipApplicationMediator;
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ClearSearchReturnUrl(false)]
        [SessionTimeout]
        public async Task<ActionResult> Apply(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.Apply(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.Apply.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case TraineeshipApplicationMediatorCodes.Apply.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> Apply(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.Submit(UserContext.CandidateId, id, model);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.Submit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case TraineeshipApplicationMediatorCodes.Submit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case TraineeshipApplicationMediatorCodes.Submit.Ok:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipWhatNext, response.Parameters);
                    case TraineeshipApplicationMediatorCodes.Submit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> AddEmptyQualificationRows(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.AddEmptyQualificationRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.AddEmptyWorkExperienceRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> AddEmptyTrainingCourseRows(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.AddEmptyTrainingCourseRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> WhatHappensNext(string id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.WhatHappensNext(UserContext.CandidateId, id, vacancyReference, vacancyTitle);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound:
                        return new TraineeshipNotFoundResult();
                    case TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> View(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.View(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.View.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);

                    case TraineeshipApplicationMediatorCodes.View.ApplicationNotFound:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);

                    case TraineeshipApplicationMediatorCodes.View.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}