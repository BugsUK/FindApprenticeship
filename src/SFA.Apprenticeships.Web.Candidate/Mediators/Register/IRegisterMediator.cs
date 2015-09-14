using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using ViewModels.Candidate;
    using ViewModels.Register;

    public interface IRegisterMediator
    {
        MediatorResponse<RegisterViewModel> Register(RegisterViewModel registerViewModel);

        MediatorResponse<ActivationViewModel> Activate(Guid candidateId, ActivationViewModel activationViewModel);

        MediatorResponse UpdateMonitoringInformation(Guid candidateId, MonitoringInformationViewModel monitoringInformationViewModel);

        MediatorResponse SendMobileVerificationCode(Guid candidateId, string verifyMobileUrl);
    }
}
