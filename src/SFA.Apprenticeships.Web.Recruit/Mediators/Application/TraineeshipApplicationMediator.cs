namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Apprenticeships.Application.Interfaces.Security;
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;
    using SFA.Infrastructure.Interfaces;

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

        public MediatorResponse<TraineeshipApplicationViewModel> View(string applicationCipherText)
        {
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

        private TraineeshipApplicationViewModel GetFailedUpdateTraineeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetTraineeshipApplicationViewModel(applicationSelectionViewModel);
            viewModel.Notes = viewModel.Notes;
            return viewModel;
        }
    }
}
