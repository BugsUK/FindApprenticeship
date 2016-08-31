namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using Application.Entities;
    using Domain.Entities.Applications;

    public interface IApplicationStatusUpdateStrategy
    {
        void Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);

        void Update(TraineeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);
    }
}
