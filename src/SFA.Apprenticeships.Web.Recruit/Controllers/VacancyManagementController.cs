namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Extensions;
    using Constants;
    using Mediators.VacancyManagement;
    using Mediators.VacancyPosting;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using SFA.Infrastructure.Interfaces;

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
            return View(result.ViewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(DeleteVacancyViewModel vacancyViewModel)
        {
            var response = _vacancyManagementMediator.Delete(vacancyViewModel);
            SetUserMessage(response.Message);
            return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome, vacancyViewModel);
        }
    }
}