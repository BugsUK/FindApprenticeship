﻿namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.UnitTests
{
    using System;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;
    using Moq;
    using NUnit.Framework;
    using Processes.Vacancies;

    [TestFixture]
    public class VacancySummaryPageConsumerAsyncTests
    {
        private Mock<IMessageBus> _messageBusMock;
        private Mock<IVacancySummaryProcessor> _vacancySummaryProcessorMock;
        private Mock<ILogService> _logService;

        [SetUp]
        public void SetUp()
        {
            _messageBusMock = new Mock<IMessageBus>();
            _vacancySummaryProcessorMock = new Mock<IVacancySummaryProcessor>();
            _logService = new Mock<ILogService>();
        }

        [TestCase(1, 20)]
        [TestCase(2, 4)]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        public void ShouldAlwaysQueueVacancySummariesAndOnlyCallCompleteOnLastPage(int pageNumber, int totalPages)
        {
            var scheduledRefreshDate = new DateTime(2001, 1, 1);
            var vacancySummaryPage = new VacancySummaryPage
            {
                PageNumber = pageNumber,
                TotalPages = totalPages,
                ScheduledRefreshDateTime = scheduledRefreshDate
            };

            var vacancySummaryPageConsumerAsync = new VacancySummaryPageConsumerAsync(_messageBusMock.Object, _vacancySummaryProcessorMock.Object, _logService.Object);
            var task = vacancySummaryPageConsumerAsync.Consume(vacancySummaryPage);
            task.Wait();

            _vacancySummaryProcessorMock.Verify(x => x.QueueVacancySummaries(It.Is<VacancySummaryPage>(vsp => vsp == vacancySummaryPage)), Times.Once);
            _messageBusMock.Verify(x => x.PublishMessage(It.Is<VacancySummaryUpdateComplete>(vsuc => vsuc.ScheduledRefreshDateTime == scheduledRefreshDate)), Times.Exactly(pageNumber == totalPages ? 1 : 0));
        }
    }
}
