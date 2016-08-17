namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Communications;
    using SFA.Infrastructure.Interfaces;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class DailyDigestControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICommunicationProcessor _communicationProcessor;

        public DailyDigestControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService,
            ICommunicationProcessor communicationProcessor, ILogService logger)
            : base(messageService, logger, "Communications", ScheduledJobQueues.DailyDigest)
        {
            _communicationProcessor = communicationProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();

                if (schedulerNotification == null) return;

                var tasks = new List<Task>
                {
                    new Task(() => _communicationProcessor.SendDailyDigests(schedulerNotification.ClientRequestId)),
                    new Task(() => _communicationProcessor.SendSavedSearchAlerts(schedulerNotification.ClientRequestId))
                };

                tasks.ForEach(t => t.Start());
                Task.WaitAll(tasks.ToArray());

                MessageService.DeleteMessage(ScheduledJobQueues.DailyDigest, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
            });
        }
    }
}
