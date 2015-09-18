namespace SFA.Apprenticeships.Application.UnitTests.ProviderUserAccount
{
    using System;
    using System.Collections.Generic;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendEmailVerificationCodeStrategyTests
    {
        private const string ValidUsername = "john.doe@example.com";
        private const string InvalidUsername = "jane.smith@example.com";
        private const string EmailVerificationCode = "123ABC";

        private Mock<ICodeGenerator> _mockCodeGenerator;
        private SendEmailVerificationCodeStrategy _strategy;

        private Mock<IProviderUserReadRepository> _mockProviderUserReadRepository;
        private Mock<IProviderUserWriteRepository> _mockProviderUserWriteRepository;
        private Mock<IProviderCommunicationService> _mockCommunicationService;

        [SetUp]
        public void SetUp()
        {
            // Code generator.
            _mockCodeGenerator = new Mock<ICodeGenerator>();

            _mockCodeGenerator.
                Setup(mock => mock.GenerateAlphaNumeric(SendEmailVerificationCodeStrategy.EmailVerificationCodeLength))
                .Returns(EmailVerificationCode);


            // Provider user read repository.
            _mockProviderUserReadRepository = new Mock<IProviderUserReadRepository>();

            // Provider user write repository.
            _mockProviderUserWriteRepository = new Mock<IProviderUserWriteRepository>();

            // Communication service.
            _mockCommunicationService = new Mock<IProviderCommunicationService>();

            // Strategy.
            _strategy = new SendEmailVerificationCodeStrategy(
                _mockProviderUserReadRepository.Object,
                _mockProviderUserWriteRepository.Object,
                _mockCodeGenerator.Object,
                _mockCommunicationService.Object);
        }

        [Test]
        public void ShouldGetProviderUser()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(ValidUsername))
                .Returns(new ProviderUser());

            // Act.
            _strategy.SendEmailVerificationCode(ValidUsername);

            // Assert.
            _mockProviderUserReadRepository.Verify(mock =>
                mock.Get(ValidUsername), Times.Once);
        }

        [Test]
        public void ShouldThrowIfProviderUserNotFound()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(InvalidUsername))
                .Returns(default(ProviderUser));

            var action = new Action(() => _strategy.SendEmailVerificationCode(InvalidUsername));

            // Act / Assert.
            action.ShouldThrow<CustomException>();
        }

        [Test]
        public void ShouldGenerateVerificationCode()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(ValidUsername))
                .Returns(new ProviderUser());

            // Act.
            _strategy.SendEmailVerificationCode(ValidUsername);

            // Assert.
            _mockCodeGenerator.Verify(mock =>
                mock.GenerateAlphaNumeric(SendEmailVerificationCodeStrategy.EmailVerificationCodeLength), Times.Once);
        }

        [Test]
        public void ShouldUpdateProviderUserWithGeneratedVerificationCode()
        {
            // Arrange.
            var providerUser = new ProviderUser();

            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(ValidUsername))
                .Returns(providerUser);

            // Act.
            _strategy.SendEmailVerificationCode(ValidUsername);

            // Assert.
            _mockProviderUserWriteRepository.Verify(mock =>
                mock.Save(providerUser), Times.Once);

            providerUser.EmailVerificationCode.Should().Be(EmailVerificationCode);
        }

        [Test]
        public void ShouldSendCommunicationRequest()
        {
            // Arrange.
            _mockProviderUserReadRepository
                .Setup(mock => mock.Get(ValidUsername))
                .Returns(new ProviderUser());

            var communicationTokens = default(IEnumerable<CommunicationToken>);

            _mockCommunicationService.Setup(mock =>
                mock.SendMessageToProviderUser(
                    It.IsAny<string>(),
                    It.IsAny<MessageTypes>(),
                    It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<string, MessageTypes, IEnumerable<CommunicationToken>>(
                    (u, m, t) => communicationTokens = t);

            // Act.
            _strategy.SendEmailVerificationCode(ValidUsername);

            // Assert.
            _mockCommunicationService.Verify(mock =>
                mock.SendMessageToProviderUser(
                    ValidUsername,
                    MessageTypes.SendProviderUserEmailVerificationCode,
                    It.IsAny<IEnumerable<CommunicationToken>>()),
                    Times.Once);

            communicationTokens.ShouldAllBeEquivalentTo(new[]
            {
                new CommunicationToken(CommunicationTokens.ProviderUserEmailVerificationCode, EmailVerificationCode)
            });
        }
    }
}
