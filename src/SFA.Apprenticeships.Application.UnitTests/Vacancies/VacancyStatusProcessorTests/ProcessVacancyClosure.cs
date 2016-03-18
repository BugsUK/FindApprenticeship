namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ProcessVacancyClosure
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
        
        [Test]
        public void UpdatesLiveStatusToClosed()
        {
            //Arrange
            var message = new VacancyEligibleForClosure(42);
            var liveVacancy = new Fixture().Build<Vacancy>()
                .With(x => x.Status, VacancyStatus.Live)
                .With(x => x.VacancyId, message.VacancyId)
                .Create();

            _apprenticeshipVacancyReadRepository.Setup(m => m.Get(message.VacancyId)).Returns(liveVacancy);
            
            //Act
            _processor.ProcessVacancyClosure(message);

            //Assert
            _apprenticeshipVacancyReadRepository.Verify(m => m.Get(message.VacancyId), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.Update(It.Is<Vacancy>(av => av.VacancyId == message.VacancyId)), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.Update(It.Is<Vacancy>(av => av.Status == VacancyStatus.Closed)), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.Update(It.IsAny<Vacancy>()), Times.Once);
        }

        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.Submitted)]
        [TestCase(VacancyStatus.Referred)]
        [TestCase(VacancyStatus.ReservedForQA)]
        [TestCase(VacancyStatus.Unknown)]
        public void DoNothingWithVacancyThatIsNotLive(VacancyStatus status)
        {
            //Arrange
            var message = new VacancyEligibleForClosure(42);
            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.Status, status)
                .With(x => x.VacancyId, message.VacancyId)
                .Create();

            _apprenticeshipVacancyReadRepository.Setup(m => m.Get(message.VacancyId)).Returns(vacancy);

            //Act
            _processor.ProcessVacancyClosure(message);

            //Assert
            _apprenticeshipVacancyReadRepository.Verify(m => m.Get(message.VacancyId), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.Update(It.IsAny<Vacancy>()), Times.Never());
        }
    }
}
