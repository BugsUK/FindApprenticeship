using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    using System;
    using System.Linq;
    using Common.Configuration;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.Account;
    using ViewModels.MyApplications;

    public class AccountMediator : MediatorBase, IAccountMediator
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly IApprenticeshipVacancyProvider _apprenticeshipVacancyProvider;
        private readonly ITraineeshipVacancyProvider _traineeshipVacancyProvider;
        private readonly IConfigurationService _configurationService;
        private readonly IAccountProvider _accountProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator;
        private readonly VerifyMobileViewModelServerValidator _verifyMobileViewModelServerValidator;
        private readonly EmailViewModelServerValidator _emailViewModelServerValidator;
        private readonly VerifyUpdatedEmailViewModelServerValidator _verifyUpdatedEmailViewModelServerValidator;

        public AccountMediator(
            IAccountProvider accountProvider,
            ICandidateServiceProvider candidateServiceProvider,
            SettingsViewModelServerValidator settingsViewModelServerValidator,
            IApprenticeshipApplicationProvider apprenticeshipApplicationProvider,
            IApprenticeshipVacancyProvider apprenticeshipVacancyProvider,
            ITraineeshipVacancyProvider traineeshipVacancyProvider,
            IConfigurationService configurationService,
            VerifyMobileViewModelServerValidator mobileViewModelServerValidator,
            EmailViewModelServerValidator emailViewModelServerValidator,
            VerifyUpdatedEmailViewModelServerValidator verifyUpdatedEmailViewModelServerValidator
            )
        {
            _accountProvider = accountProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _settingsViewModelServerValidator = settingsViewModelServerValidator;
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            _apprenticeshipVacancyProvider = apprenticeshipVacancyProvider;
            _configurationService = configurationService;
            _traineeshipVacancyProvider = traineeshipVacancyProvider;
            _verifyMobileViewModelServerValidator = mobileViewModelServerValidator;
            _emailViewModelServerValidator = emailViewModelServerValidator;
            _verifyUpdatedEmailViewModelServerValidator = verifyUpdatedEmailViewModelServerValidator;
        }

        public MediatorResponse<MyApplicationsViewModel> Index(Guid candidateId, string deletedVacancyId, string deletedVacancyTitle)
        {
            var model = _apprenticeshipApplicationProvider.GetMyApplications(candidateId);
            model.DeletedVacancyId = deletedVacancyId;
            model.DeletedVacancyTitle = deletedVacancyTitle;
            return GetMediatorResponse(AccountMediatorCodes.Index.Success, model);
        }

        public MediatorResponse Archive(Guid candidateId, int vacancyId)
        {
            var applicationViewModel = _apprenticeshipApplicationProvider.ArchiveApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.Archive.ErrorArchiving, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(AccountMediatorCodes.Archive.SuccessfullyArchived, MyApplicationsPageMessages.ApplicationArchived, UserMessageLevel.Success);
        }

        public MediatorResponse Delete(Guid candidateId, int vacancyId)
        {
            var viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (viewModel.HasError())
            {
                if (viewModel.Status != ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return GetMediatorResponse(AccountMediatorCodes.Delete.AlreadyDeleted, MyApplicationsPageMessages.ApplicationDeleted, UserMessageLevel.Warning);
                }
            }

            var applicationViewModel = _apprenticeshipApplicationProvider.DeleteApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.Delete.ErrorDeleting, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (viewModel.VacancyDetail == null)
            {
                return GetMediatorResponse(AccountMediatorCodes.Delete.SuccessfullyDeletedExpiredOrWithdrawn, MyApplicationsPageMessages.ApplicationDeleted, UserMessageLevel.Success);
            }

            return GetMediatorResponse(AccountMediatorCodes.Delete.SuccessfullyDeleted, viewModel.VacancyDetail.Title, UserMessageLevel.Success);
        }

        public MediatorResponse DismissTraineeshipPrompts(Guid candidateId)
        {
            if (_accountProvider.DismissTraineeshipPrompts(candidateId))
            {
                return GetMediatorResponse(AccountMediatorCodes.DismissTraineeshipPrompts.SuccessfullyDismissed);
            }

            return GetMediatorResponse(AccountMediatorCodes.DismissTraineeshipPrompts.ErrorDismissing, MyApplicationsPageMessages.DismissTraineeshipPromptsFailed, UserMessageLevel.Error);
        }

        public MediatorResponse<SettingsViewModel> Settings(Guid candidateId, SettingsViewModel.SettingsMode mode)
        {
            var model = _accountProvider.GetSettingsViewModel(candidateId);
            model.Mode = mode;
            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
            model.TraineeshipFeature = traineeshipFeature;
            return GetMediatorResponse(AccountMediatorCodes.Settings.Success, model);
        }

        public MediatorResponse<SettingsViewModel> SaveSettings(Guid candidateId, SettingsViewModel settingsViewModel)
        {
            var validationResult = _settingsViewModelServerValidator.Validate(settingsViewModel);
            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);

            settingsViewModel.TraineeshipFeature = traineeshipFeature;

            if (!validationResult.IsValid)
            {
                var savedSearchesModel = _accountProvider.GetSettingsViewModel(candidateId);
                settingsViewModel.SavedSearches = savedSearchesModel.SavedSearches;
                return GetMediatorResponse(AccountMediatorCodes.Settings.ValidationError, settingsViewModel, validationResult);
            }

            Candidate candidate;
            var saved = _accountProvider.TrySaveSettings(candidateId, settingsViewModel, out candidate);

            if (!saved)
            {
                return GetMediatorResponse(AccountMediatorCodes.Settings.SaveError, settingsViewModel, AccountPageMessages.SettingsUpdateFailed, UserMessageLevel.Warning);
            }

            if (candidate.MobileVerificationRequired())
            {
                return GetMediatorResponse(AccountMediatorCodes.Settings.MobileVerificationRequired, settingsViewModel, AccountPageMessages.MobileVerificationRequired, UserMessageLevel.Success);
            }

            if (settingsViewModel.Mode == SettingsViewModel.SettingsMode.YourAccount)
            {
                var anyNotificationEnabled =
                    settingsViewModel.EnableApplicationStatusChangeAlertsViaEmail ||
                    settingsViewModel.EnableApplicationStatusChangeAlertsViaText ||
                    settingsViewModel.EnableExpiringApplicationAlertsViaEmail ||
                    settingsViewModel.EnableExpiringApplicationAlertsViaText ||
                    settingsViewModel.EnableMarketingViaEmail ||
                    settingsViewModel.EnableMarketingViaText;

                if (!anyNotificationEnabled)
                {
                    return GetMediatorResponse(AccountMediatorCodes.Settings.SuccessWithWarning, settingsViewModel, AccountPageMessages.SettingsUpdatedNotificationsAlertWarning, UserMessageLevel.Info);
                }
            }

            if (settingsViewModel.Mode == SettingsViewModel.SettingsMode.SavedSearches)
            {
                var anySavedSearchAlertsEnabled =
                    settingsViewModel.SavedSearches != null && settingsViewModel.SavedSearches.Any(s => s.AlertsEnabled);

                var shouldSendSavedSearchAlerts =
                    settingsViewModel.EnableSavedSearchAlertsViaEmail || settingsViewModel.EnableSavedSearchAlertsViaText;

                if ((shouldSendSavedSearchAlerts &&
                     (!anySavedSearchAlertsEnabled && settingsViewModel.SavedSearches != null)) ||
                    (anySavedSearchAlertsEnabled && !shouldSendSavedSearchAlerts))
                {
                    return GetMediatorResponse(AccountMediatorCodes.Settings.SuccessWithWarning, settingsViewModel,
                        AccountPageMessages.SettingsUpdatedSavedSearchesAlertWarning, UserMessageLevel.Info);
                }
            }

            return GetMediatorResponse(AccountMediatorCodes.Settings.Success, settingsViewModel);
        }

        public MediatorResponse Track(Guid candidateId, int vacancyId)
        {
            var applicationViewModel = _apprenticeshipApplicationProvider.UnarchiveApplication(candidateId, vacancyId);

            if (applicationViewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.Track.ErrorTracking, applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(AccountMediatorCodes.Track.SuccessfullyTracked);
        }

        public MediatorResponse AcceptTermsAndConditions(Guid candidateId)
        {
            try
            {
                var candidate = _candidateServiceProvider.GetCandidate(candidateId);
                var currentTsAndCsVersion = _configurationService.Get<CommonWebConfiguration>().TermsAndConditionsVersion;

                if (candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion == currentTsAndCsVersion)
                {
                    return GetMediatorResponse(AccountMediatorCodes.AcceptTermsAndConditions.AlreadyAccepted);
                }

                var success = _candidateServiceProvider.AcceptTermsAndConditions(candidateId, currentTsAndCsVersion);

                if (success)
                {
                    return GetMediatorResponse(AccountMediatorCodes.AcceptTermsAndConditions.SuccessfullyAccepted);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // returns ErrorAccepting
            }

            return GetMediatorResponse(AccountMediatorCodes.AcceptTermsAndConditions.ErrorAccepting);
        }

        public MediatorResponse ApprenticeshipVacancyDetails(Guid candidateId, int vacancyId)
        {
            var vacancyDetailViewModel = _apprenticeshipVacancyProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Unavailable, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Error, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Available);
        }

        public MediatorResponse TraineeshipVacancyDetails(Guid candidateId, int vacancyId)
        {
            var vacancyDetailViewModel = _traineeshipVacancyProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Unavailable, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Error, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(AccountMediatorCodes.VacancyDetails.Available);
        }


        public MediatorResponse<VerifyMobileViewModel> VerifyMobile(Guid candidateId, string returnUrl)
        {
            var verifyMobileViewModel = _accountProvider.GetVerifyMobileViewModel(candidateId);

             var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
             verifyMobileViewModel.TraineeshipFeature = traineeshipFeature;
             verifyMobileViewModel.ReturnUrl = returnUrl ?? string.Empty;

             switch (verifyMobileViewModel.Status)
            {
                case VerifyMobileState.Ok:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.Success, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationSuccessText, UserMessageLevel.Success);
                case VerifyMobileState.MobileVerificationNotRequired:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.VerificationNotRequired, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.Error, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error);
            }
        }

        
        public MediatorResponse<VerifyMobileViewModel> VerifyMobile(Guid candidateId, VerifyMobileViewModel verifyMobileViewModel)
        {
            var validationResult = _verifyMobileViewModelServerValidator.Validate(verifyMobileViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.ValidationError, verifyMobileViewModel, validationResult);
            }

            var verifyMobileViewModelResponse = _accountProvider.VerifyMobile(candidateId, verifyMobileViewModel);

            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
            verifyMobileViewModel.TraineeshipFeature = traineeshipFeature;

            switch (verifyMobileViewModelResponse.Status)
            {
                case VerifyMobileState.Ok:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.Success, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationSuccessText, UserMessageLevel.Success);
                case VerifyMobileState.VerifyMobileCodeInvalid:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.InvalidCode, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationCodeInvalid, UserMessageLevel.Error);
                case VerifyMobileState.MobileVerificationNotRequired:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.VerificationNotRequired, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(AccountMediatorCodes.VerifyMobile.Error, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<VerifyMobileViewModel> Resend(Guid candidateId, VerifyMobileViewModel verifyMobileViewModel)
        {
            verifyMobileViewModel = _accountProvider.SendMobileVerificationCode(candidateId, verifyMobileViewModel);

            var traineeshipFeature = _apprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(candidateId);
            verifyMobileViewModel.TraineeshipFeature = traineeshipFeature;

            switch (verifyMobileViewModel.Status)
            {
                case VerifyMobileState.Ok:
                    return GetMediatorResponse(AccountMediatorCodes.Resend.ResentSuccessfully, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationCodeMayHaveBeenResent, UserMessageLevel.Success);
                case VerifyMobileState.MobileVerificationNotRequired:
                    return GetMediatorResponse(AccountMediatorCodes.Resend.ResendNotRequired, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(AccountMediatorCodes.Resend.Error, verifyMobileViewModel, VerifyMobilePageMessages.MobileVerificationCodeResendFailed, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<SavedSearchViewModel> DeleteSavedSearch(Guid candidateId, Guid savedSearchId)
        {
            var viewModel = _candidateServiceProvider.DeleteSavedSearch(candidateId, savedSearchId);

            if (viewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.DeleteSavedSearch.HasError, viewModel, AccountPageMessages.DeleteSavedSearchFailed, UserMessageLevel.Error);
            }

            return GetMediatorResponse(AccountMediatorCodes.DeleteSavedSearch.Ok, viewModel);
        }


        public MediatorResponse<EmailViewModel> UpdateEmailAddress(Guid userId, EmailViewModel emailViewModel)
        {
            var validationResult = _emailViewModelServerValidator.Validate(emailViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(AccountMediatorCodes.UpdateEmailAddress.ValidationError, emailViewModel, validationResult);
            }

            var viewModel = _accountProvider.UpdateEmailAddress(userId, emailViewModel);

            if (viewModel.HasError())
            {
                if (viewModel.UpdateStatus == UpdateEmailStatus.AccountAlreadyExists)
                {
                    return GetMediatorResponse(AccountMediatorCodes.UpdateEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.UpdateEmailAddressAlreadyInUse, UserMessageLevel.Error);
                }
                return GetMediatorResponse(AccountMediatorCodes.UpdateEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.UpdateEmailAddressError, UserMessageLevel.Error);
            }

            return GetMediatorResponse(AccountMediatorCodes.UpdateEmailAddress.Ok, viewModel, UpdateEmailAddressMessages.VerificationCodeSent, UserMessageLevel.Success);
        }

        public MediatorResponse<VerifyUpdatedEmailViewModel> VerifyUpdatedEmailAddress(Guid userId, VerifyUpdatedEmailViewModel model)
        {
            var validationResult = _verifyUpdatedEmailViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.ValidationError, model, validationResult);
            }

            var viewModel = _accountProvider.VerifyUpdatedEmailAddress(userId, model);

            if (viewModel.HasError())
            {
                switch (viewModel.UpdateStatus)
                {
                    case UpdateEmailStatus.AccountAlreadyExists:
                        return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.UpdateEmailAddressAlreadyInUse, UserMessageLevel.Error);
                    case UpdateEmailStatus.InvalidUpdateUsernameCode:
                        return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.IncorrectVerificationCode, UserMessageLevel.Error);
                    case UpdateEmailStatus.UserPasswordError:
                        return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.IncorrectPassword, UserMessageLevel.Error);
                    default:
                        return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError, viewModel, UpdateEmailAddressMessages.UpdateEmailAddressError, UserMessageLevel.Error);
                }
            }

            return GetMediatorResponse(AccountMediatorCodes.VerifyUpdatedEmailAddress.Ok, viewModel, UpdateEmailAddressMessages.UpdatedEmailSuccess, UserMessageLevel.Success);
        }

        public MediatorResponse<VerifyUpdatedEmailViewModel> ResendUpdateEmailAddressCode(Guid userId)
        {
            var viewModel = _accountProvider.ResendUpdateEmailAddressCode(userId);

            if (viewModel.HasError())
            {
                return GetMediatorResponse(AccountMediatorCodes.ResendUpdateEmailAddressCode.HasError, viewModel, UpdateEmailAddressMessages.UpdateEmailAddressError, UserMessageLevel.Error);
            }

            return GetMediatorResponse(AccountMediatorCodes.ResendUpdateEmailAddressCode.Ok, viewModel, UpdateEmailAddressMessages.UpdateEmailCodeResent, UserMessageLevel.Success);
        }
    }
}