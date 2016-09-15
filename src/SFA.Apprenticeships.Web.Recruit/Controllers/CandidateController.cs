namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Common.Attributes;
    using Raa.Common.ViewModels.Candidate;

    [OwinSessionTimeout]
    public class CandidateController : RecruitmentControllerBase
    {
        public CandidateController(IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
        }

        public ActionResult Search(CandidateSearchViewModel viewModel)
        {
            return View();
        }
    }
}