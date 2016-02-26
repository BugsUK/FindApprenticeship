using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using Common.Constants;
    using Common.Validators;
    using Common.ViewModels.Candidate;
    using Constants.Pages;
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Providers;
    using Validators;
    using ViewModels;
    using ViewModels.Register;

    public class RegisterMediator : MediatorBase, IRegisterMediator
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly MonitoringInformationViewModelValidator _monitoringInformationViewModelValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterMediator(ICandidateServiceProvider candidateServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            MonitoringInformationViewModelValidator monitoringInformationViewModelValidator)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _monitoringInformationViewModelValidator = monitoringInformationViewModelValidator;
        }

        public MediatorResponse<RegisterViewModel> Register(RegisterViewModel registerViewModel)
        {
            var emailAddress = string.IsNullOrWhiteSpace(registerViewModel.EmailAddress)
                ? string.Empty
                : registerViewModel.EmailAddress.Trim();

            UserNameAvailability userNameAvailable;

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                userNameAvailable = new UserNameAvailability
                {
                    HasError = false,
                    IsUserNameAvailable = false
                };
            }
            else
            {
                userNameAvailable = _candidateServiceProvider.IsUsernameAvailable(emailAddress);    
            }

            if (!userNameAvailable.HasError)
            {
                registerViewModel.IsUsernameAvailable = userNameAvailable.IsUserNameAvailable;
                var validationResult = _registerViewModelServerValidator.Validate(registerViewModel);

                if (!validationResult.IsValid)
                {
                    return GetMediatorResponse(RegisterMediatorCodes.Register.ValidationFailed, registerViewModel, validationResult);
                }

                var registered = _candidateServiceProvider.Register(registerViewModel);
                if (registered)
                {
                    return GetMediatorResponse(RegisterMediatorCodes.Register.SuccessfullyRegistered, registerViewModel);
                }
            }

            return GetMediatorResponse(RegisterMediatorCodes.Register.RegistrationFailed, registerViewModel, RegisterPageMessages.RegistrationFailed, UserMessageLevel.Warning);
        }

        public MediatorResponse<ActivationViewModel> Activate(Guid candidateId, ActivationViewModel activationViewModel)
        {
            var activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);

            if (!activatedResult.IsValid)
            {
                return GetMediatorResponse(RegisterMediatorCodes.Activate.FailedValidation, activationViewModel, activatedResult);
            }

            activationViewModel = _candidateServiceProvider.Activate(activationViewModel, candidateId);

            switch (activationViewModel.State)
            {
                case ActivateUserState.Activated:
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.SuccessfullyActivated, activationViewModel, ActivationPageMessages.AccountActivated, UserMessageLevel.Success);
                case ActivateUserState.Error:
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.ErrorActivating, activationViewModel, activationViewModel.ViewModelMessage, UserMessageLevel.Error);
                case ActivateUserState.InvalidCode:
                    activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.InvalidActivationCode, activationViewModel, activatedResult);
                case ActivateUserState.AlreadyActivated:
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.SuccessfullyActivated, activationViewModel, ActivationPageMessages.AlreadyActivated, UserMessageLevel.Info);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public MediatorResponse UpdateMonitoringInformation(Guid candidateId, MonitoringInformationViewModel monitoringInformationViewModel)
        {
            try
            {
                var validationResult = _monitoringInformationViewModelValidator.Validate(monitoringInformationViewModel);

                if (!validationResult.IsValid)
                {
                    return GetMediatorResponse(RegisterMediatorCodes.UpdateMonitoringInformation.FailedValidation, monitoringInformationViewModel, validationResult);
                }

                _candidateServiceProvider.UpdateMonitoringInformation(candidateId, monitoringInformationViewModel);
                return GetMediatorResponse(RegisterMediatorCodes.UpdateMonitoringInformation.SuccessfullyUpdated);
            }
            catch{
                return GetMediatorResponse(RegisterMediatorCodes.UpdateMonitoringInformation.ErrorUpdating, ActivationPageMessages.UpdatingMonitoringInformationFailure, UserMessageLevel.Error);
            }
        }

        public MediatorResponse SendMobileVerificationCode(Guid candidateId, string verifyMobileUrl)
        {
            try
            {
                var phoneNumber = _candidateServiceProvider.SendMobileVerificationCode(candidateId);
                var message = string.Format(LoginPageMessages.MobileVerificationRequiredText, phoneNumber, verifyMobileUrl);
                return GetMediatorResponse(RegisterMediatorCodes.SendMobileVerificationCode.Success, message, UserMessageLevel.Info);
            }
            catch (CustomException ex)
            {
                if (ex.Code == ErrorCodes.EntityStateError)
                {
                    return GetMediatorResponse(RegisterMediatorCodes.SendMobileVerificationCode.NotRequired);
                }
                return GetMediatorResponse(RegisterMediatorCodes.SendMobileVerificationCode.Error);
            }
        }
    }
}