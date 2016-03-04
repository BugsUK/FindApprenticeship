namespace SFA.Apprenticeships.Application.UnitTests.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Communications;
    using Builders;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ExpiringDraftCommunicationTests
    {
        private Mock<IExpiringApprenticeshipApplicationDraftRepository> _expiringDraftRepository;
        private Mock<IApplicationStatusAlertRepository> _applicationStatusAlertRepository;
        private Mock<ISavedSearchAlertRepository> _savedSearchAlertRepository;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<IUserReadRepository> _userReadRepository;
        private Mock<IServiceBus> _serviceBus;
        private CommunicationProcessor _communicationProcessor;

        [SetUp]
        public void SetUp()
        {
            _expiringDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();
            
            _applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
            _applicationStatusAlertRepository.Setup(r => r.GetCandidatesDailyDigest())
                .Returns(new Dictionary<Guid, List<ApplicationStatusAlert>>());

            _savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
            _savedSearchAlertRepository.Setup(r => r.GetCandidatesSavedSearchAlerts())
                .Returns(new Dictionary<Guid, List<SavedSearchAlert>>());

            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _userReadRepository = new Mock<IUserReadRepository>();
            _serviceBus = new Mock<IServiceBus>();

            var sendDailyDigestsStrategy = new SendDailyDigestsStrategyBuilder()
                .With(_applicationStatusAlertRepository)
                .With(_expiringDraftRepository)
                .With(_candidateReadRepository)
                .With(_userReadRepository)
                .With(_serviceBus)
                .Build();

            _communicationProcessor = new CommunicationProcessor(sendDailyDigestsStrategy, null);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void AllowEmailOrSmsShouldSendMessageAndUpdate(bool allowEmail, bool allowMobile)
        {
            _expiringDraftRepository.Setup(mock => mock.GetCandidatesDailyDigest()).Returns(GetDraftDigests(2, 2));
            _expiringDraftRepository.Setup(mock => mock.Delete(It.IsAny<ExpiringApprenticeshipApplicationDraft>()));
            _expiringDraftRepository.Setup(mock => mock.Save(It.IsAny<ExpiringApprenticeshipApplicationDraft>()));

            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableExpiringApplicationAlertsViaEmail(allowEmail)
                .EnableExpiringApplicationAlertsViaText(allowMobile)
                .VerifiedMobile(allowMobile)
                .Build();

            var user = new UserBuilder(candidate.EntityId)
                .WithStatus(UserStatuses.Active)
                .Build();

            _candidateReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(candidate);
            _userReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(user);

            _serviceBus.Setup(mock => mock.PublishMessage(It.IsAny<CommunicationRequest>()));

            var batchId = Guid.NewGuid();

            _communicationProcessor.SendDailyDigests(batchId);

            _candidateReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _userReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _expiringDraftRepository.Verify(mock => mock.GetCandidatesDailyDigest(), Times.Once);
            _expiringDraftRepository.Verify(mock => mock.Delete(It.IsAny<ExpiringApprenticeshipApplicationDraft>()), Times.Never);
            _expiringDraftRepository.Verify(mock => mock.Save(It.Is<ExpiringApprenticeshipApplicationDraft>(ed => ed.BatchId == batchId)), Times.Exactly(4));
            _serviceBus.Verify(mock => mock.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Exactly(2));
        }

        [Test]
        public void AllowNeitherEmailNorSmsShouldNotSendMessageAndSoftDeleteDrafts()
        {
            _expiringDraftRepository.Setup(mock => mock.GetCandidatesDailyDigest()).Returns(GetDraftDigests(2, 2));
            _expiringDraftRepository.Setup(mock => mock.Delete(It.IsAny<ExpiringApprenticeshipApplicationDraft>()));

            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableExpiringApplicationAlertsViaEmail(false)
                .EnableExpiringApplicationAlertsViaText(false)
                .VerifiedMobile(true)
                .Build();

            var user = new UserBuilder(candidate.EntityId)
                .WithStatus(UserStatuses.Active)
                .Build();

            _candidateReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(candidate);
            _userReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(user);

            var batchId = Guid.NewGuid();

            _communicationProcessor.SendDailyDigests(batchId);

            _candidateReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _userReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _expiringDraftRepository.Verify(mock => mock.GetCandidatesDailyDigest(), Times.Once);
            _expiringDraftRepository.Verify(mock => mock.Save(It.IsAny<ExpiringApprenticeshipApplicationDraft>()), Times.Exactly(4));
            _serviceBus.Verify(mock => mock.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Never);
        }

        [TestCase(UserStatuses.Inactive)]
        [TestCase(UserStatuses.Dormant)]
        public void InactiveOrDormantUserShouldNotSendMessageAndSoftDeleteDrafts(UserStatuses userStatus)
        {
            _expiringDraftRepository.Setup(mock => mock.GetCandidatesDailyDigest()).Returns(GetDraftDigests(2, 2));
            _expiringDraftRepository.Setup(mock => mock.Delete(It.IsAny<ExpiringApprenticeshipApplicationDraft>()));
            _expiringDraftRepository.Setup(mock => mock.Save(It.IsAny<ExpiringApprenticeshipApplicationDraft>()));

            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableExpiringApplicationAlertsViaEmail(true)
                .EnableExpiringApplicationAlertsViaText(true)
                .VerifiedMobile(true)
                .Build();

            var user = new UserBuilder(candidate.EntityId)
                .WithStatus(userStatus)
                .Build();

            _candidateReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(candidate);
            _userReadRepository.Setup(mock => mock.Get(It.IsAny<Guid>())).Returns(user);

            _serviceBus.Setup(mock => mock.PublishMessage(It.IsAny<CommunicationRequest>()));

            var batchId = Guid.NewGuid();
            _communicationProcessor.SendDailyDigests(batchId);

            _expiringDraftRepository.Verify(mock => mock.GetCandidatesDailyDigest(), Times.Once);
            _candidateReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _userReadRepository.Verify(mock => mock.Get(It.IsAny<Guid>()), Times.Exactly(2));
            _expiringDraftRepository.Verify(mock => mock.Save(It.IsAny<ExpiringApprenticeshipApplicationDraft>()), Times.Exactly(4));
            _serviceBus.Verify(mock => mock.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Never);
        }

        private static Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>> GetDraftDigests(int candidateCount, int expiringDraftCount)
        {
            var digest = new Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>>();

            for (var i = 0; i < candidateCount; i++)
            {
                var candidateId = Guid.NewGuid();

                var drafts = Builder<ExpiringApprenticeshipApplicationDraft>.CreateListOfSize(expiringDraftCount)
                    .All()
                    .With(ed => ed.CandidateId == candidateId)
                    .Build()
                    .ToList();

                digest.Add(candidateId, drafts);
            }

            return digest;
        }
    }
}
