using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System;
    using Common.ViewModels.Applications;
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

        MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyTrainingCourseRows(ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> PreviewAndSubmit(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationPreviewViewModel> Preview(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationPreviewViewModel> Submit(Guid candidateId, int vacancyId, ApprenticeshipApplicationPreviewViewModel viewModel);

        MediatorResponse<WhatHappensNextApprenticeshipViewModel> WhatHappensNext(Guid candidateId, string vacancyIdString, string vacancyReference, string vacancyTitle, string searchReturnUrl);

        MediatorResponse<ApprenticeshipApplicationViewModel> View(Guid candidateId, int vacancyId);

        MediatorResponse<SavedVacancyViewModel> SaveVacancy(Guid candidateId, int vacancyId);

        MediatorResponse<SavedVacancyViewModel> DeleteSavedVacancy(Guid candidateId, int vacancyId);
    }
}