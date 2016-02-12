namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using Common.Mediators;
    using ViewModels;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search();

        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchResultsViewModel viewModel);
    }
}