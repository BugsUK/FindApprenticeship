namespace SFA.Apprenticeships.Application.UnitTests.ProviderUserAccount
{
    using System;
    using System.Collections.Generic;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Users;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResendEmailVerificationCodeStrategyTests
    {
        private const string UnverifiedUsername = "john.doe@example.com";
        private const string VerifiedUsername = "bob.lobb@example.com";
        private const string InvalidUsername = "jane.smith@example.com";
        private const string EmailVerificationCode = "123ABC";

        private ResendEmailVerificationCodeStrategy _strategy;

        private Mock<ILogService> _logService;
        private Mock<IProviderUserReadRepository> _mockProviderUserReadRepository;
        private Mock<IProviderCommunicationService> _mockCommunicationService;

        [SetUp]
        public void SetUp()
        {
            // Log service.
            _logService = new Mock<ILogService>();

            // Provider user read repository.
            _mockProviderUserReadRepository = new Mock<IProviderUserReadRepository>();

            // Communication service.
            _mockCommunicationService = new Mock<IProviderCommunicationService>();

            // Strategy.
            _strategy = new ResendEmailVerificationCodeStrategy(
                _logService.Object,
                _mockProviderUserReadRepository.Object,
                _mockCommunicationService.Object);
        }

        [Test]
        public void ShouldGetProviderUser()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(UnverifiedUsername))
                .Returns(new ProviderUser());

            // Act.
            _strategy.ResendEmailVerificationCode(UnverifiedUsername);

            // Assert.
            _mockProviderUserReadRepository.Verify(mock =>
                mock.Get(UnverifiedUsername), Times.Once);
        }

        [Test]
        public void ShouldThrowIfProviderUserNotFound()
        {
            // Arrange.
            var action = new Action(() => _strategy.ResendEmailVerificationCode(InvalidUsername));

            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(InvalidUsername))
                .Returns(default(ProviderUser));

            // Act / Assert.
            action.ShouldThrow<CustomException>();
        }

        [Test]
        public void ShouldSendCommunicationRequest()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(UnverifiedUsername))
                .Returns(new ProviderUser
                {
                    Username = UnverifiedUsername,
                    Status = ProviderUserStatus.Registered,
                    EmailVerificationCode = EmailVerificationCode
                });

            var communicationTokens = default(IEnumerable<CommunicationToken>);

            _mockCommunicationService.Setup(mock =>
                mock.SendMessageToProviderUser(
                    It.IsAny<string>(),
                    It.IsAny<MessageTypes>(),
                    It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<string, MessageTypes, IEnumerable<CommunicationToken>>(
                    (u, m, t) => communicationTokens = t);

            // Act.
            _strategy.ResendEmailVerificationCode(UnverifiedUsername);

            // Assert.
            _mockCommunicationService.Verify(mock =>
                mock.SendMessageToProviderUser(
                    UnverifiedUsername,
                    MessageTypes.SendProviderUserEmailVerificationCode,
                    It.IsAny<IEnumerable<CommunicationToken>>()),
                    Times.Once);

            communicationTokens.ShouldAllBeEquivalentTo(new[]
            {
                new CommunicationToken(CommunicationTokens.ProviderUserUsername, UnverifiedUsername),
                new CommunicationToken(CommunicationTokens.ProviderUserEmailVerificationCode, EmailVerificationCode)
            });
        }

        [Test]
        public void ShouldNotSendCommunicationRequestWhenProviderUserIsVerified()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(VerifiedUsername))
                .Returns(new ProviderUser
                {
                    Status = ProviderUserStatus.EmailVerified
                });

            // Act.
            _strategy.ResendEmailVerificationCode(VerifiedUsername);

            // Assert.
            _mockCommunicationService.Verify(mock =>
                mock.SendMessageToProviderUser(
                    It.IsAny<string>(),
                    It.IsAny<MessageTypes>(),
                    It.IsAny<IEnumerable<CommunicationToken>>()),
                Times.Never);

            _logService.Verify(mock => mock.Info(It.IsAny<string>()), Times.Once);
        }
    }
}
