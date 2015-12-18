namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System.Diagnostics;
    using System.Linq;
    using Candidates.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;

    public class RootCommunicationHousekeeper : IRootCommunicationHousekeeper
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly IServiceBus _serviceBus;

        private readonly IApplicationStatusAlertCommunicationHousekeeper _applicationStatusAlertCommunicationHousekeeper;
        private readonly IExpiringDraftApplicationAlertCommunicationHousekeeper _expiringDraftApplicationAlertCommunicationHousekeeper;
        private readonly ISavedSearchAlertCommunicationHousekeeper _savedSearchAlertCommunicationHousekeeper;

        public RootCommunicationHousekeeper(
            ILogService logService,
            IConfigurationService configurationService,
            IServiceBus serviceBus,
            IApplicationStatusAlertCommunicationHousekeeper applicationStatusAlertCommunicationHousekeeper,
            IExpiringDraftApplicationAlertCommunicationHousekeeper expiringDraftApplicationAlertCommunicationHousekeeper,
            ISavedSearchAlertCommunicationHousekeeper savedSearchAlertCommunicationHousekeeper)
        {
            _configurationService = configurationService;
            _logService = logService;
            _serviceBus = serviceBus;

            _applicationStatusAlertCommunicationHousekeeper = applicationStatusAlertCommunicationHousekeeper;
            _expiringDraftApplicationAlertCommunicationHousekeeper = expiringDraftApplicationAlertCommunicationHousekeeper;
            _savedSearchAlertCommunicationHousekeeper = savedSearchAlertCommunicationHousekeeper;

            _applicationStatusAlertCommunicationHousekeeper.Successor = _expiringDraftApplicationAlertCommunicationHousekeeper;
            _expiringDraftApplicationAlertCommunicationHousekeeper.Successor = _savedSearchAlertCommunicationHousekeeper;
            _savedSearchAlertCommunicationHousekeeper.Successor = null;
        }

        public int QueueHousekeepingRequests()
        {
            if (!IsValidHousekeepingConfiguration())
            {
                _logService.Error("Housekeeping configuration is invalid");
                return 0;
            }

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            // TODO: AG: US794: consider finer-grained logging or using MongoDB log to determine query performance.
            var requests =
                _applicationStatusAlertCommunicationHousekeeper.GetHousekeepingRequests()
                    .Union(_expiringDraftApplicationAlertCommunicationHousekeeper.GetHousekeepingRequests()
                        .Union(_savedSearchAlertCommunicationHousekeeper.GetHousekeepingRequests()));

            var message = string.Format("Querying communications for housekeeping took {0}", stopwatch.Elapsed);

            var count = _serviceBus.PublishMessages(requests);

            stopwatch.Stop();

            message += string.Format(". Queuing {0} communication(s) for housekeeping took {1}", count, stopwatch.Elapsed);

            if (stopwatch.ElapsedMilliseconds > 60000 * 5)
            {
                _logService.Warn(message);
            }
            else
            {
                _logService.Info(message);
            }

            return count;
        }


        public void Handle(CommunicationHousekeepingRequest request)
        {
            _applicationStatusAlertCommunicationHousekeeper.Handle(request);
        }

        #region Helpers

        private bool IsValidHousekeepingConfiguration()
        {
            var housekeepingConfiguration = _configurationService.Get<HousekeepingConfiguration>();

            LogHousekeepingConfiguration(housekeepingConfiguration);

            return
                housekeepingConfiguration.HousekeepingCycleInHours != 0 &&
                housekeepingConfiguration.Communication.HardDeleteApplicationStatusAlertAfterCycles != 0 &&
                housekeepingConfiguration.Communication.HardDeleteExpiringDraftApplicationAlertAfterCycles != 0 &&
                housekeepingConfiguration.Communication.HardDeleteSavedSearchAlertAfterCycles != 0;
        }

        private void LogHousekeepingConfiguration(HousekeepingConfiguration housekeepingConfiguration)
        {
            var args = new object[]
            {
                "HousekeepingCycleInHours",
                housekeepingConfiguration.HousekeepingCycleInHours,
                "HardDeleteApplicationStatusAlertAfterCycles",
                housekeepingConfiguration.Communication.HardDeleteApplicationStatusAlertAfterCycles,
                "HardDeleteExpiringDraftApplicationAlertAfterCycles",
                housekeepingConfiguration.Communication.HardDeleteExpiringDraftApplicationAlertAfterCycles,
                "HardDeleteSavedSearchAlertAfterCycles",
                housekeepingConfiguration.Communication.HardDeleteSavedSearchAlertAfterCycles
            };

            _logService.Info("Housekeeping configuration: {0}={1} {2}={3} {4}={5} {6}={7}", args);
        }

        #endregion
    }
}