namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using Domain.Entities.Raa;
    using Mediators.VacancyPosting;
    using Mediators.VacancyStatus;
    using SFA.Infrastructure.Interfaces;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class VacancyStatusController : RecruitmentControllerBase
    {
        private readonly IVacancyStatusMediator _vacancyStatusMediator;

        public VacancyStatusController(IVacancyStatusMediator vacancyStatusMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _vacancyStatusMediator = vacancyStatusMediator;
        }

        // GET: VacancyStatus
        public ActionResult Archive(int vacancyReferenceNumber)
        {
            var response = _vacancyStatusMediator.GetArchiveVacancyViewModel(vacancyReferenceNumber);
            return View(response.ViewModel);
        }
    }
}