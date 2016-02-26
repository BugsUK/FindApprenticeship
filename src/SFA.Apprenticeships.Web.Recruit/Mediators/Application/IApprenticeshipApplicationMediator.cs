namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public interface IApprenticeshipApplicationMediator
    {
        MediatorResponse<ApprenticeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewSaveAndExit(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmSuccessful(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApplicationSelectionViewModel> ConfirmSuccessfulAppointCandidate(ApplicationSelectionViewModel applicationSelectionViewModel);
    }
}
