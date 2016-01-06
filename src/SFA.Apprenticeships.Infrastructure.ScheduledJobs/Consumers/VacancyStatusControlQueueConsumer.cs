namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class VacancyStatusControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ILogService _logger;
        private readonly IVacancyStatusProcessor _vacancyStatusProcessor;

        public VacancyStatusControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, ILogService logger, IVacancyStatusProcessor vacancyStatusProcessor)
            : base(messageService, logger, "Vacancy Status Processor", ScheduledJobQueues.VacancyStatus)
        {
            _vacancyStatusProcessor = vacancyStatusProcessor;
            _logger = logger;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();
                if (schedulerNotification != null)
                {
                    _logger.Info("Calling vacancy status processor to queue vacancies eligible for closure");
                    _vacancyStatusProcessor.QueueVacanciesForClosure(DateTime.Today.AddSeconds(-1));
                    MessageService.DeleteMessage(ScheduledJobQueues.VacancyStatus, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
                    _logger.Info("Queued vacancies eligible for closure and deleted message");
                }
            });
        }
    }
}
