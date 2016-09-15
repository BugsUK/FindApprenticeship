namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Application.Traineeship;
    using ViewModels.Candidate;

    public interface ICandidateProvider
    {
        CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel);
        CandidateApplicationsViewModel GetCandidateApplications(Guid candidateId);
        ApprenticeshipApplicationViewModel GetCandidateApprenticeshipApplication(Guid applicationId);
        TraineeshipApplicationViewModel GetCandidateTraineeshipApplication(Guid applicationId);
        CandidateApplicationSummariesViewModel GetCandidateApplicationSummaries(CandidateApplicationsSearchViewModel searchViewModel);
    }
}