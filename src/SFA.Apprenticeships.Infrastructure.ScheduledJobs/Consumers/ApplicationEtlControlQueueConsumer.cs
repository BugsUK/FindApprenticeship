namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using Application.Applications;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class ApplicationEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public ApplicationEtlControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService,
            IApplicationStatusProcessor applicationStatusProcessor, ILogService logger)
            : base(messageService, logger, "Application ETL", ScheduledJobQueues.ApplicationEtl)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var scheduleerNotification = GetLatestQueueMessage();
                if (scheduleerNotification != null)
                {
                    _applicationStatusProcessor.QueueApplicationStatusesPages();
                    MessageService.DeleteMessage(ScheduledJobQueues.ApplicationEtl, scheduleerNotification.MessageId, scheduleerNotification.PopReceipt);
                }
            });
        }
    }
}
