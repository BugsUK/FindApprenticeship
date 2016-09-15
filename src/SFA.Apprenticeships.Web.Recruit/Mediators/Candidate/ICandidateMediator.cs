namespace SFA.Apprenticeships.Web.Recruit.Mediators.Candidate
{
    using System;
    using Common.Mediators;
    using Raa.Common.ViewModels.Candidate;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel searchViewModel);
        MediatorResponse<CandidateApplicationSummariesViewModel> GetCandidateApplications(Guid candidateGuid);
    }
}