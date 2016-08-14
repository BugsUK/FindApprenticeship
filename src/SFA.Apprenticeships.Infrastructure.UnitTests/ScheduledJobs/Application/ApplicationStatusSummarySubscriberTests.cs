namespace SFA.Apprenticeships.Infrastructure.UnitTests.ScheduledJobs.Application
{
    using System;
    using Apprenticeships.Application.Applications;
    using Apprenticeships.Application.Applications.Entities;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Infrastructure.Processes.Applications;
    using Infrastructure.Processes.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApplicationStatusSummarySubscriberTests
    {
        private ApplicationStatusSummarySubscriber _applicationStatusSummarySubscriber;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessorMock;
        private Mock<IServiceBus> _serviceBus;
        readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();

        [SetUp]
        public void SetUp()
        {
            _applicationStatusProcessorMock = new Mock<IApplicationStatusProcessor>();
            _serviceBus = new Mock<IServiceBus>();

            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    StrictEtlValidation = true
                });

            _applicationStatusSummarySubscriber = new ApplicationStatusSummarySubscriber(
                _applicationStatusProcessorMock.Object, _serviceBus.Object, _configurationService.Object);
        }

        [Test]
        public void ShouldPublishMessageWhenNotReprocessingApplicationStatusSummary()
        {
            // Arrange.
            _applicationStatusProcessorMock.Setup(x => x.ProcessApplicationStatuses(It.IsAny<ApplicationStatusSummary>(), true));
            _serviceBus.Setup(x => x.PublishMessage(It.IsAny<VacancyStatusSummary>()));

            var appStatusSummary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.Empty,
                LegacyVacancyId = 101,
                ApplicationStatus = ApplicationStatuses.Submitted,
                VacancyStatus = VacancyStatuses.Live,
                ClosingDate = DateTime.UtcNow,
                LegacyApplicationId = 1001,
                UnsuccessfulReason = "Because"
            };

            // Act.
            var state = _applicationStatusSummarySubscriber.Consume(appStatusSummary);
            
            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _applicationStatusProcessorMock.Verify(x => x.ProcessApplicationStatuses(It.Is<ApplicationStatusSummary>(y => y == appStatusSummary), true));

            _serviceBus.Verify(
                mock =>
                    mock.PublishMessage(
                        It.Is<VacancyStatusSummary>(
                            v =>
                                v.LegacyVacancyId == appStatusSummary.LegacyVacancyId &&
                                v.ClosingDate == appStatusSummary.ClosingDate &&
                                v.VacancyStatus == appStatusSummary.VacancyStatus)));
        }

        [Test]
        public void ShouldNotPublishMessageWhenReprocessingApplicationStatusSummary()
        {
            // Arrange.
            _applicationStatusProcessorMock.Setup(x => x.ProcessApplicationStatuses(It.IsAny<ApplicationStatusSummary>(), true));
            _serviceBus.Setup(x => x.PublishMessage(It.IsAny<VacancyStatusSummary>()));

            var appStatusSummary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.NewGuid(),
                LegacyVacancyId = 101,
                ApplicationStatus = ApplicationStatuses.Submitted,
                VacancyStatus = VacancyStatuses.Live,
                ClosingDate = DateTime.UtcNow,
                LegacyApplicationId = 1001,
                UnsuccessfulReason = "Because"
            };

            // Act.
            var state = _applicationStatusSummarySubscriber.Consume(appStatusSummary);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _applicationStatusProcessorMock.Verify(x => x.ProcessApplicationStatuses(It.Is<ApplicationStatusSummary>(y => y == appStatusSummary), true));

            _serviceBus.Verify(mock => mock.PublishMessage(It.IsAny<VacancyStatusSummary>()), Times.Never);
        }
    }
}
