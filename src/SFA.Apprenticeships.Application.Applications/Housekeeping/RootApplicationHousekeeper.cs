namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System.Diagnostics;
    using System.Linq;
    using Candidates.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;

    public class RootApplicationHousekeeper : IRootApplicationHousekeeper
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly IServiceBus _serviceBus;

        private readonly IDraftApplicationForExpiredVacancyHousekeeper _draftApplicationForExpiredVacancyHousekeeper;
        private readonly ISubmittedApplicationHousekeeper _submittedApplicationHousekeeper;

        public RootApplicationHousekeeper(
            ILogService logService,
            IConfigurationService configurationService,
            IServiceBus serviceBus,
            IDraftApplicationForExpiredVacancyHousekeeper draftApplicationForExpiredVacancyHousekeeper,
            ISubmittedApplicationHousekeeper submittedApplicationHousekeeper)
        {
            _logService = logService;
            _configurationService = configurationService;
            _serviceBus = serviceBus;

            _draftApplicationForExpiredVacancyHousekeeper = draftApplicationForExpiredVacancyHousekeeper;
            _submittedApplicationHousekeeper = submittedApplicationHousekeeper;

            _draftApplicationForExpiredVacancyHousekeeper.Successor = _submittedApplicationHousekeeper;
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
                _draftApplicationForExpiredVacancyHousekeeper.GetHousekeepingRequests()
                    .Union(_submittedApplicationHousekeeper.GetHousekeepingRequests());

            var message = string.Format("Querying applications for housekeeping took {0}", stopwatch.Elapsed);

            var count = _serviceBus.PublishMessages(requests);

            stopwatch.Stop();

            message += string.Format(". Queuing {0} application(s) for housekeeping took {1}", count, stopwatch.Elapsed);

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

        public void Handle(ApplicationHousekeepingRequest request)
        {
            _draftApplicationForExpiredVacancyHousekeeper.Handle(request);
        }

        #region Helpers

        private bool IsValidHousekeepingConfiguration()
        {
            var housekeepingConfiguration = _configurationService.Get<HousekeepingConfiguration>();

            LogHousekeepingConfiguration(housekeepingConfiguration);

            return
                housekeepingConfiguration.HousekeepingCycleInHours != 0 &&
                housekeepingConfiguration.Application.HardDeleteDraftApplicationForExpiredVacancyAfterCycles != 0 &&
                housekeepingConfiguration.Application.HardDeleteSubmittedApplicationAfterCycles != 0;
        }

        private void LogHousekeepingConfiguration(HousekeepingConfiguration housekeepingConfiguration)
        {
            var args = new object[]
            {
                "HousekeepingCycleInHours",
                housekeepingConfiguration.HousekeepingCycleInHours,
                "HardDeleteDraftApplicationForExpiredVacancyAfterCycles",
                housekeepingConfiguration.Application.HardDeleteDraftApplicationForExpiredVacancyAfterCycles,
                "HardDeleteSubmittedApplicationAfterCycles",
                housekeepingConfiguration.Application.HardDeleteSubmittedApplicationAfterCycles
            };

            _logService.Info("Housekeeping configuration: {0}={1} {2}={3} {4}={5}", args);
        }

        #endregion
    }
}