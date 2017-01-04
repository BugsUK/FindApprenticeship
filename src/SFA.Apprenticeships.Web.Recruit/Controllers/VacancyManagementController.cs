namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Mediators.VacancyManagement;
    using Mediators.VacancyPosting;
    using Raa.Common.ViewModels.VacancyManagement;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    [OwinSessionTimeout]
    public class VacancyManagementController : RecruitmentControllerBase
    {
        private readonly IVacancyManagementMediator _vacancyManagementMediator;

        public VacancyManagementController(IVacancyManagementMediator vacancyManagementMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _vacancyManagementMediator = vacancyManagementMediator;
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteGet(DeleteVacancyViewModel vacancyViewModel)
        {
            var result = _vacancyManagementMediator.ConfirmDelete(vacancyViewModel);
            if (result.Code == VacancyManagementMediatorCodes.ConfirmDelete.NotFound)
            {
                return HttpNotFound();
            }
            return View(result.ViewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(DeleteVacancyViewModel vacancyViewModel)
        {
            var response = _vacancyManagementMediator.Delete(vacancyViewModel);
            SetUserMessage(response.Message);
            return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome, vacancyViewModel.RouteValues);
        }

        [HttpGet]
        [ActionName("EditWage")]
        public ActionResult EditWageGet(int vacancyReferenceNumber)
        {
            var result = _vacancyManagementMediator.EditWage(vacancyReferenceNumber);
            if (result.Code == VacancyManagementMediatorCodes.EditWage.NotFound)
            {
                return HttpNotFound();
            }
            return View(result.ViewModel);
        }

        [HttpPost]
        [ActionName("EditWage")]
        public async Task<ActionResult> EditWagePost(EditWageViewModel editWageViewModel)
        {
            var result = await _vacancyManagementMediator.EditWage(editWageViewModel);
            if (result.Code == VacancyManagementMediatorCodes.EditWage.NotFound)
            {
                return HttpNotFound();
            }
            if (result.Code == VacancyManagementMediatorCodes.EditWage.FailedValidation)
            {
                ModelState.Clear();
                result.ValidationResult.AddToModelState(ModelState, string.Empty);
                return View(result.ViewModel);
            }
            return View(result.ViewModel);
        }
    }
}