namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Entities;

    public interface IApplicationStatusAlertStrategy
    {
        void Send(ApplicationStatusSummary applicationStatusSummary);
    }
}
