namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using SFA.Infrastructure.Interfaces;

    //TODO: Split this class by code region
    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    [OwinSessionTimeout]
    public class VacancyPostingController : RecruitmentControllerBase
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

        #region Employer Selection

        [HttpGet]
        public ActionResult SelectEmployer(int providerSiteId, Guid? vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(providerSiteId, vacancyGuid, comeFromPreview);

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.GetProviderEmployers.NoResults:
                        return RedirectToRoute(RecruitmentRouteNames.SelectNewEmployer,
                            new { providerSiteId, vacancyGuid });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        
        [HttpGet]
        public ActionResult SearchExistingEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("SelectEmployer", response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View("SelectEmployer", response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.NoResults:
                    return View("SelectEmployer", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult AddEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetEmployers(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployers.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmEmployer(int providerSiteId, string edsUrn, Guid vacancyGuid, bool? comeFromPreview, bool? useEmployerLocation)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteId, edsUrn, vacancyGuid, comeFromPreview, useEmployerLocation);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployer.Ok:
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.GetEmployer.InvalidEmployerAddress:
                    SetUserMessage(response.Message);
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmEmployerSelection(int providerSiteId, string edsUrn, Guid vacancyGuid,
            bool? comeFromPreview)
        {
            if (comeFromPreview == true)
            {
                _vacancyPostingMediator.ClearLocationInformation(vacancyGuid);
            }

            return RedirectToRoute(RecruitmentRouteNames.ConfirmEmployer,
                new
                {
                    providerSiteId,
                    edsUrn,
                    vacancyGuid,
                    comeFromPreview
                });
        }

        [HttpGet]
        public ActionResult ConfirmNewEmployerSelection(int providerSiteId, string edsUrn, Guid vacancyGuid,
            bool? comeFromPreview)
        {
            if (comeFromPreview == true)
            {
                _vacancyPostingMediator.ClearLocationInformation(vacancyGuid);
            }

            return RedirectToRoute(RecruitmentRouteNames.ConfirmEmployer,
                new
                {
                    providerSiteId,
                    edsUrn,
                    vacancyGuid,
                    comeFromPreview
                });
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "ConfirmEmployer")]
        [HttpPost]
        public ActionResult ConfirmEmployer(VacancyPartyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel, User.GetUkprn());

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    if (viewModel.ComeFromPreview &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                    }

                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { vacancyPartyId = response.ViewModel.VacancyPartyId, vacancyGuid = response.ViewModel.VacancyGuid, numberOfPositions = response.ViewModel.NumberOfPositions, comeFromPreview = viewModel.ComeFromPreview });
                    }

                    return RedirectToRoute(RecruitmentRouteNames.AddLocations, new { providerSiteId = response.ViewModel.ProviderSiteId, employerId = response.ViewModel.Employer.EmployerId, vacancyGuid = response.ViewModel.VacancyGuid, comeFromPreview = viewModel.ComeFromPreview });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

 
        #endregion

        #region Basic Details

        [HttpGet]
        public ActionResult CreateVacancy(int vacancyPartyId, Guid vacancyGuid, int? numberOfPositions)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(vacancyPartyId, vacancyGuid, numberOfPositions);
            
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult ReviewCreateVacancy(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(vacancyReferenceNumber, true, comeFromPreview);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetNewVacancyViewModel.LocationNotSet:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmEmployer,
                        new
                        {
                            providerSiteId = viewModel.OwnerParty.ProviderSiteId,
                            edsUrn = viewModel.OwnerParty.Employer.EdsUrn,
                            vacancyGuid = viewModel.VacancyGuid,
                            comeFromPreview,
                            useEmployerLocation = viewModel.IsEmployerLocationMainApprenticeshipLocation
                        });
                case VacancyPostingMediatorCodes.GetNewVacancyViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("CreateVacancy", viewModel);

                case VacancyPostingMediatorCodes.GetNewVacancyViewModel.Ok:
                    return View("CreateVacancy", viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancyAndPreview(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel, User.GetUkprn());

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                });

            return HandleCreateVacancy(response, okAction);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancyAndExit(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancyAndExit(viewModel, User.GetUkprn());

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

            return HandleCreateVacancy(response, okAction);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancy(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel, User.GetUkprn());

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.TrainingDetails, 
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                });

            return HandleCreateVacancy(response, okAction);
        }

        [HttpPost]
        public JsonResult AutoSaveCreateVacancy(NewVacancyViewModel viewModel)
        {
            // Call autosave instead of CreateVacancy?
            var response = _vacancyPostingMediator.CreateVacancy(viewModel, User.GetUkprn());

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                case VacancyPostingMediatorCodes.CreateVacancy.OkWithWarning:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult HandleCreateVacancy(MediatorResponse<NewVacancyViewModel> response, Func<ActionResult> okAction )
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                    return okAction();

                case VacancyPostingMediatorCodes.CreateVacancy.OkWithWarning:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return okAction();

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        #endregion

        #region Training Details

        [HttpGet]
        public ActionResult TrainingDetails(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetTrainingDetailsViewModel(vacancyReferenceNumber, false, false);
            var viewModel = response.ViewModel;

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetTrainingDetailsViewModel.Ok:
                    return View(viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetails(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleTrainingDetails(response,
                () => RedirectToRoute(RecruitmentRouteNames.VacancySummary,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        [HttpPost]
        public JsonResult AutoSaveTrainingDetails(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ReviewTrainingDetails(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetTrainingDetailsViewModel(vacancyReferenceNumber, true, comeFromPreview);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetTrainingDetailsViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("TrainingDetails", viewModel);

                case VacancyPostingMediatorCodes.GetTrainingDetailsViewModel.Ok:
                    return View("TrainingDetails", viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetailsAndExit(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleTrainingDetails(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetailsAndPreview(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleTrainingDetails(response,
                () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        private ActionResult HandleTrainingDetails(MediatorResponse<TrainingDetailsViewModel> response, Func<ActionResult> okAction)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return okAction();

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult SelectFramework(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectFrameworkAsTrainingType(viewModel);

            ModelState.Clear();

            return View("TrainingDetails", response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult SelectStandard(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectStandardAsTrainingType(viewModel);

            ModelState.Clear();

            return View("TrainingDetails", response.ViewModel);
        }

        #endregion

        #region Vacancy Details

        [HttpGet]
        public ActionResult VacancySummary(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber, false, false);
            var viewModel = response.ViewModel;

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancySummaryViewModel.Ok:
                    return View(viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ReviewVacancySummary(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber, true, comeFromPreview);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancySummaryViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("VacancySummary", viewModel);

                case VacancyPostingMediatorCodes.GetVacancySummaryViewModel.Ok:
                    return View("VacancySummary", viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummary(FurtherVacancyDetailsViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);

            return HandleVacancySummary(response,
                () => RedirectToRoute(RecruitmentRouteNames.VacancyRequirementsProspects,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        [HttpPost]
        public JsonResult AutoSaveVacancySummary(FurtherVacancyDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, true);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummaryAndExit(FurtherVacancyDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancySummary(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummaryAndPreview(FurtherVacancyDetailsViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);
            
            return HandleVacancySummary(response,
                () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        private ActionResult HandleVacancySummary(MediatorResponse<FurtherVacancyDetailsViewModel> response,
            Func<ActionResult> okAction)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return okAction();

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        #endregion

        #region Requirements and Prospects

        [HttpGet]
        public ActionResult VacancyRequirementsProspects(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber, false, comeFromPreview);
            var viewModel = response.ViewModel;

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok:
                    return View(viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ReviewVacancyRequirementsProspects(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber, true, true);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancyRequirementsProspectsViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("VacancyRequirementsProspects", viewModel);

                case VacancyPostingMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok:
                    return View("VacancyRequirementsProspects", viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyRequirementsProspects")]
        [HttpPost]
        public ActionResult VacancyRequirementsProspects(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleVacancyRequirementsProspects(response);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyRequirementsProspects")]
        [HttpPost]
        public ActionResult VacancyRequirementsProspectsAndExit(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancyRequirementsProspects(response);
        }

        [HttpPost]
        public JsonResult AutoSaveRequirementsProspects(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                case VacancyPostingMediatorCodes.UpdateVacancy.OkAndExit:
                case VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk:
                case VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult HandleVacancyRequirementsProspects(
            MediatorResponse<VacancyRequirementsProspectsViewModel> response)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.OkAndExit:
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                case VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk:
                    return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy, new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });
                case VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk:
                    var routeName = RecruitmentRouteNames.VacancyQuestions;
                    if (response.ViewModel.Status == VacancyStatus.Referred || response.ViewModel.ComeFromPreview)
                    {
                        routeName = RecruitmentRouteNames.PreviewVacancy;
                    }
                    return RedirectToRoute(routeName, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        #endregion

        #region Vacancy Questions

        [HttpGet]
        public ActionResult VacancyQuestions(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber, false, comeFromPreview);
            var viewModel = response.ViewModel;

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancyQuestionsViewModel.Ok:
                    return View(viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult ReviewVacancyQuestions(int vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber, true, comeFromPreview);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancyQuestionsViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("VacancyQuestions", viewModel);

                case VacancyPostingMediatorCodes.GetVacancyQuestionsViewModel.Ok:
                    return View("VacancyQuestions", viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }


        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQuestions")]
        [HttpPost]
        public ActionResult VacancyQuestions(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleVacancyQuestions(response, () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                }));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQuestions")]
        [HttpPost]
        public ActionResult VacancyQuestionsAndExit(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancyQuestions(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        [HttpPost]
        public JsonResult AutoSaveVacancyQuestions(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult HandleVacancyQuestions(MediatorResponse<VacancyQuestionsViewModel> response,
            Func<ActionResult> okAction)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return okAction();

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        #endregion

        #region Preview

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult PreviewVacancy(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetPreviewVacancyViewModel(vacancyReferenceNumber);

            var vacancyViewModel = response.ViewModel;

            vacancyViewModel.BasicDetailsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewCreateVacancy, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.TrainingDetailsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewTrainingDetails, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.SummaryLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancySummary, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.RequirementsProspectsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyRequirementsProspects, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.QuestionsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyQuestions, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.EmployerLink = Url.RouteUrl(RecruitmentRouteNames.ConfirmEmployer, new { providerSiteId = vacancyViewModel.ProviderSite.ProviderSiteId, edsUrn = vacancyViewModel.NewVacancyViewModel.OwnerParty.Employer.EdsUrn, vacancyGuid = vacancyViewModel.NewVacancyViewModel.VacancyGuid, comeFromPreview = true });
            vacancyViewModel.LocationsLink = Url.RouteUrl(RecruitmentRouteNames.AddLocations, new { providerSiteId = vacancyViewModel.ProviderSite.ProviderSiteId, employerId = vacancyViewModel.NewVacancyViewModel.OwnerParty.Employer.EmployerId, vacancyGuid = vacancyViewModel.NewVacancyViewModel.VacancyGuid, comeFromPreview = true });

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    var view = View(vacancyViewModel);
                    return view;

                case VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok:
                    return View(vacancyViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

        }
        
        [HttpPost]
        public ActionResult SubmitVacancy(int vacancyReferenceNumber, bool resubmitoption)
        {
            var response = _vacancyPostingMediator.SubmitVacancy(vacancyReferenceNumber, resubmitoption);
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
                    return View("PreviewVacancy", vacancyViewModel);

                case VacancyPostingMediatorCodes.SubmitVacancy.SubmitOk:
                    return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, resubmitted = false });

                case VacancyPostingMediatorCodes.SubmitVacancy.ResubmitOk:
                    return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, resubmitted = true });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VacancySubmitted(int vacancyReferenceNumber, bool resubmitted)
        {
            var response = _vacancyPostingMediator.GetSubmittedVacancyViewModel(vacancyReferenceNumber, resubmitted);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        #endregion

        [HttpGet]
        public ActionResult SelectNewEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectNewEmployer(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.SelectNewEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.SelectNewEmployer.NoResults:
                case VacancyPostingMediatorCodes.SelectNewEmployer.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmNewEmployer(int providerSiteId, string edsErn, Guid vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteId, edsErn, vacancyGuid, comeFromPreview, null);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployer.Ok:
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.GetEmployer.InvalidEmployerAddress:
                    SetUserMessage(response.Message);
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult ConfirmNewEmployer(VacancyPartyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel, User.GetUkprn());

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    if (viewModel.ComeFromPreview &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                    }

                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteId = response.ViewModel.ProviderSiteId, employerId = response.ViewModel.Employer.EmployerId, vacancyGuid = response.ViewModel.VacancyGuid, numberOfPositions = response.ViewModel.NumberOfPositions, comeFromPreview = viewModel.ComeFromPreview });
                    }

                    return RedirectToRoute(RecruitmentRouteNames.AddLocations, new { providerSiteId = response.ViewModel.ProviderSiteId, employerId = response.ViewModel.Employer.EmployerId, vacancyGuid = response.ViewModel.VacancyGuid, comeFromPreview = viewModel.ComeFromPreview });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult CloneVacancy(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.CloneVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CloneVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmEmployer, new { providerSiteId = response.ViewModel.ProviderSiteId, edsUrn = response.ViewModel.Employer.EdsUrn, vacancyGuid = response.ViewModel.VacancyGuid });
                case VacancyPostingMediatorCodes.CloneVacancy.VacancyInIncorrectState:
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult Locations(int providerSiteId, int employerId, Guid vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetLocationAddressesViewModel(providerSiteId, employerId, User.GetUkprn(), vacancyGuid, comeFromPreview);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetLocationAddressesViewModel.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "AddLocations")]
        [HttpPost]
        public ActionResult Locations(LocationSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.AddLocations(viewModel, User.GetUkprn());

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                    if (viewModel.ComeFromPreview)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                    }
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { vacancyPartyId = response.ViewModel.VacancyPartyId, vacancyGuid = response.ViewModel.VacancyGuid });
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

        }

        [HttpPost]
        public JsonResult AutoSaveLocations(LocationSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.AddLocations(viewModel, User.GetUkprn());

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButtonWithParameter(SubmitButtonActionName = "AddLocations")]
        [HttpPost]
        public ActionResult SearchLocations(LocationSearchViewModel viewModel)
        {
            TempData["AlreadyAddedLocations"] = viewModel.Addresses;
            return RedirectToRoute(RecruitmentRouteNames.SearchAddresses, new
            {
                PostcodeSearch = viewModel.PostcodeSearch,
                ProviderSiteErn = viewModel.ProviderSiteEdsUrn,
                EdsUrn = viewModel.EmployerEdsUrn,
                VacancyGuid = viewModel.VacancyGuid,
                AdditionalLocationInformation = viewModel.AdditionalLocationInformation,
                Ukprn = viewModel.Ukprn,
                CurrentPage = viewModel.CurrentPage,
                TotalNumberOfPages = viewModel.TotalNumberOfPages
            });
        }

        [HttpGet]
        public ActionResult SearchAddresses(LocationSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SearchLocations(viewModel, (List<VacancyLocationAddressViewModel>)TempData["AlreadyAddedLocations"]);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.SearchLocations.Ok:
                    return View("Locations", response.ViewModel);
                case VacancyPostingMediatorCodes.SearchLocations.NotFullPostcode:
                    AddPostcodeSearchErrorToModelState(viewModel);
                    return View("Locations", response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private void AddPostcodeSearchErrorToModelState(LocationSearchViewModel viewModel)
        {
            ModelState.AddModelError("PostcodeSearch", LocationSearchViewModelMessages.PostCodeSearch.ErrorText);
            //To work around an issue with MVC: SetModelValue must be called if AddModelError is called.
            ModelState.SetModelValue("PostcodeSearch",
                new ValueProviderResult(viewModel.PostcodeSearch ?? "", (viewModel.PostcodeSearch ?? ""),
                    CultureInfo.CurrentCulture));
        }

        [HttpGet]
        public ActionResult ShowLocations(LocationSearchViewModel viewModel)
        {
            viewModel.Addresses = (List<VacancyLocationAddressViewModel>) TempData["AlreadyAddedLocations"];
            ModelState.Clear();

            return View("Locations", viewModel);
        }


        [MultipleFormActionsButtonWithParameter(SubmitButtonActionName = "AddLocations")]
        [FillParamterFromActionName(SubmitButtonActionName = "AddLocations", ParameterNames = new []{"locationIndex", "postcodeSearch" }, ParameterTypes = new []{ TypeCode.Int32, TypeCode.String } )]
        [HttpPost]
        public ActionResult UseLocation(LocationSearchViewModel viewModel, int locationIndex, string postcodeSearch)
        {
            var response = _vacancyPostingMediator.UseLocation(viewModel, locationIndex, postcodeSearch);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UseLocation.Ok:
                    return RedirectToShowLocations(viewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        
        [MultipleFormActionsButtonWithParameter(SubmitButtonActionName = "AddLocations")]
        [FillParamterFromActionName(SubmitButtonActionName = "AddLocations", ParameterNames = new[] { "locationIndex" }, ParameterTypes = new[] { TypeCode.Int32 })]
        [HttpPost]
        public ActionResult RemoveLocation(LocationSearchViewModel viewModel, int locationIndex)
        {
            var response = _vacancyPostingMediator.RemoveLocation(viewModel, locationIndex);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.RemoveLocation.Ok:
                    return RedirectToShowLocations(viewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ManageDates(int vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyDatesViewModel(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ManageDates.Ok:
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ManageDates.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult ManageDates(VacancyDatesViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ManageDates.UpdatedHasApplications:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications,
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                case VacancyPostingMediatorCodes.ManageDates.UpdatedNoApplications:
                    return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                case VacancyPostingMediatorCodes.ManageDates.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ManageDates.InvalidState:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications,
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public JsonResult AutoSaveManageDates(VacancyDatesViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, true);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ManageDates.UpdatedHasApplications:
                case VacancyPostingMediatorCodes.ManageDates.UpdatedNoApplications:
                case VacancyPostingMediatorCodes.ManageDates.FailedValidation:
                case VacancyPostingMediatorCodes.ManageDates.InvalidState:
                    ModelState.Clear();
                    return new JsonResult();
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult RedirectToShowLocations(LocationSearchViewModel viewModel)
        {
            TempData["AlreadyAddedLocations"] = viewModel.Addresses;
            return RedirectToRoute(RecruitmentRouteNames.ShowLocations, new
            {
                PostcodeSearch = viewModel.PostcodeSearch,
                ProviderSiteErn = viewModel.ProviderSiteEdsUrn,
                EdsUrn = viewModel.EmployerEdsUrn,
                VacancyGuid = viewModel.VacancyGuid,
                AdditionalLocationInformation = viewModel.AdditionalLocationInformation,
                Ukprn = viewModel.Ukprn,
                CurrentPage = viewModel.CurrentPage,
                TotalNumberOfPages = viewModel.TotalNumberOfPages
            });
        }
    }
}