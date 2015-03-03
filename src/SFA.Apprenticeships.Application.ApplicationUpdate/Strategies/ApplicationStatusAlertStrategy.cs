namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Entities;
    using Extensions;
    using Interfaces.Logging;

    public class ApplicationStatusAlertStrategy : IApplicationStatusAlertStrategy
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogService _logService;

        public ApplicationStatusAlertStrategy(IMessageBus messageBus, ILogService logService)
        {
            _messageBus = messageBus;
            _logService = logService;
        }

        public void Send(ApplicationStatusSummary applicationStatusSummary)
        {
            if (!applicationStatusSummary.IsLegacySystemUpdate()) return;

            var applicationStatus = applicationStatusSummary.ApplicationStatus;
            if (applicationStatus == ApplicationStatuses.Successful || applicationStatus == ApplicationStatuses.Unsuccessful)
            {
                var applicationStatusChanged = new ApplicationStatusChanged
                {
                    LegacyApplicationId = applicationStatusSummary.LegacyApplicationId,
                    ApplicationStatus = applicationStatusSummary.ApplicationStatus,
                    UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason
                };

                var message = string.Format("Queuing application status changed for LegacyApplicationId: {0} with ApplicationStatus: {1}, UnsuccessfulReason: {2}", applicationStatusChanged.LegacyApplicationId, applicationStatusChanged.ApplicationStatus, applicationStatusChanged.UnsuccessfulReason);
                try
                {
                    _logService.Debug(message);
                    _messageBus.PublishMessage(applicationStatusChanged);
                }
                catch (Exception ex)
                {
                    _logService.Warn(message + " failed!", ex);
                }
            }
        }
    }
}
