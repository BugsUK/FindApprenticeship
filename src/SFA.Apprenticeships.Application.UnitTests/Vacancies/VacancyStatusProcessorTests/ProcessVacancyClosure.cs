namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using System;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ProcessVacancyClosure
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
        
        [Test]
        public void UpdatesLiveStatusToClosed()
        {
            //Arrange
            var vacancyId = 1;
            var message = new VacancyEligibleForClosure(vacancyId);
            var liveVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.Status, ProviderVacancyStatuses.Live)
                .With(x => x.VacancyId, message.EntityId)
                .Create();

            _apprenticeshipVacancyReadRepository.Setup(m => m.Get(message.EntityId)).Returns(liveVacancy);
            
            //Act
            _processor.ProcessVacancyClosure(message);

            //Assert
            _apprenticeshipVacancyReadRepository.Verify(m => m.Get(message.EntityId), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.DeepSave(It.Is<ApprenticeshipVacancy>(av => av.VacancyId == message.EntityId)), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.DeepSave(It.Is<ApprenticeshipVacancy>(av => av.Status == ProviderVacancyStatuses.Closed)), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.DeepSave(It.IsAny<ApprenticeshipVacancy>()), Times.Once);
        }

        [TestCase(ProviderVacancyStatuses.Closed)]
        [TestCase(ProviderVacancyStatuses.Draft)]
        [TestCase(ProviderVacancyStatuses.PendingQA)]
        [TestCase(ProviderVacancyStatuses.RejectedByQA)]
        [TestCase(ProviderVacancyStatuses.ReservedForQA)]
        [TestCase(ProviderVacancyStatuses.Unknown)]
        public void DoNothingWithVacancyThatIsNotLive(ProviderVacancyStatuses status)
        {
            //Arrange
            var message = new VacancyEligibleForClosure(1);
            var vacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.Status, status)
                .With(x => x.VacancyId, message.EntityId)
                .Create();

            _apprenticeshipVacancyReadRepository.Setup(m => m.Get(message.EntityId)).Returns(vacancy);

            //Act
            _processor.ProcessVacancyClosure(message);

            //Assert
            _apprenticeshipVacancyReadRepository.Verify(m => m.Get(message.EntityId), Times.Once);
            _apprenticeshipVacancyWriteRepository.Verify(m => m.DeepSave(It.IsAny<ApprenticeshipVacancy>()), Times.Never());
        }
    }
}
