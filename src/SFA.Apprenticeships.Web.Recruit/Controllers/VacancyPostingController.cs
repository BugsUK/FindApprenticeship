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
    using Domain.Entities;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;

    //TODO: Split this class by code region
    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : RecruitmentControllerBase
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

        #region Employer Selection

        [HttpGet]
        public ActionResult SelectEmployer(string providerSiteErn, Guid? vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(providerSiteErn, vacancyGuid, comeFromPreview);

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
                            new { providerSiteErn = providerSiteErn, vacancyGuid = vacancyGuid });
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
        public ActionResult ConfirmEmployer(string providerSiteErn, string ern, Guid vacancyGuid, bool? comeFromPreview, bool? useEmployerLocation)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid, comeFromPreview, useEmployerLocation);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployer.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmEmployerSelection(string providerSiteErn, string ern, Guid vacancyGuid,
            bool? comeFromPreview)
        {
            if (comeFromPreview == true)
            {
                _vacancyPostingMediator.ClearLocationInformation(vacancyGuid);
            }

            return RedirectToRoute(RecruitmentRouteNames.ComfirmEmployer,
                new
                {
                    providerSiteErn = providerSiteErn,
                    ern = ern,
                    vacancyGuid = vacancyGuid,
                    comeFromPreview = comeFromPreview
                });
        }

        [HttpGet]
        public ActionResult ConfirmNewEmployerSelection(string providerSiteErn, string ern, Guid vacancyGuid,
            bool? comeFromPreview)
        {
            if (comeFromPreview == true)
            {
                _vacancyPostingMediator.ClearLocationInformation(vacancyGuid);
            }

            return RedirectToRoute(RecruitmentRouteNames.ComfirmEmployer,
                new
                {
                    providerSiteErn = providerSiteErn,
                    ern = ern,
                    vacancyGuid = vacancyGuid,
                    comeFromPreview = comeFromPreview
                });
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "ConfirmEmployer")]
        [HttpPost]
        public ActionResult ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    if (viewModel.ComeFromPreview &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                    }

                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, numberOfPositions = response.ViewModel.NumberOfPositions, comeFromPreview = viewModel.ComeFromPreview });
                    }

                    return RedirectToRoute(RecruitmentRouteNames.AddLocations, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, comeFromPreview = viewModel.ComeFromPreview });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

 
        #endregion

        #region Basic Details

        [HttpGet]
        public ActionResult CreateVacancy(string providerSiteErn, string ern, Guid vacancyGuid, int? numberOfPositions)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(User.GetUkprn(), providerSiteErn, ern, vacancyGuid, numberOfPositions);
            
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult ReviewCreateVacancy(long vacancyReferenceNumber, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(vacancyReferenceNumber, true, comeFromPreview);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            switch (response.Code)
            {
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
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);

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
            var response = _vacancyPostingMediator.CreateVacancyAndExit(viewModel);

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

            return HandleCreateVacancy(response, okAction);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancy(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.TrainingDetails, 
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                });

            return HandleCreateVacancy(response, okAction);
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
        public ActionResult TrainingDetails(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetTrainingDetailsViewModel(vacancyReferenceNumber);

            return View(response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetails(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancySummary, new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetailsAndExit(TrainingDetailsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetailsAndPreview(TrainingDetailsViewModel viewModel)
        {
            throw new NotImplementedException();
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
        public ActionResult VacancySummary(long vacancyReferenceNumber)
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
        public ActionResult ReviewVacancySummary(long vacancyReferenceNumber, bool? comeFromPreview)
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
        public ActionResult VacancySummary(VacancySummaryViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);

            return HandleVacancySummary(response,
                () => RedirectToRoute(RecruitmentRouteNames.VacancyRequirementsProspects,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummaryAndExit(VacancySummaryViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancySummary(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummaryAndPreview(VacancySummaryViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);
            
            return HandleVacancySummary(response,
                () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        private ActionResult HandleVacancySummary(MediatorResponse<VacancySummaryViewModel> response,
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
        public ActionResult VacancyRequirementsProspects(long vacancyReferenceNumber, bool? comeFromPreview)
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
        public ActionResult ReviewVacancyRequirementsProspects(long vacancyReferenceNumber)
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
                    if (response.ViewModel.Status == ProviderVacancyStatuses.RejectedByQA || response.ViewModel.ComeFromPreview)
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
        public ActionResult VacancyQuestions(long vacancyReferenceNumber, bool? comeFromPreview)
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

        public ActionResult ReviewVacancyQuestions(long vacancyReferenceNumber, bool? comeFromPreview)
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
        public ActionResult PreviewVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetPreviewVacancyViewModel(vacancyReferenceNumber);

            var vacancyViewModel = response.ViewModel;

            vacancyViewModel.BasicDetailsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewCreateVacancy, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.TrainingDetailsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewTrainingDetails, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.SummaryLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancySummary, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.RequirementsProspectsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyRequirementsProspects, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.QuestionsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyQuestions, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber, comeFromPreview = true });
            vacancyViewModel.EmployerLink = Url.RouteUrl(RecruitmentRouteNames.ComfirmEmployer, new { providerSiteErn = vacancyViewModel.ProviderSite.Ern, ern = vacancyViewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern , vacancyGuid = vacancyViewModel.NewVacancyViewModel.VacancyGuid, comeFromPreview = true });
            vacancyViewModel.LocationsLink = Url.RouteUrl(RecruitmentRouteNames.AddLocations, new { providerSiteErn = vacancyViewModel.ProviderSite.Ern, ern = vacancyViewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern , vacancyGuid = vacancyViewModel.NewVacancyViewModel.VacancyGuid, comeFromPreview = true });

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
        public ActionResult SubmitVacancy(long vacancyReferenceNumber, bool resubmitoption)
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
        public ActionResult VacancySubmitted(long vacancyReferenceNumber, bool resubmitted)
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
        public ActionResult ConfirmNewEmployer(string providerSiteErn, string ern, Guid vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid, comeFromPreview, null);
            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult ConfirmNewEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    if (viewModel.ComeFromPreview &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                    }

                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, numberOfPositions = response.ViewModel.NumberOfPositions, comeFromPreview = viewModel.ComeFromPreview });
                    }

                    return RedirectToRoute(RecruitmentRouteNames.AddLocations, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, comeFromPreview = viewModel.ComeFromPreview });
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
                    return RedirectToRoute(RecruitmentRouteNames.ComfirmEmployer, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
                case VacancyPostingMediatorCodes.CloneVacancy.VacancyInIncorrectState:
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult Locations(string providerSiteErn, string ern, Guid vacancyGuid, bool? comeFromPreview)
        {
            var response = _vacancyPostingMediator.GetLocationAddressesViewModel(providerSiteErn, ern, User.GetUkprn(), vacancyGuid, comeFromPreview);

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
            var response = _vacancyPostingMediator.AddLocations(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                    if (viewModel.ComeFromPreview)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                            new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                    }
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
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
                ProviderSiteErn = viewModel.ProviderSiteErn,
                Ern = viewModel.Ern,
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
        public ActionResult ManageDates(long vacancyReferenceNumber)
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

        private ActionResult RedirectToShowLocations(LocationSearchViewModel viewModel)
        {
            TempData["AlreadyAddedLocations"] = viewModel.Addresses;
            return RedirectToRoute(RecruitmentRouteNames.ShowLocations, new
            {
                PostcodeSearch = viewModel.PostcodeSearch,
                ProviderSiteErn = viewModel.ProviderSiteErn,
                Ern = viewModel.Ern,
                VacancyGuid = viewModel.VacancyGuid,
                AdditionalLocationInformation = viewModel.AdditionalLocationInformation,
                Ukprn = viewModel.Ukprn,
                CurrentPage = viewModel.CurrentPage,
                TotalNumberOfPages = viewModel.TotalNumberOfPages
            });
        }
    }
}