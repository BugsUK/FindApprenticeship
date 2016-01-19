namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using Application.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Moq;

    public class VacancyStatusProcessorBuilder
    {
        private Mock<IApprenticeshipVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
        private Mock<IApprenticeshipVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
        private Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public IVacancyStatusProcessor Build()
        {
            var processor = new VacancyStatusProcessor(
                _apprenticeshipVacancyReadRepository.Object,
                _apprenticeshipVacancyWriteRepository.Object,
                _serviceBus.Object,
                _logService.Object
            );

            return processor;
        }

        public VacancyStatusProcessorBuilder With(Mock<IApprenticeshipVacancyReadRepository> mock)
        {
            _apprenticeshipVacancyReadRepository = mock;
            return this;
        }

        public VacancyStatusProcessorBuilder With(Mock<IApprenticeshipVacancyWriteRepository> mock)
        {
            _apprenticeshipVacancyWriteRepository = mock;
            return this;
        }

        public VacancyStatusProcessorBuilder With(Mock<IServiceBus> serviceBus)
        {
            _serviceBus = serviceBus;
            return this;
        }

        public VacancyStatusProcessorBuilder With(Mock<ILogService> mock)
        {
            _logService = mock;
            return this;
        }
    }
}
