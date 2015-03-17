namespace SFA.Apprenticeships.Application.Communications.Strategies
{
    using System;

    public interface ISendSavedSearchAlertsStrategy
    {
        void SendSavedSearchAlerts(Guid batchId);
    }
}
