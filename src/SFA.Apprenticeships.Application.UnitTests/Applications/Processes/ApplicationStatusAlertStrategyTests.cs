namespace SFA.Apprenticeships.Application.UnitTests.Applications.Processes
{
    using Apprenticeships.Application.Applications.Entities;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationStatusAlertStrategyTests
    {
        [TestCase(ApplicationStatuses.Unknown, false)]
        [TestCase(ApplicationStatuses.Draft, false)]
        [TestCase(ApplicationStatuses.ExpiredOrWithdrawn, false)]
        [TestCase(ApplicationStatuses.Submitting, false)]
        [TestCase(ApplicationStatuses.Submitted, false)]
        [TestCase(ApplicationStatuses.InProgress, false)]
        [TestCase(ApplicationStatuses.Successful, true)]
        [TestCase(ApplicationStatuses.Unsuccessful, true)]
        public void ShouldSendAlertWhenApplicationStatusIsSuccessfulOrUnsuccessful(ApplicationStatuses applicationStatus, bool shouldPublish)
        {
            // Arrange.
            var serviceBus = new Mock<IServiceBus>();

            var strategy = new ApplicationStatusAlertStrategyBuilder()
                .With(serviceBus)
                .Build();

            var summary = new ApplicationStatusSummaryBuilder(applicationStatus)
                .Build();

            // Act.
            strategy.Send(summary);

            // Assert.
            var times = shouldPublish ? Times.Once() : Times.Never();

            serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSendAlertWhenLegacySystemUpdate(bool isLegacySystemUpdate)
        {
            // Arrange.
            var serviceBus = new Mock<IServiceBus>();

            var strategy = new ApplicationStatusAlertStrategyBuilder()
                .With(serviceBus)
                .Build();

            var summary = new ApplicationStatusSummaryBuilder(ApplicationStatuses.Successful)
                .IsLegacySystemUpdate(isLegacySystemUpdate)
                .Build();

            // Act.
            strategy.Send(summary);

            // Assert.
            var times = isLegacySystemUpdate ? Times.Once() : Times.Never();

            serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [Test]
        public void ShouldSendWellFormedAlert()
        {
            // Arrange.
            const ApplicationStatuses applicationStatus = ApplicationStatuses.Successful;
            const int legacyApplicationId = 3456789;
            const string unsuccessfulReason = "You do not have the required grades";

            ApplicationStatusChanged applicationStatusChanged = null;

            var serviceBus = new Mock<IServiceBus>();

            serviceBus.Setup(mock => mock
                .PublishMessage(It.IsAny<ApplicationStatusChanged>()))
                .Callback<ApplicationStatusChanged>(each =>
                {
                    applicationStatusChanged = each;
                });

            var strategy = new ApplicationStatusAlertStrategyBuilder()
                .With(serviceBus)
                .Build();

            var summary = new ApplicationStatusSummaryBuilder(applicationStatus)
                .WithLegacyApplicationId(legacyApplicationId)
                .WithUnsuccessfulReason(unsuccessfulReason)
                .Build();

            // Act.
            strategy.Send(summary);

            // Assert.
            applicationStatusChanged.Should().NotBeNull();
            applicationStatusChanged.LegacyApplicationId.Should().Be(legacyApplicationId);
            applicationStatusChanged.ApplicationStatus.Should().Be(applicationStatus);
            applicationStatusChanged.UnsuccessfulReason.Should().Be(unsuccessfulReason);
        }
    }
}