namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using Mediators.Application;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class ApplicationController : RecruitmentControllerBase
    {
        private readonly IApplicationMediator _applicationMediator;

        public ApplicationController(IApplicationMediator applicationMediator)
        {
            _applicationMediator = applicationMediator;
        }

        [HttpGet]
        public ActionResult VacancyApplications(long vacancyReferenceNumber)
        {
            var result = _applicationMediator.GetVacancyApplicationsViewModel(vacancyReferenceNumber);

            return View(result.ViewModel);
        }
    }
}