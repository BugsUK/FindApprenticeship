namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using System;
    using Common.Mediators;
    using ViewModels.Application.Apprenticeship;

    public interface IApprenticeshipApplicationMediator
    {
        MediatorResponse<ApprenticeshipApplicationViewModel> GetApprenticeshipApplicationViewModel(Guid applicationId);
    }
}