﻿namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Mvc;
    using Infrastructure.Presentation;
    using Mediators.Vacancy;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    [AuthorizeUser(Roles = Roles.Raa)]
    [OwinSessionTimeout]
    public class VacancyController : ManagementControllerBase
    {
        private readonly IVacancyMediator _vacancyMediator;

        public VacancyController(IVacancyMediator vacancyMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _vacancyMediator = vacancyMediator;
        }

        // GET: Vacancy
        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Review(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ReviewVacancy(vacancyReferenceNumber);

            var vacancyViewModel = response.ViewModel;

            if (response.ViewModel != null)
            {
                SetLinks(vacancyViewModel);                
            }

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.ReviewVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(vacancyViewModel);

                case VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInApiWithValidationErrors:
                case VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInAvmsWithValidationErrors:
                    SetUserMessage(response.Message);
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(vacancyViewModel);

                case VacancyMediatorCodes.ReviewVacancy.Ok:
                    return View(vacancyViewModel);

                case VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInAvms:
                case VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInApi:
                    SetUserMessage(response.Message);
                    return View(vacancyViewModel);

                case VacancyMediatorCodes.ReviewVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult ReserveForQA(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ReserveVacancyForQA(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.ReserveVacancyForQA.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });
                case VacancyMediatorCodes.ReserveVacancyForQA.NextAvailableVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });
                case VacancyMediatorCodes.ReserveVacancyForQA.NoVacanciesAvailable:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQAAction")]
        [HttpPost]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult UnReserveForQA(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.UnReserveVacancyForQA(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.UnReserveVacancyForQA.Ok:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult BasicDetails(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetBasicDetails(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetBasicVacancyDetails.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetBasicVacancyDetails.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "BasicDetails")]
        [HttpPost]
        public ActionResult BasicDetails(NewVacancyViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            return HandleBasicDetails(response);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BasicDetails")]
        public ActionResult SingleOfflineApplicationUrl(NewVacancyViewModel viewModel)
        {
            viewModel.OfflineVacancyType = OfflineVacancyType.SingleUrl;
            var response = _vacancyMediator.UpdateOfflineVacancyType(viewModel);

            return HandleBasicDetails(response);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "BasicDetails")]
        public ActionResult MultipleOfflineApplicationUrls(NewVacancyViewModel viewModel)
        {
            viewModel.OfflineVacancyType = OfflineVacancyType.MultiUrl;
            var response = _vacancyMediator.UpdateOfflineVacancyType(viewModel);

            return HandleBasicDetails(response);
        }

        private ActionResult HandleBasicDetails(MediatorResponse<NewVacancyViewModel> response)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });

                case VacancyMediatorCodes.UpdateVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult TrainingDetails(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetTrainingDetails(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetTrainingDetails.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetTrainingDetails.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult TrainingDetails(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });

                case VacancyMediatorCodes.UpdateVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult SelectFramework(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyMediator.SelectFrameworkAsTrainingType(viewModel);

            ModelState.Clear();

            return View("TrainingDetails", response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "TrainingDetails")]
        [HttpPost]
        public ActionResult SelectStandard(TrainingDetailsViewModel viewModel)
        {
            var response = _vacancyMediator.SelectStandardAsTrainingType(viewModel);

            ModelState.Clear();

            return View("TrainingDetails", response.ViewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Summary(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetVacancySummaryViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult Summary(FurtherVacancyDetailsViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });

                case VacancyMediatorCodes.UpdateVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult RequirementsAndProspects(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult RequirementsAndProspects(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                case VacancyMediatorCodes.UpdateVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Questions(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetVacancyQuestionsViewModel.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetVacancyQuestionsViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult Questions(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });

                case VacancyMediatorCodes.UpdateVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQAAction")]
        [HttpPost]
        public ActionResult Approve(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ApproveVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.ApproveVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.ApproveVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                case VacancyMediatorCodes.ApproveVacancy.PostcodeLookupFailed:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new { vacancyReferenceNumber });
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQAAction")]
        [HttpPost]
        public ActionResult Reject(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.RejectVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.RejectVacancy.InvalidVacancy:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.RejectVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }

        [HttpGet]
        public ActionResult EmployerInformation(int vacancyReferenceNumber, bool? useEmployerLocation)
        {
            var response = _vacancyMediator.GetEmployerInformation(vacancyReferenceNumber, useEmployerLocation);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetEmployerInformation.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.GetEmployerInformation.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult EmployerInformation(VacancyOwnerRelationshipViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateEmployerInformation(viewModel);

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyMediatorCodes.UpdateEmployerInformation.Ok:
                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                        response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
                    {
                        return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                            new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                    }

                    return RedirectToRoute(ManagementRouteNames.AddLocations, new { vacancyReferenceNumber = viewModel.VacancyReferenceNumber });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult Locations(int vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetLocationAddressesViewModel(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetLocationAddressesViewModel.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "AddLocations")]
        [HttpPost]
        public ActionResult Locations(LocationSearchViewModel viewModel)
        {
            var response = _vacancyMediator.AddLocations(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.AddLocations.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });

                case VacancyMediatorCodes.AddLocations.FailedValidation:
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
            return RedirectToRoute(ManagementRouteNames.SearchAddresses, new
            {
                PostcodeSearch = viewModel.PostcodeSearch,
                VacancyGuid = viewModel.VacancyGuid,
                AdditionalLocationInformation = viewModel.AdditionalLocationInformation,
                VacancyReferenceNumber = viewModel.VacancyReferenceNumber
            });
        }

        [HttpGet]
        public ActionResult SearchAddresses(LocationSearchViewModel viewModel)
        {
            var response = _vacancyMediator.SearchLocations(viewModel, (List<VacancyLocationAddressViewModel>)TempData["AlreadyAddedLocations"]);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.SearchLocations.Ok:
                    return View("Locations", response.ViewModel);
                case VacancyMediatorCodes.SearchLocations.NotFullPostcode:
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

        [MultipleFormActionsButtonWithParameter(SubmitButtonActionName = "AddLocations")]
        [FillParamterFromActionName(SubmitButtonActionName = "AddLocations", ParameterNames = new[] { "locationIndex", "postcodeSearch" }, ParameterTypes = new[] { TypeCode.Int32, TypeCode.String })]
        [HttpPost]
        public ActionResult UseLocation(LocationSearchViewModel viewModel, int locationIndex, string postcodeSearch)
        {
            var response = _vacancyMediator.UseLocation(viewModel, locationIndex, postcodeSearch);

            switch (response.Code)
            {
                case VacancyMediatorCodes.UseLocation.Ok:
                    return RedirectToShowLocations(viewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ShowLocations(LocationSearchViewModel viewModel)
        {
            viewModel.Addresses = (List<VacancyLocationAddressViewModel>)TempData["AlreadyAddedLocations"];
            ModelState.Clear();

            return View("Locations", viewModel);
        }

        [MultipleFormActionsButtonWithParameter(SubmitButtonActionName = "AddLocations")]
        [FillParamterFromActionName(SubmitButtonActionName = "AddLocations", ParameterNames = new[] { "locationIndex" }, ParameterTypes = new[] { TypeCode.Int32 })]
        [HttpPost]
        public ActionResult RemoveLocation(LocationSearchViewModel viewModel, int locationIndex)
        {
            var response = _vacancyMediator.RemoveLocation(viewModel, locationIndex);

            switch (response.Code)
            {
                case VacancyMediatorCodes.RemoveLocation.Ok:
                    return RedirectToShowLocations(viewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult RedirectToShowLocations(LocationSearchViewModel viewModel)
        {
            TempData["AlreadyAddedLocations"] = viewModel.Addresses;
            return RedirectToRoute(ManagementRouteNames.ShowLocations, new
            {
                PostcodeSearch = viewModel.PostcodeSearch,
                VacancyGuid = viewModel.VacancyGuid,
                AdditionalLocationInformation = viewModel.AdditionalLocationInformation,
                CurrentPage = viewModel.CurrentPage,
                VacancyReferenceNumber = viewModel.VacancyReferenceNumber
            });
        }
    }
}