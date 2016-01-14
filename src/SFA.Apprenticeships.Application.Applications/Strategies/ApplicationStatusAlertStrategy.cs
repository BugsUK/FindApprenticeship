namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Entities;
    using Extensions;
    using SFA.Infrastructure.Interfaces;

    public class ApplicationStatusAlertStrategy : IApplicationStatusAlertStrategy
    {
        private readonly ILogService _logService;
        private readonly IServiceBus _serviceBus;

        public ApplicationStatusAlertStrategy(ILogService logService, IServiceBus serviceBus)
        {
            _logService = logService;
            _serviceBus = serviceBus;
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
                    _serviceBus.PublishMessage(applicationStatusChanged);
                }
                catch (Exception ex)
                {
                    _logService.Warn(message + " failed!", ex);
                }
            }
        }
    }
}
