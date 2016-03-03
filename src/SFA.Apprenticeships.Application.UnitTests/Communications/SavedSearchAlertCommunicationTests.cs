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
    public class SavedSearchAlertCommunicationTests
    {
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void AllowEmailOrSmsShouldSendMessageAndUpdate(bool allowEmail, bool allowMobile)
        {
            var savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();

            savedSearchAlertRepository.Setup(x => x.GetCandidatesSavedSearchAlerts()).Returns(GetSavedSearchAlerts(2, 2));

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableSavedSearchAlertsViaEmail(allowEmail)
                .EnableSavedSearchAlertsViaText(allowMobile)
                .VerifiedMobile(allowMobile)
                .Build();

            candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(candidate);

            var userReadRepository = new Mock<IUserReadRepository>();
            var user = new UserBuilder(candidate.EntityId).WithStatus(UserStatuses.Active).Build();

            userReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(user);

            var serviceBus = new Mock<IServiceBus>();

            var sendSavedSearchAlertsStrategy = new SendSavedSearchAlertsStrategyBuilder()
                .With(candidateReadRepository)
                .With(userReadRepository)
                .With(savedSearchAlertRepository)
                .With(serviceBus)
                .Build();

            var communicationProcessor = new CommunicationProcessor(null, sendSavedSearchAlertsStrategy);
            var batchId = Guid.NewGuid();

            communicationProcessor.SendSavedSearchAlerts(batchId);

            savedSearchAlertRepository.Verify(x => x.GetCandidatesSavedSearchAlerts(), Times.Once);
            candidateReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            userReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            savedSearchAlertRepository.Verify(x => x.Delete(It.IsAny<SavedSearchAlert>()), Times.Never);
            savedSearchAlertRepository.Verify(x => x.Save(It.IsAny<SavedSearchAlert>()), Times.Exactly(4));
            serviceBus.Verify(x => x.PublishMessage(It.IsAny<CommunicationRequest>()), Times.Exactly(2));
        }

        [Test]
        public void AllowNeitherEmailNorSmsShouldNotSendMessageAndSoftDeleteAlerts()
        {
            var savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();

            savedSearchAlertRepository.Setup(x => x.GetCandidatesSavedSearchAlerts()).Returns(GetSavedSearchAlerts(2, 2));

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableSavedSearchAlertsViaEmail(false)
                .EnableSavedSearchAlertsViaText(false)
                .Build();

            candidateReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(candidate);

            var userReadRepository = new Mock<IUserReadRepository>();
            var user = new UserBuilder(candidate.EntityId).WithStatus(UserStatuses.Active).Build();

            userReadRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(user);

            var sendSavedSearchAlertsStrategy = new SendSavedSearchAlertsStrategyBuilder()
                .With(savedSearchAlertRepository)
                .With(candidateReadRepository)
                .With(userReadRepository)
                .Build();

            var communicationProcessor = new CommunicationProcessor(null, sendSavedSearchAlertsStrategy);
            var batchId = Guid.NewGuid();

            communicationProcessor.SendSavedSearchAlerts(batchId);

            savedSearchAlertRepository.Verify(x => x.GetCandidatesSavedSearchAlerts(), Times.Once);
            candidateReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            userReadRepository.Verify(x => x.Get(It.IsAny<Guid>()), Times.Exactly(2));
            savedSearchAlertRepository.Verify(x => x.Save(It.IsAny<SavedSearchAlert>()), Times.Exactly(4));
        }

        #region Helpers

        private static Dictionary<Guid, List<SavedSearchAlert>> GetSavedSearchAlerts(int candidateCount, int alertCount)
        {
            var allSavedSearchAlerts = new Dictionary<Guid, List<SavedSearchAlert>>();

            for (var i = 0; i < candidateCount; i++)
            {
                var candidateId = Guid.NewGuid();

                var candidateSavedSearchAlerts = new Fixture()
                    .Build<SavedSearchAlert>()
                    .CreateMany(alertCount)
                    .ToList();

                allSavedSearchAlerts.Add(candidateId, candidateSavedSearchAlerts);
            }

            return allSavedSearchAlerts;
        }

        #endregion
    }
}