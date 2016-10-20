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
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewRevertToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ReviewSetToSubmitted(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> PromoteToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> SendUnsuccessfulDecision(ApprenticeshipApplicationViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmRevertToInProgress(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApplicationSelectionViewModel> RevertToInProgress(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<ApprenticeshipApplicationViewModel> View(string application);
        MediatorResponse<BulkApplicationsRejectViewModel> GetApprenticeshipApplicationViewModel(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel);
        MediatorResponse<BulkApplicationsRejectViewModel> SendBulkUnsuccessfulDecision(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel);
    }
}
