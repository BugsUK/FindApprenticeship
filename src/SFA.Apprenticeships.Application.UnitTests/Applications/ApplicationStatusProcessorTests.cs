namespace SFA.Apprenticeships.Application.UnitTests.Applications
{
    using Apprenticeships.Application.Application.Entities;
    using Apprenticeships.Application.Application.Strategies;
    using Apprenticeships.Application.Applications;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using Interfaces;
    using System;
    using System.Linq;

    [TestFixture]
    public class ApplicationStatusProcessorTests
    {
        private ApplicationStatusProcessor _applicationStatusProcessor;
        private Mock<IApplicationStatusUpdateStrategy> _applicationStatusUpdateStrategy;
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadMock;
        private Mock<ICandidateReadRepository> _candidateReadMock;
        private Mock<ITraineeshipApplicationReadRepository> _traineeshipApplicationReadMock;
        private Mock<ILogService> _loggerMock;
        private Mock<IServiceBus> _serviceBusMock;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationReadMock = new Mock<IApprenticeshipApplicationReadRepository>();
            _traineeshipApplicationReadMock = new Mock<ITraineeshipApplicationReadRepository>();
            _candidateReadMock = new Mock<ICandidateReadRepository>();
            _applicationStatusUpdateStrategy = new Mock<IApplicationStatusUpdateStrategy>();
            _serviceBusMock = new Mock<IServiceBus>();
            _loggerMock = new Mock<ILogService>();

            _applicationStatusProcessor = new ApplicationStatusProcessor(
                _loggerMock.Object,
                _serviceBusMock.Object,
                _apprenticeshipApplicationReadMock.Object,
                _traineeshipApplicationReadMock.Object,
                _candidateReadMock.Object,
                _applicationStatusUpdateStrategy.Object);
        }

        [Test]
        public void ShouldQueueApprenticeshipApplicationStatusSummaryForEachApplication()
        {
            //Would never get back from both app and trn but just checking right things are called
            var apprenticeshipApplicationSummaries =
                Enumerable.Repeat(new ApprenticeshipApplicationSummary
                {
                    Status = ApplicationStatuses.Draft,
                    LegacyVacancyId = 123
                }, 4);

            _apprenticeshipApplicationReadMock.Setup(
                x => x.GetApplicationSummaries(It.Is<int>(id => id == 123)))
                .Returns(apprenticeshipApplicationSummaries);

            var closingDate = DateTime.UtcNow.AddMonths(-2);

            _applicationStatusProcessor.ProcessApplicationStatuses(new VacancyStatusSummary
            {
                LegacyVacancyId = 123,
                VacancyStatus = VacancyStatuses.Expired,
                ClosingDate = closingDate
            });

            _apprenticeshipApplicationReadMock.Verify(
                x =>
                    x.GetApplicationSummaries(
                        It.Is<int>(id => id == 123)), Times.Once);

            _serviceBusMock.Verify(
                x =>
                    x.PublishMessage(
                        It.Is<ApplicationStatusSummary>(
                            ass =>
                                (ass.LegacyVacancyId == 123) && ass.ClosingDate == closingDate &&
                                ass.VacancyStatus == VacancyStatuses.Expired)), Times.Exactly(4));
        }
    }
}