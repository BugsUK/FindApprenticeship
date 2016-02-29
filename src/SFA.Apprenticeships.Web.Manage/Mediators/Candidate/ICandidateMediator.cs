namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using System;
    using Common.Mediators;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.Application.Traineeship;
    using ViewModels;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search();
        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel viewModel);
        MediatorResponse<CandidateApplicationsViewModel> GetCandidateApplications(Guid candidateId);
        MediatorResponse<ApprenticeshipApplicationViewModel> GetCandidateApprenticeshipApplication(Guid applicationId);
        MediatorResponse<TraineeshipApplicationViewModel> GetCandidateTraineeshipApplication(Guid applicationId);
    }
}