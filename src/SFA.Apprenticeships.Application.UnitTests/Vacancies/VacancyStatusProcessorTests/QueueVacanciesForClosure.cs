namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using System;
    using System.Linq;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Queries;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class QueueVacanciesForClosure
    {
        private IVacancyStatusProcessor _processor;
        private Mock<IApprenticeshipVacancyReadRepository> _apprenticeshipVacancyReadRepository;
        private Mock<IApprenticeshipVacancyWriteRepository> _apprenticeshipVacancyWriteRepository;
        private Mock<IServiceBus> _serviceBus;
        private Mock<ILogService> _logService;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            _apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
            _serviceBus = new Mock<IServiceBus>();
            _logService = new Mock<ILogService>();

            _processor = new VacancyStatusProcessorBuilder()
                .With(_apprenticeshipVacancyReadRepository)
                .With(_apprenticeshipVacancyWriteRepository)
                .With(_serviceBus)
                .With(_logService)
                .Build();
        }

        [TestCase(10)]
        [TestCase(1000)]
        public void QueuesMultipleVacancies(int capacity)
        {
            //Arrange
            var deadline = DateTime.Now;
            var outParam = It.IsAny<int>();
            var result = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.Status, ProviderVacancyStatuses.Live)
                .CreateMany(capacity).ToList();
            _apprenticeshipVacancyReadRepository
                .Setup(m => m.Find(It.Is<ApprenticeshipVacancyQuery>(q => q.LatestClosingDate == deadline), out outParam))
                .Returns(result);

            //Act
            _processor.QueueVacanciesForClosure(deadline);

            //Assert
            _apprenticeshipVacancyReadRepository.Verify(
                m => m.Find(It.Is<ApprenticeshipVacancyQuery>(q => q.LatestClosingDate == deadline), out outParam),
                Times.Once);
            _apprenticeshipVacancyReadRepository.Verify(m => m.Find(It.IsAny<ApprenticeshipVacancyQuery>(), out outParam), Times.Once);
            _serviceBus.Verify(m => m.PublishMessage(It.IsAny<VacancyEligibleForClosure>()), Times.Exactly(capacity));
        }
    }
}
