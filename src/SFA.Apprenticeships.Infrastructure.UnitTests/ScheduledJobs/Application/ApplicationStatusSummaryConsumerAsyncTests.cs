namespace SFA.Apprenticeships.Infrastructure.UnitTests.ScheduledJobs.Application
{
    using System;
    using Apprenticeships.Application.Applications;
    using Apprenticeships.Application.Applications.Entities;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Infrastructure.Processes.Applications;
    using Infrastructure.Processes.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationStatusSummaryConsumerAsyncTests
    {
        private ApplicationStatusSummaryConsumerAsync _applicationStatusSummaryConsumerAsync;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessorMock;
        private Mock<IMessageBus> _bus;
        readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();

        [SetUp]
        public void SetUp()
        {
            _applicationStatusProcessorMock = new Mock<IApplicationStatusProcessor>();
            _bus = new Mock<IMessageBus>();
            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    StrictEtlValidation = true
                });
            _applicationStatusSummaryConsumerAsync = new ApplicationStatusSummaryConsumerAsync(_applicationStatusProcessorMock.Object, _bus.Object, _configurationService.Object);
        }

        [Test]
        public void ConsumeShouldProcessMessageCorrectly()
        {
            _applicationStatusProcessorMock.Setup(x => x.ProcessApplicationStatuses(It.IsAny<ApplicationStatusSummary>(), true));
            _bus.Setup(x => x.PublishMessage(It.IsAny<VacancyStatusSummary>()));

            var appStatusSummary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.Empty,
                LegacyVacancyId = 101,
                ApplicationStatus = ApplicationStatuses.Submitted,
                VacancyStatus = VacancyStatuses.Live,
                ClosingDate = DateTime.Now,
                LegacyApplicationId = 1001,
                UnsuccessfulReason = "Because"
            };

            var task = _applicationStatusSummaryConsumerAsync.Consume(appStatusSummary);
            task.Wait();

            _applicationStatusProcessorMock.Verify(x => x.ProcessApplicationStatuses(It.Is<ApplicationStatusSummary>(y => y == appStatusSummary), true));
            _bus.Verify(
                x =>
                    x.PublishMessage(
                        It.Is<VacancyStatusSummary>(
                            v =>
                                v.LegacyVacancyId == appStatusSummary.LegacyVacancyId &&
                                v.ClosingDate == appStatusSummary.ClosingDate &&
                                v.VacancyStatus == appStatusSummary.VacancyStatus)));

        }
    }
}
