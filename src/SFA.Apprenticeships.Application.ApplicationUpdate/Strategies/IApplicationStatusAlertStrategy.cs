namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using System;
    using Entities;

    public interface IApplicationStatusAlertStrategy
    {
        void Send(ApplicationStatusSummary applicationStatusSummary);
    }
}
