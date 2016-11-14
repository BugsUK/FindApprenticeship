namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;

    public interface ITraineeshipApplicationMediator
    {
        MediatorResponse<TraineeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel);
        MediatorResponse<TraineeshipApplicationViewModel> ReviewSaveAndExit(TraineeshipApplicationViewModel traineeshipApplicationViewModel);
        MediatorResponse<TraineeshipApplicationViewModel> View(string application);
        MediatorResponse<TraineeshipApplicationViewModel> ReviewSetToSubmitted(TraineeshipApplicationViewModel traineeshipApplicationViewModel);
        MediatorResponse<TraineeshipApplicationViewModel> PromoteToInProgress(TraineeshipApplicationViewModel traineeshipApplicationViewModel);
    }
}
