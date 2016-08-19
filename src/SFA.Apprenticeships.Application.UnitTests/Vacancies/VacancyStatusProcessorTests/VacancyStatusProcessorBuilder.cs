namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.VacancyStatusProcessorTests
{
    using Apprenticeships.Application.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    public class VacancyStatusProcessorBuilder
    {
        private Mock<IVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private Mock<IVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
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

        public VacancyStatusProcessorBuilder With(Mock<IVacancyReadRepository> mock)
        {
            _apprenticeshipVacancyReadRepository = mock;
            return this;
        }

        public VacancyStatusProcessorBuilder With(Mock<IVacancyWriteRepository> mock)
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
