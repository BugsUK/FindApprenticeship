namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using Common.Constants;
    using Common.Services;
    using Constants.Pages;
    using Domain.Entities.Users;
    using Providers;
    using Validators;
    using ViewModels;
    using ViewModels.Candidate;
    using ViewModels.Register;

    public class RegisterMediator : MediatorBase, IRegisterMediator
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterMediator(ICandidateServiceProvider candidateServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
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
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.SuccessfullyActivated, activationViewModel, activationViewModel.ViewModelMessage, UserMessageLevel.Success);
                case ActivateUserState.InvalidCode:
                    activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);
                    return GetMediatorResponse(RegisterMediatorCodes.Activate.InvalidActivationCode, activationViewModel, activatedResult);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public MediatorResponse UpdateMonitoringInformation(Guid candidateId, MonitoringInformationViewModel monitoringInformationViewModel)
        {
            try
            {
                _candidateServiceProvider.UpdateMonitoringInformation(candidateId, monitoringInformationViewModel);
                return GetMediatorResponse(RegisterMediatorCodes.UpdateMonitoringInformation.SuccessfullyUpdated);
            }
            catch{
                return GetMediatorResponse(RegisterMediatorCodes.UpdateMonitoringInformation.ErrorUpdating, ActivationPageMessages.UpdatingMonitoringInformationFailure, UserMessageLevel.Error);
            }
        }
    }
}