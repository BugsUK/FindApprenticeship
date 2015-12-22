namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.Vacancy;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using FluentValidation.Mvc;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.Provider;
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
            // vacancyViewModel.LocationsLink = Url.RouteUrl(ManagementRouteNames.AddLocations, new { providerSiteErn = vacancyViewModel.ProviderSite.Ern, ern = vacancyViewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern, vacancyGuid = vacancyViewModel.NewVacancyViewModel.VacancyGuid, comeFromPreview = true });

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
        public ActionResult EmployerInformation(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetEmployerInformation(vacancyReferenceNumber);

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

                    throw new InvalidMediatorCodeException(response.Code);
                //return RedirectToRoute(ManagementRouteNames.AddLocations, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid, comeFromPreview = viewModel.ComeFromPreview });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}