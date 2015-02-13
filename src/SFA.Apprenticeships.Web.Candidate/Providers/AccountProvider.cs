﻿namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using ViewModels.Account;
    using ViewModels.Locations;

    public class AccountProvider : IAccountProvider
    {
        private readonly ILogService _logger;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public AccountProvider(
            ICandidateService candidateService,
            IMapper mapper, ILogService logger)
        {
            _mapper = mapper;
            _logger = logger;
            _candidateService = candidateService;
        }

        public SettingsViewModel GetSettingsViewModel(Guid candidateId)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to get Settings View Model for candidate with Id={0}", candidateId);
                
                var candidate = _candidateService.GetCandidate(candidateId);
                var settings = _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);
                settings.AllowEmailComms = candidate.CommunicationPreferences.AllowEmail;
                settings.AllowSmsComms = candidate.CommunicationPreferences.AllowMobile;
                settings.VerifiedMobile = candidate.CommunicationPreferences.VerifiedMobile;
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
                    model.VerifiedMobile = false;
                }
                candidate.CommunicationPreferences.VerifiedMobile = model.VerifiedMobile;
                
                PatchRegistrationDetails(candidate.RegistrationDetails, model);
                _candidateService.SaveCandidate(candidate);
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
            //todo try catch 
            var candidate = _candidateService.GetCandidate(candidateId);
            if (candidate.MobileVerificationRequired())
            {
                return new VerifyMobileViewModel { MobileNumber = candidate.RegistrationDetails.PhoneNumber };
            }
            return new VerifyMobileViewModel { Status = VerifyMobileState.MobileVerificationNotRequired };
        }

        public VerifyMobileViewModel VerifyMobile(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to verify mobile code candidateId {0} and mobile number {1}",
                                                                                    candidateId, model.MobileNumber);
            try
            {
                _candidateService.VerifyMobileCode(candidateId, model.VerifyMobileCode);
                return new VerifyMobileViewModel { Status = VerifyMobileState.Ok };
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Info(e.Message, e);
                        return new VerifyMobileViewModel { Status = VerifyMobileState.MobileVerificationNotRequired };
                    case Application.Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed:
                        _logger.Info(e.Message, e);
                        return new VerifyMobileViewModel
                        {
                            Status = VerifyMobileState.VerifyMobileCodeInvalid
                        };
                    default:
                        _logger.Error(e.Message, e);
                        return new VerifyMobileViewModel { Status = VerifyMobileState.Error };
                }
            }
            catch (Exception e)
            {
                _logger.Error("Mobile code verification failed for candidateId {0} and Lme {1}", candidateId, model.MobileNumber, e);
                return new VerifyMobileViewModel { Status = VerifyMobileState.Error };
            }
        }

        public VerifyMobileViewModel SendMobileVerificationCode(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to send mobile verification code for candidateId {0} to mobile number {1}",
                                                                                    candidateId, model.MobileNumber);

            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                _candidateService.SendMobileVerificationCode(candidate);
                return new VerifyMobileViewModel { Status = VerifyMobileState.Ok };
            }
            catch (CustomException e)
            {
                VerifyMobileState status;
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    default:
                        status = VerifyMobileState.Error;
                        _logger.Error(e.Message, e);
                        break;
                }
                return new VerifyMobileViewModel(e.Message) { Status = status };
            }
            catch (Exception e)
            {
                var message = string.Format("Sending Mobile verification code to mobile number {1} failed for candidateId {0} ", 
                                                                                        candidateId, model.MobileNumber );
                _logger.Error(message, e);
                return new VerifyMobileViewModel(message) { Status = VerifyMobileState.Error};
            }
        }
    }
}
        