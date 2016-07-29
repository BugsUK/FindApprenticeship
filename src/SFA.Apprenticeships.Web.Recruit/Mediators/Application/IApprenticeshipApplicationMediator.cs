namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Apprenticeship;

    public interface IApprenticeshipApplicationMediator
    {
        MediatorResponse<ApprenticeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewSaveAndExit(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApplicationSelectionViewModel> SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApplicationSelectionViewModel> SendUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> View(string application);
    }
}
