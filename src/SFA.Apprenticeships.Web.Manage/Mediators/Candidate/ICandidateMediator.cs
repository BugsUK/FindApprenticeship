namespace SFA.Apprenticeships.Web.Manage.Mediators.Candidate
{
    using System;
    using Common.Mediators;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.Application.Traineeship;
    using Raa.Common.ViewModels.Candidate;
    using ViewModels;

    public interface ICandidateMediator
    {
        MediatorResponse<CandidateSearchResultsViewModel> Search();
        MediatorResponse<CandidateSearchResultsViewModel> Search(CandidateSearchViewModel viewModel);
        MediatorResponse<CandidateApplicationsViewModel> GetCandidateApplications(Guid candidateId);
        MediatorResponse<ApprenticeshipApplicationViewModel> GetCandidateApprenticeshipApplication(Guid applicationId);
        MediatorResponse<TraineeshipApplicationViewModel> GetCandidateTraineeshipApplication(Guid applicationId);
        MediatorResponse<CandidateVacancy> GetCandidateApprenticeshipVacancyViewModel(int vacancyId, Guid applicationId);
        MediatorResponse<CandidateVacancy> GetCandidateTraineeshipVacancyViewModel(int vacancyId, Guid applicationId);
    }
}