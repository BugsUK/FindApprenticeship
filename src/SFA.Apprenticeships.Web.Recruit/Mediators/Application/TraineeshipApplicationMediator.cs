namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;

    public class TraineeshipApplicationMediator : MediatorBase, ITraineeshipApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly TraineeshipApplicationViewModelServerValidator _traineeshipApplicationViewModelServerValidator;

        public TraineeshipApplicationMediator(IApplicationProvider applicationProvider, TraineeshipApplicationViewModelServerValidator traineeshipApplicationViewModelServerValidator)
        {
            _applicationProvider = applicationProvider;
            _traineeshipApplicationViewModelServerValidator = traineeshipApplicationViewModelServerValidator;
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetTraineeshipApplicationViewModelForReview(applicationSelectionViewModel);

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Review.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> ReviewSaveAndExit(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var validationResult = _traineeshipApplicationViewModelServerValidator.Validate(traineeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation, traineeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateTraineeshipApplicationViewModelNotes(traineeshipApplicationViewModel.ApplicationSelection.ApplicationId, traineeshipApplicationViewModel.Notes);
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok, traineeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateTraineeshipApplicationViewModel(traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        private TraineeshipApplicationViewModel GetFailedUpdateTraineeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetTraineeshipApplicationViewModel(applicationSelectionViewModel);
            viewModel.Notes = viewModel.Notes;
            return viewModel;
        }
    }
}
