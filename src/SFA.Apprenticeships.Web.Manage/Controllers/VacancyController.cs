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
        public ActionResult Review(string vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Approve(long vacancyReferenceNumber)
        {
            var response = _vacancyMediator.ApproveVacancy(vacancyReferenceNumber);

            switch (response.Code)
            {
                case VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
                case VacancyMediatorCodes.ApproveVacancy.Ok:
                    return RedirectToRoute(ManagementRouteNames.ReviewVacancy, response.ViewModel.VacancyReferenceNumber);
                default:
                    return RedirectToRoute(ManagementRouteNames.Dashboard);
            }
        }
    }
}