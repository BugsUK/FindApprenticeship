namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using System;
    using Common.Mediators;
    using ViewModels;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search();
        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel viewModel);
        MediatorResponse<CandidateApplicationsViewModel> GetCandidateApplications(Guid candidateId);
    }
}