namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.SavedSearchProcessorTests
{
    using Apprenticeships.Application.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Locations;
    using Infrastructure.Interfaces;
    using Interfaces.Vacancies;
    using Moq;
    using Vacancy;

    public class SavedSearchProcessorBuilder
    {
        private Mock<ISavedSearchReadRepository> _savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
        private Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<ILocationSearchService> _locationSearchService = new Mock<ILocationSearchService>();
        private Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>> _vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
        private Mock<ISavedSearchAlertRepository> _savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
        private Mock<ISavedSearchWriteRepository> _savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public ISavedSearchProcessor Build()
        {
            var processor = new SavedSearchProcessor(
                _savedSearchReadRepository.Object,
                _serviceBus.Object,
                _userReadRepository.Object,
                _candidateReadRepository.Object,
                _locationSearchService.Object,
                _vacancySearchProvider.Object,
                _savedSearchAlertRepository.Object,
                _savedSearchWriteRepository.Object,
                _logService.Object);

            return processor;
        }

        public SavedSearchProcessorBuilder With(Mock<ISavedSearchReadRepository> savedSearchReadRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<IServiceBus> serviceBus)
        {
            _serviceBus = serviceBus;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<ILocationSearchService> locationSearchService)
        {
            _locationSearchService = locationSearchService;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>> vacancySearchProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<ISavedSearchAlertRepository> savedSearchAlertRepository)
        {
            _savedSearchAlertRepository = savedSearchAlertRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<ISavedSearchWriteRepository> savedSearchWriteRepository)
        {
            _savedSearchWriteRepository = savedSearchWriteRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<ILogService> logService)
        {
            _logService = logService;
            return this;
        }
    }
}