namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Users;
    using Common.Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using ViewModels.Account;
    using ViewModels.Locations;
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
                var savedSearches = _candidateService.RetrieveSavedSearches(candidateId);
                var settings = _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);

                settings.Username = user.Username;
                settings.PendingUsername = user.PendingUsername;

                settings.AllowEmailComms = candidate.CommunicationPreferences.AllowEmail;
                settings.AllowSmsComms = candidate.CommunicationPreferences.AllowMobile;
                settings.VerifiedMobile = candidate.CommunicationPreferences.VerifiedMobile;
                settings.SmsEnabled = _configurationService.Get<WebConfiguration>().Features.SmsEnabled;

                settings.SendApplicationStatusChanges = candidate.CommunicationPreferences.SendApplicationStatusChanges;
                settings.SendApprenticeshipApplicationsExpiring = candidate.CommunicationPreferences.SendApprenticeshipApplicationsExpiring;
                settings.SendMarketingCommunications = candidate.CommunicationPreferences.SendMarketingCommunications;

                settings.SendSavedSearchAlertsViaEmail = candidate.CommunicationPreferences.SendSavedSearchAlertsViaEmail;
                settings.SendSavedSearchAlertsViaText = candidate.CommunicationPreferences.SendSavedSearchAlertsViaText;

                var savedSeachViewModels = savedSearches == null ? new List<SavedSearchViewModel>() : savedSearches.Select(s => s.ToViewModel()).ToList();

                settings.SavedSearches = savedSeachViewModels;

                return settings;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
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

                candidate.CommunicationPreferences.AllowEmail = model.AllowEmailComms;
                candidate.CommunicationPreferences.AllowMobile = model.AllowSmsComms;
                
                if (candidate.RegistrationDetails.PhoneNumber != model.PhoneNumber)
                {
                    candidate.CommunicationPreferences.VerifiedMobile = false;
                }

                candidate.CommunicationPreferences.SendApplicationStatusChanges = model.SendApplicationStatusChanges;
                candidate.CommunicationPreferences.SendApprenticeshipApplicationsExpiring = model.SendApprenticeshipApplicationsExpiring;
                candidate.CommunicationPreferences.SendMarketingCommunications = model.SendMarketingCommunications;

                candidate.CommunicationPreferences.SendSavedSearchAlertsViaEmail = model.SendSavedSearchAlertsViaEmail;
                candidate.CommunicationPreferences.SendSavedSearchAlertsViaText = model.SendSavedSearchAlertsViaText;

                PatchRegistrationDetails(candidate.RegistrationDetails, model);
                _candidateService.SaveCandidate(candidate);

                if (model.SavedSearches != null && model.SavedSearches.Count > 0)
                {
                    var savedSearches = _candidateService.RetrieveSavedSearches(candidateId);
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

            VerifyMobileViewModel model = new VerifyMobileViewModel();
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
            _logger.Debug("Calling AccountProvider to verify mobile code candidateId {0} and mobile number {1}",
                                                                                    candidateId, model.PhoneNumber);
            try
            {
                _candidateService.VerifyMobileCode(candidateId, model.VerifyMobileCode);
                model.Status =VerifyMobileState.Ok;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Info(e.Message, e);
                        model.Status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    case Application.Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed:
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
            _logger.Debug("Calling AccountProvider to send mobile verification code for candidateId {0} to mobile number {1}",
                                                                                    candidateId, model.PhoneNumber);
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
                var message = string.Format("Sending Mobile verification code to mobile number {1} failed for candidateId {0} ", 
                                                                                        candidateId, model.PhoneNumber );
                _logger.Error(message, e);
                
                model.Status = VerifyMobileState.Error;
                model.ViewModelMessage = e.Message;
            }
            return model;
        }

        public VertifyUpdatedEmailViewModel UpdateEmailAddress(Guid userId, string updatedEmailAddress)
        {
            _logger.Debug("Calling AccountProvider to update username for Id: {0} to new email address: {1}", userId, updatedEmailAddress);
            var model = new VertifyUpdatedEmailViewModel();
            try
            {
                _userAccountService.UpdateUsername(userId, updatedEmailAddress);
                model.UpdateStatus = UpdateEmailStatus.Ok;
            }
            catch (CustomException ex)
            {
                switch (ex.Code)
                {
                    case ErrorCodes.UserDirectoryAccountExistsError:
                        model.UpdateStatus = UpdateEmailStatus.AccountAlreadyExixts;
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating username", ex);
                model.UpdateStatus = UpdateEmailStatus.Error;
            }

            return model;
        }

        public VertifyUpdatedEmailViewModel VerifyUpdatedEmailAddress(Guid userId, VertifyUpdatedEmailViewModel model)
        {
            _logger.Debug("Calling AccountProvider to verify code: {0} and password: {1} for the userId: {2} to update their username", model.PendingUsernameCode, model.Password, userId);

            try
            {
                _userAccountService.VerifyUpdateUsername(userId, model.PendingUsernameCode, model.Password);
                model.UpdateStatus = UpdateEmailStatus.Updated;
            }
            catch (CustomException ex)
            {
                switch (ex.Code)
                {
                    case ErrorCodes.UserDirectoryAccountExistsError:
                        model.UpdateStatus = UpdateEmailStatus.AccountAlreadyExixts;
                        break;
                    case ErrorCodes.InvalidUpdateUsernameCode:
                        model.UpdateStatus = UpdateEmailStatus.InvalidUpdateUsernameCode;
                        break;
                    case ErrorCodes.UserPasswordError:
                        model.UpdateStatus = UpdateEmailStatus.UserPasswordError;
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error verifying username update and password codes", ex);
                model.UpdateStatus = UpdateEmailStatus.Error;                
            }

            return model;
        }

        public VertifyUpdatedEmailViewModel ResendUpdateEmailAddressCode(Guid userId)
        {
            _logger.Debug("Calling AccountProvider to resent update email address code for the userId: {0}", userId);
            var model = new VertifyUpdatedEmailViewModel();

            try
            {
                _userAccountService.ResendUpdateUsernameCode(userId);
                model.UpdateStatus = UpdateEmailStatus.Ok;
            }
            catch (Exception ex)
            {
                _logger.Error("Error resending username code", ex);
                model.UpdateStatus = UpdateEmailStatus.Error;
            }

            return model;
        }
    }
}
        