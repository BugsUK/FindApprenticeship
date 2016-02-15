namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Common.ViewModels;

    public class CandidateSearchResultsViewModel
    {
        public CandidateSearchViewModel SearchViewModel { get; set; }

        public PageableViewModel<CandidateSummaryViewModel> Candidates { get; set; }
    }
}