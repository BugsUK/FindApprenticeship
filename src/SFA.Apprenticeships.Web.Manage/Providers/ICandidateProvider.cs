namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using ViewModels;

    public interface ICandidateProvider
    {
        CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel);
    }
}