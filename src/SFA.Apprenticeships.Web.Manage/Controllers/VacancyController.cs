namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.Vacancy;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using FluentValidation.Mvc;
    using Raa.Common.ViewModels.Vacancy;

    [AuthorizeUser(Roles = Roles.Raa)]
    [OwinSessionTimeout]
    public class VacancyController : Controller
    {
        private readonly IVacancyMediator _vacancyMediator;

        public VacancyController(IVacancyMediator vacancyMediator)
        {
            _vacancyMediator = vacancyMediator;
        }

        // GET: Vacancy
        public ActionResult Review(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ReserveVacancyForQA(vacancyReferenceNumber);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.GetVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    var view = View(response.ViewModel);
                    return view;

                case VacancyMediatorCodes.GetVacancy.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult BasicDetails(long vacancyReferenceNumber)
        {
            var model = _vacancyMediator.GetBasicDetails(vacancyReferenceNumber);
            return View(model.ViewModel);
        }

        [HttpPost]
        public ActionResult BasicDetails(NewVacancyViewModel viewModel)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.Summary,
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

        public ActionResult RequirementsAndProspoects(long vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult RequirementsAndProspoects(VacancyRequirementsProspectsViewModel viewModel)
        {
            throw new NotImplementedException();
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
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy, new { vacancyReferenceNumber =  response.ViewModel.VacancyReferenceNumber});
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
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy, new { vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber });
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }
    }
}