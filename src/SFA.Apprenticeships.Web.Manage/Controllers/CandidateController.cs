namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Domain.Entities;

    public class CandidateController : ManagementControllerBase
    {
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Search()
        {
            return View();
        }
    }
}