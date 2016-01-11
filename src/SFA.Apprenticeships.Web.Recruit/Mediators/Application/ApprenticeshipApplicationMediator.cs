namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Common.Constants;
    using Common.Mediators;
    using Constants.ViewModels;
    using Providers;
    using Validators;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

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
    }
}