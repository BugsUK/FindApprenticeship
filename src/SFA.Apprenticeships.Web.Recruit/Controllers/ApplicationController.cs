namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa;
    using Mediators.Application;
    using Raa.Common.ViewModels.Application;
    using SFA.Infrastructure.Interfaces;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class ApplicationController : RecruitmentControllerBase
    {
        private readonly IApplicationMediator _applicationMediator;

        public ApplicationController(IApplicationMediator applicationMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
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

        [HttpGet]
        public ActionResult ShareApplications(int vacancyReferenceNumber)
        {
            var response = _applicationMediator.ShareApplications(vacancyReferenceNumber);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.View.Ok:
                    return View(response.ViewModel);
                case ApprenticeshipApplicationMediatorCodes.View.LinkExpired:
                    return View("LinkExpired");
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