namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Mediators;
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
            var response = _applicationMediator.GetVacancyApplicationsViewModel(vacancyReferenceNumber);

            switch (response.Code)
            {
                case ApplicationMediatorCodes.GetVacancyApplicationsViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}