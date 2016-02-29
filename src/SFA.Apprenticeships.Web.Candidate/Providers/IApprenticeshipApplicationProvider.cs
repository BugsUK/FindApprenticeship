namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipApplicationProvider
    {
        ApprenticeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationPreviewViewModel GetApplicationPreviewViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel CreateApplicationViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApprenticeshipApplicationViewModel savedModel, ApprenticeshipApplicationViewModel submittedModel);

        MyApplicationsViewModel GetMyApplications(Guid candidateId);

        void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);

        ApprenticeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextApprenticeshipViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId, string searchReturnUrl);

        ApprenticeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel UnarchiveApplication(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId);

        TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId);

        SavedVacancyViewModel SaveVacancy(Guid candidateId, int vacancyId);

        SavedVacancyViewModel DeleteSavedVacancy(Guid candidateId, int vacancyId);
    }
}
