using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Common.Validators.Extensions;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Mediators.Vacancy;

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

        public ActionResult EditBasicDetails(long vacancyReferenceNumber)
        {
            var model = _vacancyMediator.GetBasicDetails(vacancyReferenceNumber);
            return View(model.ViewModel);
        }

        [HttpPost]
        public ActionResult SaveBasicDetails(NewVacancyViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Summary(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.GetVacancySummaryViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Summary(VacancySummaryViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyMediator.UpdateVacancy(viewModel, acceptWarnings);

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

        public ActionResult EditRequirementsAndProspoects(long vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult SaveRequirementsAndProspoects(VacancyRequirementsProspectsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult EditQuestions(long vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult SaveQuestions(VacancyQuestionsViewModel viewModel)
        {
            throw new NotImplementedException();
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