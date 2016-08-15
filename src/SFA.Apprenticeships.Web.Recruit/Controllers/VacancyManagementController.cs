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
        public ActionResult DeleteGet(int vacancyId)
        {
            //TODO add VacanciesSummarySearchViewModel
            var vacancyViewModel = new DeleteVacancyViewModel {VacancyId = vacancyId};
            var result = _vacancyManagementMediator.ConfirmDelete(vacancyViewModel);
            return View(result.ViewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int vacancyId)
        {
            var vacancyViewModel = new DeleteVacancyViewModel { VacancyId = vacancyId };
            _vacancyManagementMediator.Delete(vacancyViewModel);
            // TODO redirect with VacanciesSummarySearchViewModel
            return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);
        }
    }
}