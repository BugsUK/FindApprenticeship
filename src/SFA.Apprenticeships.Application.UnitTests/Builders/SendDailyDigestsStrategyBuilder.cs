namespace SFA.Apprenticeships.Application.UnitTests.Builders
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Communications.Strategies;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class SendDailyDigestsStrategyBuilder
    {
        private Mock<ILogService> _logService = new Mock<ILogService>();
        private Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();

        private Mock<IExpiringApprenticeshipApplicationDraftRepository> _expiringApprenticeshipApplicationDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();
        private Mock<IApplicationStatusAlertRepository> _applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();

        public SendDailyDigestsStrategyBuilder()
        {
            _expiringApprenticeshipApplicationDraftRepository.Setup(r => r.GetCandidatesDailyDigest()).Returns(new Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>>());
            _applicationStatusAlertRepository.Setup(r => r.GetCandidatesDailyDigest()).Returns(new Dictionary<Guid, List<ApplicationStatusAlert>>());
        }

        public SendDailyDigestsStrategyBuilder With(Mock<IExpiringApprenticeshipApplicationDraftRepository> expiringApprenticeshipApplicationDraftRepository)
        {
            _expiringApprenticeshipApplicationDraftRepository = expiringApprenticeshipApplicationDraftRepository;
            return this;
        }

        public SendDailyDigestsStrategyBuilder With(Mock<IApplicationStatusAlertRepository> applicationStatusAlertRepository)
        {
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            return this;
        }

        public SendDailyDigestsStrategyBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public SendDailyDigestsStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public SendDailyDigestsStrategyBuilder With(Mock<IServiceBus> serviceBus)
        {
            _serviceBus = serviceBus;
            return this;
        }

        public SendDailyDigestsStrategy Build()
        {
            return new SendDailyDigestsStrategy(
                _logService.Object,
                _serviceBus.Object,
                _expiringApprenticeshipApplicationDraftRepository.Object,
                _applicationStatusAlertRepository.Object,
                _candidateReadRepository.Object,
                _userReadRepository.Object);
        }
    }
}
