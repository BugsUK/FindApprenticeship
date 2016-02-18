namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Mediators;
    using Constants;
    using Domain.Entities;
    using Domain.Entities.Raa;
    using Mediators.Application;
    using ViewModels.Application;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class ApplicationController : RecruitmentControllerBase
    {
        private readonly IApplicationMediator _applicationMediator;

        public ApplicationController(IApplicationMediator applicationMediator)
        {
            _applicationMediator = applicationMediator;
        }

        [HttpGet]
        public ActionResult VacancyApplications(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var response = _applicationMediator.GetVacancyApplicationsViewModel(vacancyApplicationsSearch);

            switch (response.Code)
            {
                case ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult VacancyApplications(VacancyApplicationsViewModel vacancyApplications)
        {
            return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, vacancyApplications.VacancyApplicationsSearch);
        }
    }
}