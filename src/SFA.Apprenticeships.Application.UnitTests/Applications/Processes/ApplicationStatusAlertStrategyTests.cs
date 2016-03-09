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
        public void ApplicationStatusPublishMessage(ApplicationStatuses applicationStatus, bool shouldPublish)
        {
            var serviceBus = new Mock<IServiceBus>();
            var strategy = new ApplicationStatusAlertStrategyBuilder().With(serviceBus).Build();
            var summary = new ApplicationStatusSummaryBuilder(applicationStatus).Build();

            strategy.Send(summary);

            var times = shouldPublish ? Times.Once() : Times.Never();
            serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsLegacySystemUpdatePublishMessage(bool isLegacySystemUpdate)
        {
            var serviceBus = new Mock<IServiceBus>();
            var strategy = new ApplicationStatusAlertStrategyBuilder().With(serviceBus).Build();
            var summary = new ApplicationStatusSummaryBuilder(ApplicationStatuses.Successful).IsLegacySystemUpdate(isLegacySystemUpdate).Build();

            strategy.Send(summary);

            var times = isLegacySystemUpdate ? Times.Once() : Times.Never();
            serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<ApplicationStatusChanged>()), times);
        }

        [Test]
        public void Mapping()
        {
            const ApplicationStatuses applicationStatus = ApplicationStatuses.Successful;
            const int legacyApplicationId = 3456789;
            const string unsuccessfulReason = "You do not have the required grades";

            ApplicationStatusChanged applicationStatusChanged = null;

            var serviceBus = new Mock<IServiceBus>();

            serviceBus.Setup(mb => mb.PublishMessage(It.IsAny<ApplicationStatusChanged>())).Callback<ApplicationStatusChanged>(asc => { applicationStatusChanged = asc; });

            var strategy = new ApplicationStatusAlertStrategyBuilder().With(serviceBus).Build();
            var summary = new ApplicationStatusSummaryBuilder(applicationStatus).WithLegacyApplicationId(legacyApplicationId).WithUnsuccessfulReason(unsuccessfulReason).Build();

            strategy.Send(summary);

            applicationStatusChanged.Should().NotBeNull();
            applicationStatusChanged.LegacyApplicationId.Should().Be(legacyApplicationId);
            applicationStatusChanged.ApplicationStatus.Should().Be(applicationStatus);
            applicationStatusChanged.UnsuccessfulReason.Should().Be(unsuccessfulReason);
        }
    }
}