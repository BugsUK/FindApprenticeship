namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.Application.Traineeship;
    using Raa.Common.ViewModels.Candidate;
    using ViewModels;

    public interface ICandidateProvider
    {
        CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel);
        CandidateApplicationsViewModel GetCandidateApplications(Guid candidateId);
        ApprenticeshipApplicationViewModel GetCandidateApprenticeshipApplication(Guid applicationId);
        TraineeshipApplicationViewModel GetCandidateTraineeshipApplication(Guid applicationId);
    }
}