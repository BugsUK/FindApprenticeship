namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Mediators;
    using Mediators.Application;
    using SFA.Infrastructure.Interfaces;

    public class EmployerApplicationController : RecruitmentControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public EmployerApplicationController(IConfigurationService configurationService, ILogService loggingService,
            IApprenticeshipApplicationMediator apprenticeshipApplicationMediator)
            : base(configurationService, loggingService)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
        }

        public ActionResult ViewAnonymised(string application)
        {
            var response = _apprenticeshipApplicationMediator.View(application);

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
    }
}