namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Mediators;
    using Mediators.Application;

    public class ApprenticeshipApplicationController : RecruitmentControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
        }

        [HttpGet]
        public ActionResult Review(Guid applicationId)
        {
            var response = _apprenticeshipApplicationMediator.GetApprenticeshipApplicationViewModel(applicationId);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.GetApprenticeshipApplicationViewModel.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}