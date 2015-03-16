namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.SavedSearchProcessorTests
{
    using Application.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Vacancies;
    using Moq;
    using Vacancy;

    public class SavedSearchProcessorBuilder
    {
        private Mock<ISavedSearchReadRepository> _savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
        private Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>> _vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
        private Mock<ISavedSearchAlertRepository> _savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
        private Mock<ISavedSearchWriteRepository> _savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public ISavedSearchProcessor Build()
        {
            var processor = new SavedSearchProcessor(_savedSearchReadRepository.Object, _messageBus.Object, _userReadRepository.Object, _candidateReadRepository.Object, _vacancySearchProvider.Object, _savedSearchAlertRepository.Object, _savedSearchWriteRepository.Object, _logService.Object);
            return processor;
        }

        public SavedSearchProcessorBuilder With(Mock<ISavedSearchReadRepository> savedSearchReadRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            return this;
        }

        public SavedSearchProcessorBuilder With(Mock<IMessageBus> messageBus)
        {
            _messageBus = messageBus;
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