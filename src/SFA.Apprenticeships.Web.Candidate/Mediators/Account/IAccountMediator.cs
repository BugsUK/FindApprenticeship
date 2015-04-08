﻿namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    using System;
    using ViewModels.Account;
    using ViewModels.MyApplications;

    public interface IAccountMediator
    {
        MediatorResponse<MyApplicationsViewModel> Index(Guid candidateId, string deletedVacancyId, string deletedVacancyTitle);

        MediatorResponse Archive(Guid candidateId, int vacancyId);

        MediatorResponse Delete(Guid candidateId, int vacancyId);

        MediatorResponse DismissTraineeshipPrompts(Guid candidateId);

        MediatorResponse<SettingsViewModel> Settings(Guid candidateId, SettingsViewModel.SettingsMode mode);

        MediatorResponse<SettingsViewModel> SaveSettings(Guid candidateId, SettingsViewModel settingsViewModel);

        MediatorResponse Track(Guid candidateId, int vacancyId);

        MediatorResponse AcceptTermsAndConditions(Guid candidateId);

        MediatorResponse ApprenticeshipVacancyDetails(Guid candidateId, int vacancyId);

        MediatorResponse TraineeshipVacancyDetails(Guid candidateId, int vacancyId);

        MediatorResponse<VerifyMobileViewModel> VerifyMobile(Guid candidateId, string returnUrl);

        MediatorResponse<VerifyMobileViewModel> VerifyMobile(Guid candidateId, VerifyMobileViewModel verifyMobileViewModel);

        MediatorResponse<VerifyMobileViewModel> Resend(Guid candidateId, VerifyMobileViewModel model);

        MediatorResponse<SavedSearchViewModel> DeleteSavedSearch(Guid candidateId, Guid savedSearchId);

        MediatorResponse<VertifyUpdatedEmailViewModel> VerifyUpdatedEmailAddress(Guid userId, VertifyUpdatedEmailViewModel model);

        MediatorResponse<VertifyUpdatedEmailViewModel> UpdateEmailAddress(Guid userId, string updatedEmailAddress);

        MediatorResponse<VertifyUpdatedEmailViewModel> ResendUpdateEmailAddressCode(Guid userId);
    }
}