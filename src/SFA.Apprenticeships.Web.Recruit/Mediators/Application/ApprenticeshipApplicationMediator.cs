namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Common.Constants;
    using Common.Mediators;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Apprenticeship;

    public class ApprenticeshipApplicationMediator : MediatorBase, IApprenticeshipApplicationMediator
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ApprenticeshipApplicationViewModelServerValidator _apprenticeshipApplicationViewModelServerValidator;

        public ApprenticeshipApplicationMediator(IApplicationProvider applicationProvider, ApprenticeshipApplicationViewModelServerValidator apprenticeshipApplicationViewModelServerValidator)
        {
            _applicationProvider = applicationProvider;
            _apprenticeshipApplicationViewModelServerValidator = apprenticeshipApplicationViewModelServerValidator;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModelForReview(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Review.Ok, viewModel);
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
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes);

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
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes);

                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ReviewSaveAndExit(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var validationResult = _apprenticeshipApplicationViewModelServerValidator.Validate(apprenticeshipApplicationViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation, apprenticeshipApplicationViewModel, validationResult);
            }

            try
            {
                _applicationProvider.UpdateApprenticeshipApplicationViewModelNotes(apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId, apprenticeshipApplicationViewModel.Notes);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok, apprenticeshipApplicationViewModel);
            }
            catch (Exception)
            {
                var viewModel = GetFailedUpdateApprenticeshipApplicationViewModel(apprenticeshipApplicationViewModel.ApplicationSelection);
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Error, viewModel, ApplicationViewModelMessages.UpdateNotesFailed, UserMessageLevel.Error);
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
            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModelForReview(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.Ok, viewModel);
        }

        public MediatorResponse<ApplicationSelectionViewModel> SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationViewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);
            var viewModel = _applicationProvider.SendSuccessfulDecision(applicationSelectionViewModel);

            var candidateName = applicationViewModel.ApplicantDetails.Name;
            var message = string.Format(ApplicationViewModelMessages.SuccessfulDecisionFormat, candidateName);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.SendSuccessfulDecision.Ok, viewModel, message, UserMessageLevel.Info);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> ConfirmUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var viewModel = _applicationProvider.GetApprenticeshipApplicationViewModelForReview(applicationSelectionViewModel);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok, viewModel);
        }

        public MediatorResponse<ApplicationSelectionViewModel> SendUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationViewModel = _applicationProvider.GetApprenticeshipApplicationViewModel(applicationSelectionViewModel);
            var viewModel = _applicationProvider.SendUnsuccessfulDecision(applicationSelectionViewModel);

            var candidateName = applicationViewModel.ApplicantDetails.Name;
            var message = string.Format(ApplicationViewModelMessages.UnsuccessfulDecisionFormat, candidateName);

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok, viewModel, message, UserMessageLevel.Info);
        }
    }
}
