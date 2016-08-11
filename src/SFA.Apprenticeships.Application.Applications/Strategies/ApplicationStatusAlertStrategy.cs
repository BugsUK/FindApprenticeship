namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Entities;
    using Extensions;
    using Infrastructure.Interfaces;

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
            if (!applicationStatusSummary.IsLegacySystemUpdate())
            {
                return;
            }

            if (!(applicationStatusSummary.ApplicationStatus == ApplicationStatuses.Successful ||
                applicationStatusSummary.ApplicationStatus == ApplicationStatuses.Unsuccessful))
            {
                return;
            }

            var applicationStatusChanged = new ApplicationStatusChanged
            {
                LegacyApplicationId = applicationStatusSummary.LegacyApplicationId,
                ApplicationStatus = applicationStatusSummary.ApplicationStatus,
                UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason
            };

            var message =
                $"Queuing application status changed for LegacyApplicationId: {applicationStatusChanged.LegacyApplicationId} with ApplicationStatus: {applicationStatusChanged.ApplicationStatus}, UnsuccessfulReason: {applicationStatusChanged.UnsuccessfulReason}";

            try
            {
                _logService.Debug(message);
                _serviceBus.PublishMessage(applicationStatusChanged);
            }
            catch (Exception e)
            {
                _logService.Warn(message + " failed!", e);
            }
        }

        public void Send(ApplicationStatuses originalStatus, ApplicationStatusSummary applicationStatusSummary)
        {
            throw new NotImplementedException();
        }
    }
}
