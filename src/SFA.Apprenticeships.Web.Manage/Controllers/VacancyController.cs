namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Constants;

    [AuthorizeUser(Roles = Roles.Raa)]
    [OwinSessionTimeout]
    public class VacancyController : Controller
    {
        // GET: Vacancy
        public ActionResult Review(string vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Accept(long vacancyReferenceNumber)
        {
            return RedirectToRoute(ManagementRouteNames.Dashboard);
        }
    }
}