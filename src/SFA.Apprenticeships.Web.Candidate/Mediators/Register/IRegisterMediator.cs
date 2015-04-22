namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using ViewModels.Register;

    public interface IRegisterMediator
    {
        MediatorResponse<RegisterViewModel> Register(RegisterViewModel registerViewModel);

        MediatorResponse<ActivationViewModel> Activate(Guid candidateId, ActivationViewModel activationViewModel);

        //todo: move to login mediator
        MediatorResponse<ForgottenCredentialsViewModel> ForgottenPassword(ForgottenCredentialsViewModel forgottenCredentialsViewModel);

        //todo: move to login mediator
        MediatorResponse<ForgottenCredentialsViewModel> ForgottenEmail(ForgottenCredentialsViewModel forgottenCredentialsViewModel);

        //todo: move to login mediator
        MediatorResponse<PasswordResetViewModel> ResetPassword(PasswordResetViewModel resetViewModel);
    }
}
