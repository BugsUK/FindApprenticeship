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
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ApplicationStatusAlertCommunicationTests
    {
        [Test]
        public void ShouldSendOneEmailPerCandidate()
        {
            var alerts = GetAlertCandidatesDailyDigest(2, 1);
            var drafts = GetExpiringDraftsCandidatesDailyDigest(2, 2);

            //Ensure the alerts and drafts share one candidate
            var candidateId = alerts.Keys.First();
            var draft = drafts.First();

            drafts.Remove(draft.Key);
            drafts[candidateId] = draft.Value;

            var applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();

            applicationStatusAlertRepository.Setup(x => x.GetCandidatesDailyDigest()).Returns(alerts);

            var expiringDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();

            expiringDraftRepository.Setup(r => r.GetCandidatesDailyDigest()).Returns(drafts);

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableApplicationStatusChangeAlertsViaEmail(true)
                .EnableExpiringApplicationAlertsViaEmail(true)
                .Build();

            candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(candidate);

            var userReadRepository = new Mock<IUserReadRepository>();
            var user = new UserBuilder(Guid.NewGuid()).WithStatus(UserStatuses.Active).Build();

            userReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(user);

            var serviceBus = new Mock<IServiceBus>();

            var sendDailyDigestsStrategy = new SendDailyDigestsStrategyBuilder()
                .With(applicationStatusAlertRepository)
                .With(expiringDraftRepository)
                .With(candidateReadRepository)
                .With(userReadRepository)
                .With(serviceBus)
                .Build();

            var communicationProcessor = new CommunicationProcessor(sendDailyDigestsStrategy, null);

            var batchId = Guid.NewGuid();

            communicationProcessor.SendDailyDigests(batchId);
            serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Exactly(3));
        }

        [Test]
        public void AllowNeitherEmailNorSmsShouldNotSendMessageAndSoftDeleteAlerts()
        {
            var expiringDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();

            expiringDraftRepository.Setup(x => x.GetCandidatesDailyDigest()).Returns(GetExpiringDraftsCandidatesDailyDigest(0, 0));

            var applicationStatusAlertRepository = new Mock<IApplicationStatusAlertRepository>();

            applicationStatusAlertRepository.Setup(x => x.GetCandidatesDailyDigest()).Returns(GetAlertCandidatesDailyDigest(2, 2));

            var candidateReadRepository = new Mock<ICandidateReadRepository>();

            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableApplicationStatusChangeAlertsViaEmail(false)
                .EnableApplicationStatusChangeAlertsViaText(false)
                .EnableExpiringApplicationAlertsViaEmail(false)
                .EnableExpiringApplicationAlertsViaText(false)
                .Build();

            candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(candidate);

            var userReadRepository = new Mock<IUserReadRepository>();
            var user = new UserBuilder(candidate.EntityId).WithStatus(UserStatuses.Active).Build();

            userReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(user);

            var serviceBus = new Mock<IServiceBus>();

            var sendDailyDigestsStrategy = new SendDailyDigestsStrategyBuilder()
                .With(applicationStatusAlertRepository)
                .With(expiringDraftRepository)
                .With(candidateReadRepository)
                .With(userReadRepository)
                .With(serviceBus)
                .Build();

            var batchId = Guid.NewGuid();

            var communicationProcessor = new CommunicationProcessor(sendDailyDigestsStrategy, null);

            communicationProcessor.SendDailyDigests(batchId);

            applicationStatusAlertRepository.Verify(x => x.GetCandidatesDailyDigest(), Times.Once);
            candidateReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            userReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            applicationStatusAlertRepository.Verify(x => x.Save(It.IsAny<ApplicationStatusAlert>()), Times.Exactly(4));
        }

        private static Dictionary<Guid, List<ApplicationStatusAlert>> GetAlertCandidatesDailyDigest(int candidateCount, int alertCount)
        {
            var digest = new Dictionary<Guid, List<ApplicationStatusAlert>>();

            for (var i = 0; i < candidateCount; i++)
            {
                var candidateId = Guid.NewGuid();

                var alerts = new Fixture().Build<ApplicationStatusAlert>()
                    .With(asa => asa.CandidateId, candidateId)
                    .CreateMany(alertCount).ToList();

                digest.Add(candidateId, alerts);
            }

            return digest;
        }

        private static Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>> GetExpiringDraftsCandidatesDailyDigest(int candidateCount, int expiringDraftCount)
        {
            var digest = new Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>>();

            for (var i = 0; i < candidateCount; i++)
            {
                var candidateId = Guid.NewGuid();

                var alerts = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>()
                    .With(asa => asa.CandidateId, candidateId)
                    .CreateMany(expiringDraftCount).ToList();

                digest.Add(candidateId, alerts);
            }

            return digest;
        }
    }
}