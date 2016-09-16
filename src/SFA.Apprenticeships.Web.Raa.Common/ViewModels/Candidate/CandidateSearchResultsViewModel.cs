namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using Web.Common.ViewModels;

    public class CandidateSearchResultsViewModel
    {
        public CandidateSearchViewModel SearchViewModel { get; set; }

        public PageableViewModel<CandidateSummaryViewModel> Candidates { get; set; }
    }
}