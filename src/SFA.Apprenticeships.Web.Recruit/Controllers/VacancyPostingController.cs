namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Common.ViewModels;
    using Constants;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
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
        public ActionResult SelectEmployer(string providerSiteErn, Guid? vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(providerSiteErn, vacancyGuid);

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
                case VacancyPostingMediatorCodes.GetProviderEmployers.NoResults:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        
        [HttpGet]
        public ActionResult SearchExistingEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("SelectEmployer", response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
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
        public ActionResult ConfirmEmployer(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployer.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
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
                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, numberOfPositions = response.ViewModel.NumberOfPositions });
                    }
                    return RedirectToRoute(RecruitmentRouteNames.AddLocations, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
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
        public ActionResult ReviewCreateVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(vacancyReferenceNumber, true);
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

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.VacancySummary, 
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

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult SelectFramework(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectFrameworkAsTrainingType(viewModel);

            ModelState.Clear();

            return View("CreateVacancy", response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult SelectStandard(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectStandardAsTrainingType(viewModel);

            ModelState.Clear();

            return View("CreateVacancy", response.ViewModel);
        }

        #endregion

        #region Vacancy Details

        [HttpGet]
        public ActionResult VacancySummary(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber, false);
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
        public ActionResult ReviewVacancySummary(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber, true);
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
        public ActionResult VacancyRequirementsProspects(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber, false);
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
            var response = _vacancyPostingMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber, true);
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
                    if (response.ViewModel.Status == ProviderVacancyStatuses.RejectedByQA)
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
        public ActionResult VacancyQuestions(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber, false);
            var viewModel = response.ViewModel;

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetVacancyQuestionsViewModel.Ok:
                    return View(viewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        public ActionResult ReviewVacancyQuestions(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber, true);
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

            vacancyViewModel.BasicDetailsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewCreateVacancy, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.SummaryLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancySummary, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.RequirementsProspectsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyRequirementsProspects, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.QuestionsLink = Url.RouteUrl(RecruitmentRouteNames.ReviewVacancyQuestions, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });

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

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

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
        public ActionResult ConfirmNewEmployer(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid);
            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult ConfirmNewEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
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
        public ActionResult Locations(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetLocationAddressesViewModel(providerSiteErn, ern, User.GetUkprn(), vacancyGuid);

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
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
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