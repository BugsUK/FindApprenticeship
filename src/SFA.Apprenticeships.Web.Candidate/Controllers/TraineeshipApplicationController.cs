﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Application;
    using ViewModels.Applications;

    [UserJourneyContext(UserJourney.Traineeship, Order = 2)]
    public class TraineeshipApplicationController : CandidateControllerBase
    {
        private readonly ITraineeshipApplicationMediator _traineeshipApplicationMediator;

        public TraineeshipApplicationController(ITraineeshipApplicationMediator traineeshipApplicationMediator,
            IConfigurationService configurationService) : base(configurationService)
        {
            _traineeshipApplicationMediator = traineeshipApplicationMediator;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ClearSearchReturnUrl(false)]
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
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Submit")]
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
                        return RedirectToAction("WhatHappensNext", response.Parameters);
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
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyQualificationRows")]
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
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyWorkExperienceRows")]
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

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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

        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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