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
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.Draft, false)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.ExpiredOrWithdrawn, false)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.Submitting, false)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.Submitted, false)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.InProgress, false)]
        [TestCase(ApplicationStatuses.Successful, ApplicationStatuses.Successful, false)]
        [TestCase(ApplicationStatuses.Unsuccessful, ApplicationStatuses.Unsuccessful, false)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.Successful, true)]
        [TestCase(ApplicationStatuses.Unknown, ApplicationStatuses.Unsuccessful, true)]
        public void ShouldSendAlertWhenApplicationStatusHasChangedToSuccessfulOrUnsuccessful(
            ApplicationStatuses currentStatus,
            ApplicationStatuses newStatus,
            bool shouldSendAlert)
        {
            // Arrange.
            var serviceBus = new Mock<IServiceBus>();

            var strategy = new ApplicationStatusAlertStrategyBuilder()
                .With(serviceBus)
                .Build();

            var summary = new ApplicationStatusSummaryBuilder(newStatus)
                .Build();

            // Act.
            strategy.Send(currentStatus, summary);

            // Assert.
            var times = shouldSendAlert ? Times.Once() : Times.Never();

            serviceBus.Verify(mock => mock
                .PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [TestCase(false)]
        [TestCase(true)]
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
            strategy.Send(ApplicationStatuses.Unknown,  summary);

            // Assert.
            var times = isLegacySystemUpdate ? Times.Once() : Times.Never();

            serviceBus.Verify(mock => mock
                .PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [Test]
        public void ShouldSendWellFormedAlert()
        {
            // Arrange.
            const ApplicationStatuses currentStatus = ApplicationStatuses.Submitted;
            const ApplicationStatuses newStatus = ApplicationStatuses.Successful;

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

            var summary = new ApplicationStatusSummaryBuilder(newStatus)
                .WithLegacyApplicationId(legacyApplicationId)
                .WithUnsuccessfulReason(unsuccessfulReason)
                .Build();

            // Act.
            strategy.Send(currentStatus, summary);

            // Assert.
            applicationStatusChanged.Should().NotBeNull();
            applicationStatusChanged.LegacyApplicationId.Should().Be(legacyApplicationId);
            applicationStatusChanged.ApplicationStatus.Should().Be(newStatus);
            applicationStatusChanged.UnsuccessfulReason.Should().Be(unsuccessfulReason);
        }
    }
}