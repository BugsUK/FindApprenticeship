namespace SFA.Apprenticeships.Application.UnitTests.Communications
{
    using System;
    using System.Collections.Generic;
    using Application.Communications;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;

    public class CommunicationProcessorBuilder
    {
        private Mock<IExpiringApprenticeshipApplicationDraftRepository> _expiringApprenticeshipApplicationDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();
        private Mock<IApplicationStatusAlertRepository> _applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
        private Mock<ISavedSearchAlertRepository> _savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();

        public CommunicationProcessorBuilder()
        {
            _expiringApprenticeshipApplicationDraftRepository.Setup(r => r.GetCandidatesDailyDigest()).Returns(new Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>>());
            _applicationStatusAlertRepository.Setup(r => r.GetCandidatesDailyDigest()).Returns(new Dictionary<Guid, List<ApplicationStatusAlert>>());
            _savedSearchAlertRepository.Setup(r => r.GetCandidatesSavedSearchAlerts()).Returns(new Dictionary<Guid, List<SavedSearchAlert>>());
        }

        public CommunicationProcessorBuilder With(Mock<IExpiringApprenticeshipApplicationDraftRepository> expiringApprenticeshipApplicationDraftRepository)
        {
            _expiringApprenticeshipApplicationDraftRepository = expiringApprenticeshipApplicationDraftRepository;
            return this;
        }

        public CommunicationProcessorBuilder With(Mock<IApplicationStatusAlertRepository> applicationStatusAlertRepository)
        {
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            return this;
        }

        public CommunicationProcessorBuilder With(Mock<ISavedSearchAlertRepository> savedSearchAlertRepository)
        {
            _savedSearchAlertRepository = savedSearchAlertRepository;
            return this;
        }

        public CommunicationProcessorBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public CommunicationProcessorBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public CommunicationProcessorBuilder With(Mock<IMessageBus> messageBus)
        {
            _messageBus = messageBus;
            return this;
        }

        public CommunicationProcessor Build()
        {
            var processor = new CommunicationProcessor(
                _expiringApprenticeshipApplicationDraftRepository.Object, _applicationStatusAlertRepository.Object, _savedSearchAlertRepository.Object, _candidateReadRepository.Object, _userReadRepository.Object, _messageBus.Object);
 
            return processor;
        }
    }
}