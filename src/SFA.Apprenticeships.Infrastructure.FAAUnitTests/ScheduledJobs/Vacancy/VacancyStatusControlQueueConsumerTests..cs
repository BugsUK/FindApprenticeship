namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.ScheduledJobs.Vacancy
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Vacancies;
    using Domain.Interfaces.Messaging;
    using Infrastructure.ScheduledJobs.Consumers;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class VacancyStatusControlQueueConsumerTests
    {
        private Mock<ILogService> _logService;
        private Mock<IJobControlQueue<StorageQueueMessage>> _messageServiceMock;
        private Mock<IVacancyStatusProcessor> _mockVacancyStatusProcessor;
        //private Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _apprenticeshipIndexerService;
        //private Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>> _traineeshipsIndexerService;
        
        [SetUp]
        public void SetUp()
        {
            _logService = new Mock<ILogService>();
            _messageServiceMock = new Mock<IJobControlQueue<StorageQueueMessage>>();
            _mockVacancyStatusProcessor = new Mock<IVacancyStatusProcessor>();
            _logService = new Mock<ILogService>();
        }
        
        [Test]
        public void ShouldRunProcessForEndOfPreviousDay()
        {
            var now = DateTime.UtcNow;
            var previousDay = now.AddDays(-1);
            var expectedDateTime = new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 23, 59, 59);

            var scheduledMessageQueue = GetScheduledMessagesQueue();
            _messageServiceMock.Setup(x => x.GetMessage(It.IsAny<string>())).Returns(scheduledMessageQueue.Dequeue);
            var vacancyConsumer = new VacancyStatusControlQueueConsumer(_messageServiceMock.Object, _logService.Object, _mockVacancyStatusProcessor.Object);
            var task = vacancyConsumer.CheckScheduleQueue();
            task.Wait();

            //_messageServiceMock.Verify(x => x.GetMessage(It.Is<string>(queueName => queueName == ScheduledJobQueues.VacancyEtl)), Times.Exactly(queuedScheduledMessages + 1));
            //_messageServiceMock.Verify(x => x.DeleteMessage(It.Is<string>(queueName => queueName == ScheduledJobQueues.VacancyEtl), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(queuedScheduledMessages == 0 ? 0 : queuedScheduledMessages - 1));
            //_apprenticeshipIndexerService.Verify(x => x.CreateScheduledIndex(It.Is<DateTime>(d => d == DateTime.Today)), Times.Exactly(queuedScheduledMessages > 0 ? 1 : 0));
            _mockVacancyStatusProcessor.Verify(x => x.QueueVacanciesForClosure(expectedDateTime), Times.Once);
        }

        private static Queue<StorageQueueMessage> GetScheduledMessagesQueue()
        {
            var queue = new Queue<StorageQueueMessage>();


            var storageScheduleMessage = new StorageQueueMessage
            {
                ClientRequestId = Guid.NewGuid(),
                ExpectedExecutionTime = DateTime.Today,
                SchedulerJobId = Guid.NewGuid().ToString()
            };

            queue.Enqueue(storageScheduleMessage);

            queue.Enqueue(null);

            return queue;
        }
    }
}
