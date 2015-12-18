namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using SFA.Infrastructure.Interfaces;
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
    public class VacancySummaryCompleteSubscriberTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IServiceBus> _mockServiceBus;
        private Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _mockApprenticeshipIndexer;
        private Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>> _mockTraineeshipIndexer;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockServiceBus = new Mock<IServiceBus>();
            _mockApprenticeshipIndexer = new Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
            _mockTraineeshipIndexer = new Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>();
        }

        [Test]
        public void ShouldQueueCreateVacancySiteMapRequest()
        {
            // Arrange.
            const string apprenticeshipVacancyIndexName = "apprenticeships.2015-12-31-00";
            const string traineeshipVacancyIndexName = "traineeships.2015-12-31-00";

            var scheduledRefreshDateTime = DateTime.UtcNow;

            _mockApprenticeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(true);
            _mockApprenticeshipIndexer.Setup(mock => mock.SwapIndex(scheduledRefreshDateTime)).Returns(apprenticeshipVacancyIndexName);

            _mockTraineeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(true);
            _mockTraineeshipIndexer.Setup(mock => mock.SwapIndex(scheduledRefreshDateTime)).Returns(traineeshipVacancyIndexName);

            CreateVacancySiteMapRequest actualCreateVacancySiteMapRequest = null;

            _mockServiceBus.Setup(mock => mock
                .PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()))
                .Callback((CreateVacancySiteMapRequest request) => actualCreateVacancySiteMapRequest = request);

            var subscriber = new VacancySummaryCompleteSubscriber(
                _mockLogService.Object, _mockServiceBus.Object, _mockApprenticeshipIndexer.Object, _mockTraineeshipIndexer.Object);

            // Act.
            var state = subscriber.Consume(new VacancySummaryUpdateComplete
            {
                ScheduledRefreshDateTime = scheduledRefreshDateTime
            });

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _mockApprenticeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockApprenticeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Once);

            _mockTraineeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockTraineeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Once);

            _mockServiceBus.Verify(mock => mock.PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()), Times.Once());

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
            var scheduledRefreshDateTime = DateTime.UtcNow;

            _mockApprenticeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(apprenticeshipVacancyIndexOk);
            _mockTraineeshipIndexer.Setup(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime)).Returns(traineeshipVacancyIndexOk);

            var subscriber = new VacancySummaryCompleteSubscriber(
                _mockLogService.Object, _mockServiceBus.Object, _mockApprenticeshipIndexer.Object, _mockTraineeshipIndexer.Object);

            // Act.
            var state = subscriber.Consume(new VacancySummaryUpdateComplete
            {
                ScheduledRefreshDateTime = scheduledRefreshDateTime
            });

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _mockApprenticeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockApprenticeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Exactly(apprenticeshipVacancyIndexOk ? 1 : 0));

            _mockTraineeshipIndexer.Verify(mock => mock.IsIndexCorrectlyCreated(scheduledRefreshDateTime), Times.Once);
            _mockTraineeshipIndexer.Verify(mock => mock.SwapIndex(scheduledRefreshDateTime), Times.Exactly(traineeshipVacancyIndexOk ? 1 : 0));

            _mockServiceBus.Verify(mock => mock.PublishMessage(It.IsAny<CreateVacancySiteMapRequest>()), Times.Never);
        }
    }
}
