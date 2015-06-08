namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using Interfaces.Logging;

    public class RootApplicationHousekeeper : IRootApplicationHousekeeper
    {
        private readonly ILogService _logService;
        private readonly IMessageBus _messageBus;

        private readonly IDraftApplicationForExpiredVacancyHousekeeper _draftApplicationForExpiredVacancyHousekeeper;
        private readonly ISubmittedApplicationHousekeeper _submittedApplicationHousekeeper;

        public RootApplicationHousekeeper(
            ILogService logService,
            IMessageBus messageBus,
            IDraftApplicationForExpiredVacancyHousekeeper draftApplicationForExpiredVacancyHousekeeper,
            ISubmittedApplicationHousekeeper submittedApplicationHousekeeper)
        {
            _logService = logService;
            _messageBus = messageBus;

            _draftApplicationForExpiredVacancyHousekeeper = draftApplicationForExpiredVacancyHousekeeper;
            _submittedApplicationHousekeeper = submittedApplicationHousekeeper;

            _draftApplicationForExpiredVacancyHousekeeper.Succesor = _submittedApplicationHousekeeper;
        }

        public int QueueHousekeepingRequests()
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();

            var applications =
                _draftApplicationForExpiredVacancyHousekeeper.GetHousekeepingRequests()
                    .Union(_submittedApplicationHousekeeper.GetHousekeepingRequests());

            var message = string.Format("Querying applications for housekeeping took {0}", stopwatch.Elapsed);

            var count = 0;

            Parallel.ForEach(applications, application =>
            {
                _messageBus.PublishMessage(application);
                Interlocked.Increment(ref count);
            });

            stopwatch.Stop();

            message += string.Format(". Queuing {0} applications for housekeeping took {1}", count, stopwatch.Elapsed);

            if (stopwatch.ElapsedMilliseconds > 60000)
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
    }
}