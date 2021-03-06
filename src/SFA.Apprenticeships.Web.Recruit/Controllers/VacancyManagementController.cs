﻿namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Constants;
    using Mediators.VacancyManagement;
    using Mediators.VacancyPosting;
    using Raa.Common.ViewModels.Vacancy;

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
    }
}