namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Vacancies;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

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
                    var yesterday  = DateTime.UtcNow.AddDays(-1);
                    var endOfDay = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);

                    _logger.Info("Calling vacancy status processor to queue vacancies eligible for closure");

                    _vacancyStatusProcessor.QueueVacanciesForClosure(endOfDay);

                    MessageService.DeleteMessage(ScheduledJobQueues.VacancyStatus, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
                    _logger.Info("Queued vacancies eligible for closure and deleted message");
                }
            });
        }
    }
}
