namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderUser
{
    using System;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Users;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.ProviderUser;

    [TestFixture]
    [Parallelizable]
    public class ProviderUserProviderTests
    {
        private Mock<IUserProfileService> _mockUserProfileService;
        private Mock<IProviderService> _mockProviderService;
        private Mock<IProviderUserAccountService> _mockProviderUserAccountService;

        [SetUp]
        public void SetUp()
        {
            _mockUserProfileService = new Mock<IUserProfileService>();
            _mockProviderService = new Mock<IProviderService>();
            _mockProviderUserAccountService = new Mock<IProviderUserAccountService>();
        }

        [TestCase("ABC123", "abc123")]
        [TestCase("abc123", "ABC123")]
        public void ShouldValidateEmailVerificationCodeCaseInsensitive(
            string savedEmailVerificationCode, string enteredEmailVerificationCode)
        {
            // Arrange.
            const string username = "a.user";

            var providerUser = new ProviderUser
            {
                EmailVerificationCode = savedEmailVerificationCode
            };

            _mockUserProfileService
                .Setup(mock => mock.GetProviderUser(username))
                .Returns(providerUser);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .Build();

            // Act.
            var result = providerUserProvider.ValidateEmailVerificationCode(username, enteredEmailVerificationCode);

            // Assert.
            result.Should().BeTrue();
        }

        [Test]
        public void ShouldUpdateUserProviderWhenEmailVerificationCodeIsValid()
        {
            // Arrange.
            const string username = "a.user";
            const string emailVerificationCode = "ABC123";

            var providerUser = new ProviderUser
            {
                Status = ProviderUserStatus.Registered,
                EmailVerificationCode = emailVerificationCode,
                EmailVerifiedDate = null
            };

            _mockUserProfileService
                .Setup(mock => mock.GetProviderUser(username))
                .Returns(providerUser);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .Build();

            // Act.
            var result = providerUserProvider.ValidateEmailVerificationCode(username, emailVerificationCode);

            // Assert.
            result.Should().BeTrue();

            _mockUserProfileService.Verify(mock =>
                mock.UpdateProviderUser(providerUser), Times.Once);

            providerUser.Status.Should().Be(ProviderUserStatus.EmailVerified);
            providerUser.EmailVerificationCode.Should().BeNull();
            providerUser.EmailVerifiedDate.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public void ShouldSaveNewProviderUser()
        {
            // Arrange.
            const string username = "new.user";

            _mockUserProfileService
                .Setup(mock => mock
                    .GetProviderUser(username))
                .Returns(default(ProviderUser));

            var newProviderUser = default(ProviderUser);

            _mockUserProfileService
                .Setup(mock => mock
                    .CreateProviderUser(It.IsAny<ProviderUser>()))
                .Returns<ProviderUser>(providerUser => providerUser)
                .Callback<ProviderUser>(providerUser =>
                {
                    newProviderUser = providerUser;
                });

            var provider = new Fixture()
                .Build<Provider>()
                .Create();

            _mockProviderService
                .Setup(mock =>
                    mock.GetProvider(provider.Ukprn))
                .Returns(provider);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .With(_mockProviderService)
                .Build();

            var originalViewModel = new Fixture()
                .Build<ProviderUserViewModel>()
                .Create();

            // Act.
            var savedViewModel = providerUserProvider.SaveProviderUser(
                username, provider.Ukprn, originalViewModel);

            // Assert: calls.
            _mockProviderService.Verify(mock =>
                mock.GetProvider(provider.Ukprn), Times.Once);

            // Assert: saved provider user.
            newProviderUser.ProviderId.Should().Be(provider.ProviderId);
            newProviderUser.ProviderUserGuid.Should().NotBe(Guid.Empty);
            newProviderUser.Status.Should().Be(ProviderUserStatus.Registered);

            // Assert: view model
            savedViewModel.Should().NotBeNull();

            savedViewModel.Fullname.Should().Be(originalViewModel.Fullname);
            savedViewModel.EmailAddress.Should().Be(originalViewModel.EmailAddress);
            savedViewModel.PhoneNumber.Should().Be(originalViewModel.PhoneNumber);
            savedViewModel.DefaultProviderSiteId.Should().Be(originalViewModel.DefaultProviderSiteId);
            savedViewModel.EmailAddressVerified.Should().Be(false);
        }

        [Test]
        public void ShouldSaveExistingProviderUser()
        {
            // Arrange.
            const string username = "existing.user";
            const string ukprn = "1000000";

            var existingProviderUser = new Fixture()
                .Build<ProviderUser>()
                .Create();

            _mockUserProfileService
                .Setup(mock => mock
                    .GetProviderUser(username))
                .Returns(existingProviderUser);

            _mockUserProfileService
                .Setup(mock => mock
                    .UpdateProviderUser(It.IsAny<ProviderUser>()))
                .Returns<ProviderUser>(providerUser => providerUser);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .With(_mockProviderService)
                .Build();

            var originalViewModel = new Fixture()
                .Build<ProviderUserViewModel>()
                .Create();

            // Act.
            var savedViewModel = providerUserProvider.SaveProviderUser(
                username, ukprn, originalViewModel);

            // Assert.
            savedViewModel.Should().NotBeNull();

            savedViewModel.Fullname.Should().Be(originalViewModel.Fullname);
            savedViewModel.EmailAddress.Should().Be(originalViewModel.EmailAddress);
            savedViewModel.PhoneNumber.Should().Be(originalViewModel.PhoneNumber);
            savedViewModel.DefaultProviderSiteId.Should().Be(originalViewModel.DefaultProviderSiteId);
            savedViewModel.EmailAddressVerified.Should().Be(false);
        }

        [Test]
        public void ShouldSendEmailVerificationCodeToNewProviderUser()
        {
            // Arrange.
            const string username = "new.user";

            _mockUserProfileService
                .Setup(mock => mock
                    .GetProviderUser(username))
                .Returns(default(ProviderUser));

            var providerUser = new Fixture()
                .Build<ProviderUser>()
                .Create();

            _mockUserProfileService
                .Setup(mock => mock
                    .CreateProviderUser(It.IsAny<ProviderUser>()))
                .Returns(providerUser);

            var provider = new Fixture()
                .Build<Provider>()
                .Create();

            _mockProviderService
                .Setup(mock =>
                    mock.GetProvider(provider.Ukprn))
                .Returns(provider);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .With(_mockProviderService)
                .With(_mockProviderUserAccountService)
                .Build();

            var originalViewModel = new Fixture()
                .Build<ProviderUserViewModel>()
                .With(each => each.EmailAddress, "john.doe@example.com")
                .Create();

            // Act.
            providerUserProvider.SaveProviderUser(
                username, provider.Ukprn, originalViewModel);

            // Assert.
            _mockProviderUserAccountService.Verify(mock =>
                mock.SendEmailVerificationCode(username), Times.Once);

        }

        [TestCase("john.doe@example.com", "jane.doe@example.com", true)]
        [TestCase("john.doe@example.com", "JANE.doe@EXAMPLE.com", true)]
        [TestCase("john.doe@example.com", "john.doe@example.com", false)]
        [TestCase("john.doe@example.com", "JOHN.doe@EXAMPLE.com", false)]
        public void ShouldSendEmailVerificationCodeToExistingProviderUserWhenEmailChanges(
            string originalEmailAddress, string newEmailAddress, bool expectToSendVerificationCodeEmail)
        {
            // Arrange.
            const string username = "existing.user";
            const string ukprn = "1000000";

            var existingProviderUser = new Fixture()
                .Build<ProviderUser>()
                .With(each => each.Email, originalEmailAddress)
                .Create();

            _mockUserProfileService
                .Setup(mock => mock
                    .GetProviderUser(username))
                .Returns(existingProviderUser);

            var providerUser = new Fixture()
                .Build<ProviderUser>()
                .With(each => each.Email, originalEmailAddress)
                .Create();

            _mockUserProfileService
                .Setup(mock => mock
                    .UpdateProviderUser(It.IsAny<ProviderUser>()))
                .Returns(providerUser);

            var providerUserProvider = new ProviderUserProviderBuilder()
                .With(_mockUserProfileService)
                .With(_mockProviderService)
                .With(_mockProviderUserAccountService)
                .Build();

            var originalViewModel = new Fixture()
                .Build<ProviderUserViewModel>()
                .With(each => each.EmailAddress, newEmailAddress)
                .Create();

            // Act.
            providerUserProvider.SaveProviderUser(
                username, ukprn, originalViewModel);

            // Assert.
            var expectedTimes = expectToSendVerificationCodeEmail ? 1 : 0;

            _mockProviderUserAccountService.Verify(mock =>
                mock.SendEmailVerificationCode(username), Times.Exactly(expectedTimes));
        }
    }
}
