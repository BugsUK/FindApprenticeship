namespace SFA.Apprenticeships.Web.Recruit.Mediators.Candidate
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Candidate;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel searchViewModel);
    }
}