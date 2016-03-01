namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using ViewModels;

    public interface ICandidateProvider
    {
        CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel);
        CandidateApplicationsViewModel GetCandidateApplications(Guid candidateId);
    }
}