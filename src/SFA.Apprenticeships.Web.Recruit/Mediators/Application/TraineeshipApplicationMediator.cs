namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Security;
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;
    using System;
    using System.Web;

    public class TraineeshipApplicationMediator : MediatorBase, ITraineeshipApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly TraineeshipApplicationViewModelServerValidator _traineeshipApplicationViewModelServerValidator;
        private readonly IDecryptionService<AnonymisedApplicationLink> _decryptionService;
        private readonly IDateTimeService _dateTimeService;

        public TraineeshipApplicationMediator(IApplicationProvider applicationProvider, TraineeshipApplicationViewModelServerValidator traineeshipApplicationViewModelServerValidator, IDecryptionService<AnonymisedApplicationLink> decryptionService, IDateTimeService dateTimeService)
        {
            _applicationProvider = applicationProvider;
            _traineeshipApplicationViewModelServerValidator = traineeshipApplicationViewModelServerValidator;
            _decryptionService = decryptionService;
            _dateTimeService = dateTimeService;
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetTraineeshipApplicationViewModel(applicationSelectionViewModel);

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Review.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> ReviewSaveAndExit(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var validationResult = _traineeshipApplicationViewModelServerValidator.Validate(traineeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation, traineeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateTraineeshipApplicationViewModelNotes(traineeshipApplicationViewModel.ApplicationSelection.ApplicationId, traineeshipApplicationViewModel.Notes, true);
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok, traineeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateTraineeshipApplicationViewModel(traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<TraineeshipApplicationViewModel> View(string applicationCipherText)
        {
            applicationCipherText = HttpUtility.UrlDecode(applicationCipherText);
            applicationCipherText = applicationCipherText?.Replace(' ', '+');

            var anomymisedApplicationLink = _decryptionService.Decrypt(applicationCipherText);

            var applicationSelectionViewModel = new ApplicationSelectionViewModel
            {
                ApplicationId = anomymisedApplicationLink.ApplicationId
            };

            var viewModel = _applicationProvider.GetTraineeshipApplicationViewModel(applicationSelectionViewModel);

            if (_dateTimeService.UtcNow > anomymisedApplicationLink.ExpirationDateTime)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.View.LinkExpired, viewModel);
            }

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.View.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> ReviewSetToSubmitted(
            TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var validationResult =
                _traineeshipApplicationViewModelServerValidator.Validate(traineeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(
                    ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation,
                    traineeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateTraineeshipApplicationViewModelNotes(
                    traineeshipApplicationViewModel.ApplicationSelection.ApplicationId,
                    traineeshipApplicationViewModel.Notes, false);
                _applicationProvider.SetStateSubmitted(traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok,
                    traineeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel =
                    GetFailedUpdateTraineeshipApplicationViewModel(
                        traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error, viewModel,
                    ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<TraineeshipApplicationViewModel> PromoteToInProgress(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var validationResult = _traineeshipApplicationViewModelServerValidator.Validate(traineeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.FailedValidation, traineeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateTraineeshipApplicationViewModelNotes(traineeshipApplicationViewModel.ApplicationSelection.ApplicationId, traineeshipApplicationViewModel.Notes, false);
                _applicationProvider.SetStateInProgress(traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Ok, traineeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateTraineeshipApplicationViewModel(traineeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
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
