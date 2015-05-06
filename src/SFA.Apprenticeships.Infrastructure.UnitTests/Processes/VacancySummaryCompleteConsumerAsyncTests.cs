namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Interfaces.Logging;
    using Application.Vacancies.Entities;
    using Application.Vacancies.Entities.SiteMap;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using FluentAssertions;
    using Infrastructure.Processes.Vacancies;
    using Moq;
    using NUnit.Framework;
    using VacancyIndexer;

    [TestFixture]
    public class VacancySummaryCompleteConsumerAsyncTests
    {
        private Mock<ILogService> _mockLogger;
        private Mock<IMessageBus> _mockMessageBus;
        private Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _mockApprenticeshipIndexer;
        private Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>> _mockTraineeshipIndexer;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();
            _mockMessageBus = new Mock<IMessageBus>();
            _mockApprenticeshipIndexer = new Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
            _mockTraineeshipIndexer = new Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>();
        }

        [Test]
        public void ShouldQueueCreateVacancySiteMapRequest()
        {
            // Arrange.
            const string apprenticeshipVacancyIndexName = "apprenticeships.2015-12-31-00";
            const string traineeshipVacancyIndexName = "traineeships.2015-12-31-00";

            var scheduledRefreshDateTime = DateTime.Now;

            _mockApprenticeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(true);
            _mockApprenticeshipIndexer.Setup(mock => mock.SwapIndex(scheduledRefreshDateTime)).Returns(apprenticeshipVacancyIndexName);

            _mockTraineeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(true);
            _mockTraineeshipIndexer.Setup(mock => mock.SwapIndex(scheduledRefreshDateTime)).Returns(traineeshipVacancyIndexName);

            CreateVacancySiteMapRequest actualCreateVacancySiteMapRequest = null;

            _mockMessageBus.Setup(mock => mock
                .PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()))
                .Callback((CreateVacancySiteMapRequest request) => actualCreateVacancySiteMapRequest = request);

            var consumer = new VacancySummaryCompleteConsumerAsync(
                _mockLogger.Object, _mockMessageBus.Object, _mockApprenticeshipIndexer.Object, _mockTraineeshipIndexer.Object);

            // Act.
            consumer.Consume(new VacancySummaryUpdateComplete
            {
                ScheduledRefreshDateTime = scheduledRefreshDateTime
            });

            // Assert.
            _mockApprenticeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockApprenticeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Once);

            _mockTraineeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockTraineeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Once);

            _mockMessageBus.Verify(mock => mock.PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()), Times.Once());

            actualCreateVacancySiteMapRequest.ShouldBeEquivalentTo(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = apprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = traineeshipVacancyIndexName
            });
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ShouldNotQueueCreateVacancySiteMapRequestWhenIndexNotCorrectlyCreated(
            bool apprenticeshipVacancyIndexOk, bool traineeshipVacancyIndexOk)
        {
            // Arrange.
            var scheduledRefreshDateTime = DateTime.Now;

            _mockApprenticeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(apprenticeshipVacancyIndexOk);
            _mockTraineeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(traineeshipVacancyIndexOk);

            var consumer = new VacancySummaryCompleteConsumerAsync(
                _mockLogger.Object, _mockMessageBus.Object, _mockApprenticeshipIndexer.Object, _mockTraineeshipIndexer.Object);

            // Act.
            var task = consumer.Consume(new VacancySummaryUpdateComplete
            {
                ScheduledRefreshDateTime = scheduledRefreshDateTime
            });
            task.Wait();

            // Assert.
            _mockApprenticeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockApprenticeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Exactly(apprenticeshipVacancyIndexOk ? 1 : 0));

            _mockTraineeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockTraineeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Exactly(traineeshipVacancyIndexOk ? 1 : 0));

            _mockMessageBus.Verify(mock => mock.PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()), Times.Never);
        }
    }
}
