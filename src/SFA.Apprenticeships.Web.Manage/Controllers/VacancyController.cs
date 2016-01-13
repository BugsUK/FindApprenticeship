namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.Vacancy;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Domain.Entities;
    using FluentValidation.Mvc;
    using Infrastructure.Presentation;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.VacancyPosting;

    [AuthorizeUser(Roles = Roles.Raa)]
    [OwinSessionTimeout]
    public class VacancyController : ManagementControllerBase
    {
        private readonly IVacancyMediator _vacancyMediator;

        public VacancyController(IVacancyMediator vacancyMediator)
        {
            _vacancyMediator = vacancyMediator;
        }

        // GET: Vacancy
        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Review(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ReserveVacancyForQA(vacancyReferenceNumber);
            var vacancyViewModel = response.ViewModel;

            vacancyViewModel.BasicDetailsLink = Url.RouteUrl(ManagementRouteNames.BasicDetails,
                new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
            vacancyViewModel.SummaryLink = Url.RouteUrl(ManagementRouteNames.Summary,
                new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
            vacancyViewModel.RequirementsProspectsLink = Url.RouteUrl(ManagementRouteNames.RequirementsAndProspoects,
                new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
            vacancyViewModel.QuestionsLink = Url.RouteUrl(ManagementRouteNames.Questions,
                new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
            vacancyViewModel.EmployerLink = Url.RouteUrl(ManagementRouteNames.EmployerInformation,
                new {vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber});
            vacancyViewModel.LocationsLink = Url.RouteUrl(ManagementRouteNames.AddLocations,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });

            vacancyViewModel.IsEditable = vacancyViewModel.Status.IsStateReviewable();

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    var view = View(vacancyViewModel);
                    return view;

                case VacancyMediatorCodes.GetVacancy.Ok:
                    return View(vacancyViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult BasicDetails(long vacancyReferenceNumber)
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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "BasicDetails")]
        [HttpPost]
        public ActionResult SelectFramework(NewVacancyViewModel viewModel)
        {
            var response = _vacancyMediator.SelectFrameworkAsTrainingType(viewModel);

            ModelState.Clear();

            return View("BasicDetails", response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "BasicDetails")]
        [HttpPost]
        public ActionResult SelectStandard(NewVacancyViewModel viewModel)
        {
            var response = _vacancyMediator.SelectStandardAsTrainingType(viewModel);

            ModelState.Clear();

            return View("BasicDetails", response.ViewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Summary(long vacancyReferenceNumber)
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
        public ActionResult Summary(VacancySummaryViewModel viewModel)
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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult RequirementsAndProspects(long vacancyReferenceNumber)
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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true, VaryByParam = "none")]
        public ActionResult Questions(long vacancyReferenceNumber)
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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQAAction")]
        [HttpPost]
        public ActionResult Approve(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ApproveVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.ApproveVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQAAction")]
        [HttpPost]
        public ActionResult Reject(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.RejectVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.RejectVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy,
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }

        [HttpGet]
        public ActionResult EmployerInformation(long vacancyReferenceNumber, bool? useEmployerLocation)
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
        public ActionResult EmployerInformation(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateEmployerInformation(viewModel);

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyMediatorCodes.UpdateEmployerInformation.Ok:
                    if (response.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
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
        public ActionResult Locations(long vacancyReferenceNumber)
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
                        new {vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber});

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