﻿namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using Application.Applications;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Processes.Configuration;

    public class ApplicationEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly int _applicationStatusExtractWindow;

        public ApplicationEtlControlQueueConsumer(
            IConfigurationService configurationService,
            IJobControlQueue<StorageQueueMessage> messageService,
            IApplicationStatusProcessor applicationStatusProcessor, ILogService logger)
            : base(messageService, logger, "Application ETL", ScheduledJobQueues.ApplicationEtl)
        {
            _applicationStatusExtractWindow = configurationService.Get<ProcessConfiguration>(ProcessConfiguration.ConfigurationName).ApplicationStatusExtractWindow;
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var scheduleerNotification = GetLatestQueueMessage();
                if (scheduleerNotification != null)
                {
                    _applicationStatusProcessor.QueueApplicationStatusesPages(_applicationStatusExtractWindow);
                    MessageService.DeleteMessage(ScheduledJobQueues.ApplicationEtl, scheduleerNotification.MessageId, scheduleerNotification.PopReceipt);
                }
            });
        }
    }
}
