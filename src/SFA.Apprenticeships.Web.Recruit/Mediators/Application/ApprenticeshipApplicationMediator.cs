namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Security;
    using Common.Constants;
    using Common.Mediators;
    using Constants.Messages;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using System;
    using System.Web;

    public class ApprenticeshipApplicationMediator : MediatorBase, IApprenticeshipApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;

        private readonly ApprenticeshipApplicationViewModelServerValidator _apprenticeshipApplicationViewModelServerValidator;
        private readonly IDecryptionService<AnonymisedApplicationLink> _decryptionService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogService _logService;

        public ApprenticeshipApplicationMediator(IApplicationProvider applicationProvider,
            ApprenticeshipApplicationViewModelServerValidator apprenticeshipApplicationViewModelServerValidator,
            IDecryptionService<AnonymisedApplicationLink> decryptionService, IDateTimeService dateTimeService,
            ILogService logService)
        {
            _applicationProvider = applicationProvider;
            _apprenticeshipApplicationViewModelServerValidator = apprenticeshipApplicationViewModelServerValidator;
            _decryptionService = decryptionService;
            _dateTimeService = dateTimeService;
            _logService = logService;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            if (applicationSelectionViewModel.ApplicationId == Guid.Empty)
            {
                _logService.Info("Review vacancy failed: VacancyGuid is empty.");
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Review.NoApplicationId, new ApprenticeshipApplicationViewModel(), ApplicationPageMessages.ApplicationNotFound, UserMessageLevel.Info);
            }

            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Review.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> View(string applicationCipherText)
        {
            applicationCipherText = HttpUtility.UrlDecode(applicationCipherText);
            applicationCipherText = applicationCipherText?.Replace(' ', '+');

            var anomymisedApplicationLink = _decryptionService.Decrypt(applicationCipherText);

            var applicationSelectionViewModel = new ApplicationSelectionViewModel
            {
                ApplicationId = anomymisedApplicationLink.ApplicationId
            };

            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);

            if (_dateTimeService.UtcNow > anomymisedApplicationLink.ExpirationDateTime)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.View.LinkExpired, viewModel);
            }

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.View.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes, true);

                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes, true);

                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ReviewRevertToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes, true);

                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ReviewSetToSubmitted(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes, false);
                _applicationProvider.SetStateSubmitted(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> PromoteToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes, false);
                _applicationProvider.SetStateInProgress(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        private ApprenticeshipApplicationViewModel GetFailedUpdateApprenticeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);
            viewModel.Notes = viewModel.Notes;
            return viewModel;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            if (applicationSelectionViewModel.ApplicationId == Guid.Empty)
            {
                _logService.Error("Confirm successful decision failed: VacancyGuid is empty.");
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.NoApplicationId, new ApprenticeshipApplicationViewModel(), ApplicationPageMessages.ApplicationNotFound, UserMessageLevel.Info);
            }

            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.SendSuccessfulDecision(applicationSelectionViewModel);
            var applicationViewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(viewModel);
            applicationViewModel.ConfirmationStatusSentMessage = ApplicationViewModelMessages.SuccessfulDecisionFormat;
            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.SendSuccessfulDecision.Ok, applicationViewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            if (applicationSelectionViewModel.ApplicationId == Guid.Empty)
            {
                _logService.Error("Confirm unsuccessful decision failed: VacancyGuid is empty.");
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.NoApplicationId, new ApprenticeshipApplicationViewModel(), ApplicationPageMessages.ApplicationNotFound, UserMessageLevel.Info);
            }

            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> SendUnsuccessfulDecision(ApprenticeshipApplicationViewModel applicationViewModel)
        {
            var selectionViewModel = _applicationProvider.SendUnsuccessfulDecision(applicationViewModel.ApplicationSelection, applicationViewModel.UnSuccessfulReason);
            var updatedApplicationViewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(selectionViewModel);
            updatedApplicationViewModel.ConfirmationStatusSentMessage =
                ApplicationViewModelMessages.UnsuccessfulDecisionFormat;
            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok, updatedApplicationViewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmRevertToInProgress(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            if (applicationSelectionViewModel.ApplicationId == Guid.Empty)
            {
                _logService.Error("Confirm revert to viewed failed: VacancyGuid is empty.");
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.NoApplicationId, new ApprenticeshipApplicationViewModel(), ApplicationPageMessages.ApplicationNotFound, UserMessageLevel.Info);
            }

            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.Ok, viewModel);
        }

        public MediatorResponse<ApplicationSelectionViewModel> RevertToInProgress(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationViewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);
            var viewModel = _applicationProvider.SetStateInProgress(applicationSelectionViewModel);

            var candidateName = applicationViewModel.ApplicantDetails.Name;
            var message = string.Format(ApplicationViewModelMessages.RevertToInProgressFormat, candidateName);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.RevertToInProgress.Ok, viewModel, message, UserMessageLevel.Info);
        }
    }
}
