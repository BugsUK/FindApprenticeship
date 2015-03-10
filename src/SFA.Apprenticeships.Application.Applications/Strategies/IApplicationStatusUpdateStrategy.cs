namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using Domain.Entities.Applications;
    using Entities;

    public interface IApplicationStatusUpdateStrategy
    {
        void Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);

        void Update(TraineeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);
    }
}
