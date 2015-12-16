namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public interface IApplicationProvider
    {
        VacancyApplicationsViewModel GetVacancyApplicationsViewModel(long vacancyReferenceNumber);
        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModel(Guid applicationId);
        ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModelForReview(Guid applicationId);
        void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes);
    }
}