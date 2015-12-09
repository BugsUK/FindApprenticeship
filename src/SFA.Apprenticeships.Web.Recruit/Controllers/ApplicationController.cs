namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Constants;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class ApplicationController : RecruitmentControllerBase
    {
        [HttpGet]
        public ActionResult VacancyApplications(long vacancyReferenceNumber)
        {
            return View();
        }
    }
}