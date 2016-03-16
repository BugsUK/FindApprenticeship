namespace SFA.Apprenticeships.Application.UnitTests.UserAccount
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.UserAccount.Configuration;
    using Apprenticeships.Application.UserAccount.Strategies;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class SendAccountUnlockTests
    {
        [Test]
        public void RenewAccountUnlockCode()
        {
            var lockUserStrategy = new Mock<ILockUserStrategy>();
            var communicationService = new Mock<ICommunicationService>();

            var strategy =
                new SendAccountUnlockCodeStrategyBuilder().With(lockUserStrategy)
                    .With(communicationService)
                    .WithUnlockCodeExpiration(DateTime.UtcNow.AddDays(-1))
                    .Build();

            strategy.SendAccountUnlockCode("Username");

            lockUserStrategy.Verify(lus => lus.LockUser(It.IsAny<User>()), Times.Once);
            communicationService.Verify(
                cs =>
                    cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendAccountUnlockCode,
                        It.IsAny<IEnumerable<CommunicationToken>>()));
        }

        [Test]
        public void SendCode()
        {
            var lockUserStrategy = new Mock<ILockUserStrategy>();
            var communicationService = new Mock<ICommunicationService>();

            var strategy =
                new SendAccountUnlockCodeStrategyBuilder().With(lockUserStrategy)
                    .With(communicationService)
                    .Build();

            strategy.SendAccountUnlockCode("Username");

            lockUserStrategy.Verify(lus => lus.LockUser(It.IsAny<User>()), Times.Never);
            communicationService.Verify(
                cs =>
                    cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendAccountUnlockCode,
                        It.IsAny<IEnumerable<CommunicationToken>>()));
        }
    }

    public class SendAccountUnlockCodeStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private readonly Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<ILockUserStrategy> _lockUserStrategy = new Mock<ILockUserStrategy>();
        private Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private DateTime _unlockCodeExpiration = DateTime.UtcNow.AddDays(1);

        public SendAccountUnlockCodeStrategyBuilder With(Mock<ILockUserStrategy> lockUserStrategy)
        {
            _lockUserStrategy = lockUserStrategy;
            return this;
        }

        public SendAccountUnlockCodeStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }

        public SendAccountUnlockCodeStrategyBuilder WithUnlockCodeExpiration(DateTime expireation)
        {
            _unlockCodeExpiration = expireation;
            return this;
        }

        public SendAccountUnlockCodeStrategy Build()
        {
            _userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(), true)).Returns(new User
            {
                Status = UserStatuses.Locked,
                AccountUnlockCodeExpiry = _unlockCodeExpiration,
            });

            _candidateReadRepository.Setup(crr => crr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                RegistrationDetails = new RegistrationDetails()
            });

            _configurationService.Setup(
                x => x.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration() {UnlockCodeExpiryDays = 30});

            return new SendAccountUnlockCodeStrategy(_configurationService.Object, _userReadRepository.Object,
                _candidateReadRepository.Object, _lockUserStrategy.Object, _communicationService.Object);
        }
    }
}