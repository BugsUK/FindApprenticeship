namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public interface IApplicationProvider
    {
        VacancyApplicationsViewModel GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch);

        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel);

        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModelForReview(ApplicationSelectionViewModel applicationSelectionViewModel);

        void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes);
    }
}
