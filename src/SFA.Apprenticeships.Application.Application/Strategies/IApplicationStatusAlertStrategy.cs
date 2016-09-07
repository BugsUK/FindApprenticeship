namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using Application.Entities;
    using Domain.Entities.Applications;

    public interface IApplicationStatusAlertStrategy
    {
        void Send(ApplicationStatuses currentStatus, ApplicationStatusSummary applicationStatusSummary);
    }
}
