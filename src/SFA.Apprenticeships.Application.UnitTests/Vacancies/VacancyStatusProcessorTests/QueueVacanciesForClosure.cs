namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class QueueVacanciesForClosure
    {
        private IVacancyStatusProcessor _processor;
        private Mock<IVacancyReadRepository> _apprenticeshipVacancyReadRepository;
        private Mock<IVacancyWriteRepository> _apprenticeshipVacancyWriteRepository;
        private Mock<IServiceBus> _serviceBus;
        private Mock<ILogService> _logService;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
            _apprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
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
            int outParam;

            var result = new Fixture().Build<Vacancy>()
                .With(x => x.Status, VacancyStatus.Live)
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
