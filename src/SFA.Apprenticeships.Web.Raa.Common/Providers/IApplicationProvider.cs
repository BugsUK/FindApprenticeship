namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Application.Traineeship;

    public interface IApplicationProvider
    {
        VacancyApplicationsViewModel GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch);

        ShareApplicationsViewModel GetShareApplicationsViewModel(int vacancyReferenceNumber);

        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel);

        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModelForReview(ApplicationSelectionViewModel applicationSelectionViewModel);

        void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes);

        ApplicationSelectionViewModel SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);

        ApplicationSelectionViewModel SendUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel, string candidateApplicationFeedback);

        ApplicationSelectionViewModel SetStateInProgress(ApplicationSelectionViewModel applicationSelectionViewModel);

        TraineeshipApplicationViewModel GetTraineeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel);

        TraineeshipApplicationViewModel GetTraineeshipApplicationViewModelForReview(ApplicationSelectionViewModel applicationSelectionViewModel);

        void UpdateTraineeshipApplicationViewModelNotes(Guid applicationId, string notes);

        void ShareApplications(int vacancyReferenceNumber, string providerName, IDictionary<string, string> applicationLinks, DateTime linkExpiryDateTime, string recipientEmailAddress);
        ApplicationSelectionViewModel SetStateSubmitted(ApplicationSelectionViewModel applicationSelection);
    }
}
