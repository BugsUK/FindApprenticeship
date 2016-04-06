namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Application.Interfaces.Logging;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Interfaces.Configuration;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Application;
    using ViewModels.Applications;
    using ViewModels.VacancySearch;

    [UserJourneyContext(UserJourney.Apprenticeship, Order = 2)]
    public class ApprenticeshipApplicationController : CandidateControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;
        private readonly ILogService _logService;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator,
            ILogService logService,
            IConfigurationService configurationService)
            : base(configurationService, logService)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
            _logService = logService;
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Resume(int id)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.Resume(originalCandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Resume.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Resume.IncorrectState:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Resume.Ok:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipApply, response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> Apply(string id)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);

                var response = _apprenticeshipApplicationMediator.Apply(originalCandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Apply.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Apply.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Apply.IncorrectState:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Apply.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        public async Task<ActionResult> Apply(int id, ApprenticeshipApplicationViewModel model)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.PreviewAndSubmit(originalCandidateId, id, model);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Ok:
                        ModelState.Clear();
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipPreview, response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        public async Task<ActionResult> Save(int id, ApprenticeshipApplicationViewModel model)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                MediatorResponse<ApprenticeshipApplicationViewModel> response;

                try
                {
                    response = _apprenticeshipApplicationMediator.Save(originalCandidateId, id, model);
                }
                catch (Exception)
                {
                    //TODO: Remove once the cause of saving exceptions is fixed
                    var requestDebugString = Request.ToDebugString();
                    _logService.Error("Error when saving apprenticeship application. Request: {0}", requestDebugString);
                    throw;
                }

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Save.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.Save.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Save.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Save.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Save.IncorrectState:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Save.Ok:
                        ModelState.Clear();
                        return View("Apply", response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<JsonResult> AutoSave(int id, ApprenticeshipApplicationViewModel model)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run(() =>
            {
                ValidateCandidateId(originalCandidateId);
                MediatorResponse<AutoSaveResultViewModel> response;

                try
                {
                    response = _apprenticeshipApplicationMediator.AutoSave(originalCandidateId, id, model);
                }
                catch (Exception)
                {
                    //TODO: Remove once the cause of the null candidate objects is fixed
                    var requestDebugString = Request.ToDebugString();
                    _logService.Error("Error when auto saving apprenticeship application. Request: {0}", requestDebugString);
                    throw;
                }

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.VacancyNotFound:
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.HasError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.ValidationError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.IncorrectState:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.Ok:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        public async Task<ActionResult> AddEmptyQualificationRows(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.AddEmptyQualificationRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.AddEmptyWorkExperienceRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationAction")]
        public async Task<ActionResult> AddEmptyTrainingCourseRows(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.AddEmptyTrainingCourseRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> Preview(int id)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.Preview(originalCandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Preview.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Preview.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Preview.IncorrectState:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Preview.Ok:
                        // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
                        ViewBag.VacancyId = id;
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Preview(int id, ApprenticeshipApplicationPreviewViewModel model)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.Submit(originalCandidateId, id, model);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Submit.AcceptSubmitValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Submit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Submit.IncorrectState:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Submit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipPreview, response.Parameters);
                    case ApprenticeshipApplicationMediatorCodes.Submit.Ok:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipWhatNext, response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> WhatHappensNext(string id, string vacancyReference, string vacancyTitle)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.WhatHappensNext(originalCandidateId, id, vacancyReference, vacancyTitle, ViewBag.SearchReturnUrl as string);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.Ok:
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
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run<ActionResult>(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.View(originalCandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.View.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.View.ApplicationNotFound:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.View.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<JsonResult> SaveVacancy(int id)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.SaveVacancy(originalCandidateId, id);

                return SavedVacancyResultFromViewModel(response);
            });
        }

        [HttpDelete]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<JsonResult> DeleteSavedVacancy(int id)
        {
            var originalCandidateId = UserContext.CandidateId;

            return await Task.Run(() =>
            {
                ValidateCandidateId(originalCandidateId);
                var response = _apprenticeshipApplicationMediator.DeleteSavedVacancy(originalCandidateId, id);

                return SavedVacancyResultFromViewModel(response);
            });
        }

        #region Helpers

        private static JsonResult SavedVacancyResultFromViewModel(MediatorResponse<SavedVacancyViewModel> response)
        {
            return new JsonResult
            {
                Data = new
                {
                    applicationStatus = response.ViewModel.ApplicationStatus.HasValue
                        ? response.ViewModel.ApplicationStatus.ToString()
                        : "Unsaved"
                }
            };
        }

        #endregion
    }
}