namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using Entities;
    using Domain.Entities.Applications;

    public interface IApplicationStatusAlertStrategy
    {
        void Send(ApplicationStatuses currentStatus, ApplicationStatusSummary applicationStatusSummary);
    }
}
