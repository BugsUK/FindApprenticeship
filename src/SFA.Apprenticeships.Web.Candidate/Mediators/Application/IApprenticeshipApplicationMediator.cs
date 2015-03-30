namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System;
    using ViewModels.Applications;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipApplicationMediator
    {
        MediatorResponse<ApprenticeshipApplicationViewModel> Apply(Guid candidateId, string vacancyIdString);

        MediatorResponse<ApprenticeshipApplicationViewModel> Resume(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationViewModel> Save(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<AutoSaveResultViewModel> AutoSave(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyQualificationRows(ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyWorkExperienceRows(ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> PreviewAndSubmit(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> Preview(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId);

        MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, string vacancyIdString, string vacancyReference, string vacancyTitle);

        MediatorResponse<ApprenticeshipApplicationViewModel> View(Guid candidateId, int vacancyId);

        MediatorResponse<SavedVacancyViewModel> SaveVacancy(Guid candidateId, int vacancyId);

        MediatorResponse<SavedVacancyViewModel> DeleteSavedVacancy(Guid candidateId, int vacancyId);
    }
}