namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Users;
    using Common.Configuration;
    using Common.ViewModels.Locations;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;

    using ViewModels.Account;
    using ErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class AccountProvider : IAccountProvider
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly ICandidateService _candidateService;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public AccountProvider(
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IMapper mapper,
            ILogService logger,
            IConfigurationService configurationService)
        {
            _mapper = mapper;
            _logger = logger;
            _configurationService = configurationService;
            _candidateService = candidateService;
            _userAccountService = userAccountService;
        }

        public SettingsViewModel GetSettingsViewModel(Guid candidateId)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to get Settings View Model for candidate with Id={0}", candidateId);

                var candidate = _candidateService.GetCandidate(candidateId);
                var user = _userAccountService.GetUser(candidateId);
                var savedSearches = _candidateService.GetSavedSearches(candidateId);
                var settings = _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);

                settings.Username = user.Username;
                settings.PendingUsername = user.PendingUsername;

                settings.VerifiedMobile = candidate.CommunicationPreferences.VerifiedMobile;
                settings.SmsEnabled = _configurationService.Get<CommonWebConfiguration>().Features.SmsEnabled;

                // Communication preferences.
                var communicationPreferences = candidate.CommunicationPreferences;

                settings.EnableApplicationStatusChangeAlertsViaEmail = communicationPreferences.ApplicationStatusChangePreferences.EnableEmail;
                settings.EnableApplicationStatusChangeAlertsViaText = communicationPreferences.ApplicationStatusChangePreferences.EnableText;

                settings.EnableExpiringApplicationAlertsViaEmail = communicationPreferences.ExpiringApplicationPreferences.EnableEmail;
                settings.EnableExpiringApplicationAlertsViaText = communicationPreferences.ExpiringApplicationPreferences.EnableText;

                settings.EnableMarketingViaEmail = communicationPreferences.MarketingPreferences.EnableEmail;
                settings.EnableMarketingViaText = communicationPreferences.MarketingPreferences.EnableText;

                settings.EnableSavedSearchAlertsViaEmail = communicationPreferences.SavedSearchPreferences.EnableEmail;
                settings.EnableSavedSearchAlertsViaText = communicationPreferences.SavedSearchPreferences.EnableText;

                // Monitoring information.
                settings.MonitoringInformation.Gender = candidate.MonitoringInformation.Gender.HasValue
                    ? (int)candidate.MonitoringInformation.Gender
                    : default(int?);

                settings.MonitoringInformation.DisabilityStatus = candidate.MonitoringInformation.DisabilityStatus.HasValue
                    ? (int)candidate.MonitoringInformation.DisabilityStatus
                    : default(int?);

                settings.MonitoringInformation.Ethnicity = candidate.MonitoringInformation.Ethnicity.HasValue
                    ? candidate.MonitoringInformation.Ethnicity
                    : default(int?);

                settings.MonitoringInformation.AnythingWeCanDoToSupportYourInterview = candidate.ApplicationTemplate.AboutYou.Support;
                settings.MonitoringInformation.RequiresSupportForInterview = !string.IsNullOrWhiteSpace(candidate.ApplicationTemplate.AboutYou.Support);
                
                // Saved searches.
                var savedSeachViewModels = savedSearches == null ? new List<SavedSearchViewModel>() : savedSearches.Select(s => s.ToViewModel(_configurationService.Get<CommonWebConfiguration>().SubCategoriesFullNamesLimit)).ToList();

                settings.SavedSearches = savedSeachViewModels;

                return settings;
            }
            catch (Exception e)
            {
                var message = string.Format(
                    "Unexpected error while getting settings view model on AccountProvider for candidate with Id={0}.",
                    candidateId);

                _logger.Error(message, e);
                throw;
            }
        }

        public bool TrySaveSettings(Guid candidateId, SettingsViewModel model, out Candidate candidate)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to save the settings for candidate with Id={0}", candidateId);

                candidate = _candidateService.GetCandidate(candidateId);

                if (candidate.RegistrationDetails.PhoneNumber != model.PhoneNumber)
                {
                    candidate.CommunicationPreferences.VerifiedMobile = false;
                }

                // Communication preferences.
                var communicationPreferences = candidate.CommunicationPreferences;

                communicationPreferences.ApplicationStatusChangePreferences.EnableEmail = model.EnableApplicationStatusChangeAlertsViaEmail;
                communicationPreferences.ApplicationStatusChangePreferences.EnableText = model.EnableApplicationStatusChangeAlertsViaText;

                communicationPreferences.ExpiringApplicationPreferences.EnableEmail = model.EnableExpiringApplicationAlertsViaEmail;
                communicationPreferences.ExpiringApplicationPreferences.EnableText = model.EnableExpiringApplicationAlertsViaText;

                communicationPreferences.MarketingPreferences.EnableEmail = model.EnableMarketingViaEmail;
                communicationPreferences.MarketingPreferences.EnableText = model.EnableMarketingViaText;

                communicationPreferences.SavedSearchPreferences.EnableEmail = model.EnableSavedSearchAlertsViaEmail;
                communicationPreferences.SavedSearchPreferences.EnableText = model.EnableSavedSearchAlertsViaText;

                // Monitoring information.
                var monitoringInformation = candidate.MonitoringInformation;
                var aboutYou = candidate.ApplicationTemplate.AboutYou;

                monitoringInformation.Gender = model.MonitoringInformation.Gender.HasValue
                    ? (Gender)model.MonitoringInformation.Gender.Value
                    : default(Gender?);

                monitoringInformation.DisabilityStatus = (DisabilityStatus?)model.MonitoringInformation.DisabilityStatus;

                monitoringInformation.Ethnicity = model.MonitoringInformation.Ethnicity == 0
                    ? default(int?)
                    : model.MonitoringInformation.Ethnicity;

                if (model.IsJavascript && !model.MonitoringInformation.RequiresSupportForInterview)
                {
                    model.MonitoringInformation.AnythingWeCanDoToSupportYourInterview = null;
                }

                aboutYou.Support = model.MonitoringInformation.AnythingWeCanDoToSupportYourInterview;

                PatchRegistrationDetails(candidate.RegistrationDetails, model);

                _candidateService.SaveCandidate(candidate);
                if (candidate.MobileVerificationRequired())
                {
                    _candidateService.SendMobileVerificationCode(candidate);
                }

                if (model.SavedSearches != null && model.SavedSearches.Count > 0)
                {
                    var savedSearches = _candidateService.GetSavedSearches(candidateId);

                    foreach (var updatedSavedSearch in model.SavedSearches)
                    {
                        var savedSearch = savedSearches.SingleOrDefault(s => s.EntityId == updatedSavedSearch.Id);

                        if (savedSearch != null && savedSearch.AlertsEnabled != updatedSavedSearch.AlertsEnabled)
                        {
                            savedSearch.AlertsEnabled = updatedSavedSearch.AlertsEnabled;
                            _candidateService.UpdateSavedSearch(savedSearch);
                        }
                    }
                }

                _logger.Debug("Settings saved for candidate with Id={0}", candidateId);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Save settings failed for candidate " + candidateId, e);
                candidate = null;
                return false;
            }
        }

        public bool DismissTraineeshipPrompts(Guid candidateId)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to dismiss traineeship prompts for candidate with Id={0}", candidateId);

                var candidate = _candidateService.GetCandidate(candidateId);

                candidate.CommunicationPreferences.AllowTraineeshipPrompts = false;
                _candidateService.SaveCandidate(candidate);

                _logger.Debug("Settings saved for candidate with Id={0}", candidateId);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Dismiss traineeship prompts failed for candidate " + candidateId, e);
                return false;
            }
        }

        private void PatchRegistrationDetails(RegistrationDetails registrationDetails, SettingsViewModel model)
        {
            registrationDetails.FirstName = model.Firstname;
            registrationDetails.LastName = model.Lastname;
            registrationDetails.DateOfBirth = new DateTime(
                // ReSharper disable PossibleInvalidOperationException
                model.DateOfBirth.Year.Value, model.DateOfBirth.Month.Value, model.DateOfBirth.Day.Value);
            // ReSharper restore PossibleInvalidOperationException
            registrationDetails.Address = _mapper.Map<AddressViewModel, Address>(model.Address);
            registrationDetails.PhoneNumber = model.PhoneNumber;
        }

        public VerifyMobileViewModel GetVerifyMobileViewModel(Guid candidateId)
        {
            _logger.Debug("Calling CandidateService to fetch candidateId {0} details", candidateId);

            var model = new VerifyMobileViewModel();

            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                model.PhoneNumber = candidate.RegistrationDetails.PhoneNumber;
                if (!candidate.MobileVerificationRequired())
                {
                    model.Status = VerifyMobileState.MobileVerificationNotRequired;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Mobile code verification failed for candidateId {0} and Lme {1}", candidateId, model.PhoneNumber, e);
                model.ViewModelMessage = e.Message;
                model.Status = VerifyMobileState.Error;
            }
            return model;
        }

        public VerifyMobileViewModel VerifyMobile(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to verify mobile code candidateId {0} and mobile number {1}", candidateId, model.PhoneNumber);

            try
            {
                _candidateService.VerifyMobileCode(candidateId, model.VerifyMobileCode);
                model.Status = VerifyMobileState.Ok;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Info(e.Message, e);
                        model.Status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    case ErrorCodes.MobileCodeVerificationFailed:
                        _logger.Info(e.Message, e);

                        model.Status = VerifyMobileState.VerifyMobileCodeInvalid;
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        model.Status = VerifyMobileState.Error;
                        break;
                }
                model.ViewModelMessage = e.Message;
            }
            catch (Exception e)
            {
                _logger.Error("Mobile code verification failed for candidateId {0} and Lme {1}", candidateId, model.PhoneNumber, e);
                model.Status = VerifyMobileState.Error;
                model.ViewModelMessage = e.Message;
            }
            return model;
        }

        public VerifyMobileViewModel SendMobileVerificationCode(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to send mobile verification code for candidateId {0} to mobile number {1}", candidateId, model.PhoneNumber);

            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                _candidateService.SendMobileVerificationCode(candidate);
                model.Status = VerifyMobileState.Ok;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        model.Status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    default:
                        model.Status = VerifyMobileState.Error;
                        _logger.Error(e.Message, e);
                        break;
                }
                model.ViewModelMessage = e.Message;
            }
            catch (Exception e)
            {
                var message = string.Format(
                    "Sending Mobile verification code to mobile number {1} failed for candidateId {0} ",
                    candidateId, model.PhoneNumber);

                _logger.Error(message, e);

                model.Status = VerifyMobileState.Error;
                model.ViewModelMessage = e.Message;
            }
            return model;
        }

        public EmailViewModel UpdateEmailAddress(Guid userId, EmailViewModel emailViewModel)
        {
            _logger.Debug("Calling AccountProvider to update username for Id: {0} to new email address: {1}", userId, emailViewModel.EmailAddress);

            try
            {
                _userAccountService.UpdateUsername(userId, emailViewModel.EmailAddress);
                emailViewModel.UpdateStatus = UpdateEmailStatus.Ok;
            }
            catch (CustomException ex)
            {
                switch (ex.Code)
                {
                    case ErrorCodes.UserDirectoryAccountExistsError:
                        emailViewModel.UpdateStatus = UpdateEmailStatus.AccountAlreadyExists;
                        emailViewModel.ViewModelMessage = UpdateEmailStatus.AccountAlreadyExists.ToString();
                        break;
                    default:
                        _logger.Error("Unknown CustomException", ex);
                        emailViewModel.UpdateStatus = UpdateEmailStatus.Error;
                        emailViewModel.ViewModelMessage = UpdateEmailStatus.Error.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating username", ex);
                emailViewModel.UpdateStatus = UpdateEmailStatus.Error;
                emailViewModel.ViewModelMessage = UpdateEmailStatus.Error.ToString();
            }

            return emailViewModel;
        }

        public VerifyUpdatedEmailViewModel VerifyUpdatedEmailAddress(Guid userId, VerifyUpdatedEmailViewModel model)
        {
            _logger.Debug("Calling AccountProvider to verify code: {0} and password: {1} for the userId: {2} to update their username", model.PendingUsernameCode, model.VerifyPassword, userId);

            try
            {
                _candidateService.UpdateUsername(userId, model.PendingUsernameCode, model.VerifyPassword);
                model.UpdateStatus = UpdateEmailStatus.Updated;
            }
            catch (CustomException ex)
            {
                switch (ex.Code)
                {
                    case ErrorCodes.UserDirectoryAccountExistsError:
                        model.UpdateStatus = UpdateEmailStatus.AccountAlreadyExists;
                        model.ViewModelMessage = UpdateEmailStatus.AccountAlreadyExists.ToString();
                        break;
                    case ErrorCodes.InvalidUpdateUsernameCode:
                        model.UpdateStatus = UpdateEmailStatus.InvalidUpdateUsernameCode;
                        model.ViewModelMessage = UpdateEmailStatus.InvalidUpdateUsernameCode.ToString();
                        break;
                    case ErrorCodes.UserPasswordError:
                        model.UpdateStatus = UpdateEmailStatus.UserPasswordError;
                        model.ViewModelMessage = UpdateEmailStatus.UserPasswordError.ToString();
                        break;
                    default:
                        _logger.Error("Unexpected custom error from candidateService.UpdateUsername", ex);
                        model.UpdateStatus = UpdateEmailStatus.Error;
                        model.ViewModelMessage = UpdateEmailStatus.Error.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error verifying username update and password codes", ex);
                model.UpdateStatus = UpdateEmailStatus.Error;
                model.ViewModelMessage = UpdateEmailStatus.Error.ToString();
            }

            return model;
        }

        public VerifyUpdatedEmailViewModel ResendUpdateEmailAddressCode(Guid userId)
        {
            _logger.Debug("Calling AccountProvider to resent update email address code for the userId: {0}", userId);
            var model = new VerifyUpdatedEmailViewModel();

            try
            {
                _userAccountService.ResendUpdateUsernameCode(userId);
                model.UpdateStatus = UpdateEmailStatus.Ok;
            }
            catch (Exception ex)
            {
                _logger.Error("Error resending username code", ex);
                model.UpdateStatus = UpdateEmailStatus.Error;
                model.ViewModelMessage = UpdateEmailStatus.Error.ToString();
            }

            return model;
        }
    }
}
