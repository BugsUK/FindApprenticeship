namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Domain.Entities;
    using Mediators.Candidate;
    using ViewModels;

    public class CandidateController : ManagementControllerBase
    {
        private readonly ICandidateMediator _candidateMediator;

        public CandidateController(ICandidateMediator candidateMediator)
        {
            _candidateMediator = candidateMediator;
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Search()
        {
            var result = _candidateMediator.Search();

            return View(result.ViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Search(CandidateSearchResultsViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}