using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Domain.Entities.Raa.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.ViewModels.Provider;
    using Recruit.Mediators.ProviderUser;

    [TestFixture]
    [Parallelizable]
    public class VerifyEmailAddressTests : TestBase
    {
        private const string UserName = "userName";

        [TestCase("invalidcode")]
        [TestCase("^&%^$&")]
        [TestCase("ABCDE^")]
        public void ValidationFailsTest(string code)
        {
            // Arrange.
            var mediator = GetMediator();
            var verifyEmailModel = new VerifyEmailViewModel{ VerificationCode = code};

            // Act.
            var response = mediator.VerifyEmailAddress(UserName, verifyEmailModel);

            // Assert.
            response.Should().NotBeNull();
            response.ValidationResult.IsValid.Should().BeFalse();
            response.Code.Should().Be(ProviderUserMediatorCodes.VerifyEmailAddress.FailedValidation);
        }

        [TestCase("ABC123")]
        [TestCase("zxcv34")]
        [TestCase("123456")]
        public void InvalidCodeTest(string code)
        {
            // Arrange.
            MockProviderUserProvider.Setup(x => x.ValidateEmailVerificationCode(UserName, code)).Returns(false);
            var mediator = GetMediator();
            var verifyEmailModel = new VerifyEmailViewModel { VerificationCode = code };

            // Act.
            var response = mediator.VerifyEmailAddress(UserName, verifyEmailModel);

            // Assert.
            response.Should().NotBeNull();
            response.ValidationResult.Should().BeNull();
            response.Code.Should().Be(ProviderUserMediatorCodes.VerifyEmailAddress.InvalidCode);
        }

        [Test]
        public void OnBoardingCompleteAndNotMigratedTest()
        {
            // Arrange.
            var mockUser = new Fixture().Build<ProviderUser>().Create();
            var providerSetToUseFAA = new Fixture().Build<ProviderViewModel>().With(u => u.IsMigrated, false).Create();
            MockProviderUserProvider.Setup(x => x.GetProviderUser(It.IsAny<string>())).Returns(mockUser);
            MockProviderUserProvider.Setup(x => x.ValidateEmailVerificationCode(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            MockProviderProvider.Setup(x => x.GetProviderViewModel(It.IsAny<int>())).Returns(providerSetToUseFAA);
            var mediator = GetMediator();
            var verifyEmailModel = new VerifyEmailViewModel { VerificationCode = "ABC123" };

            // Act.
            var response = mediator.VerifyEmailAddress(UserName, verifyEmailModel);

            // Assert.
            response.Code.Should().Be(ProviderUserMediatorCodes.VerifyEmailAddress.OkNotYetMigrated);
        }

        [TestCase("ABC123")]
        [TestCase("zxcv34")]
        [TestCase("123456")]
        public void ValidCodeTest(string code)
        {
            // Arrange.
            var providerSetToUseFAA = new Fixture().Build<ProviderViewModel>().With(u => u.IsMigrated, true).Create();
            var mockUser = new Fixture().Build<ProviderUser>().Create();
            MockProviderUserProvider.Setup(x => x.ValidateEmailVerificationCode(UserName, code)).Returns(true);
            MockProviderUserProvider.Setup(x => x.GetProviderUser(UserName)).Returns(mockUser);
            MockProviderProvider.Setup(x => x.GetProviderViewModel(It.IsAny<int>())).Returns(providerSetToUseFAA);
            var mediator = GetMediator();
            var verifyEmailModel = new VerifyEmailViewModel { VerificationCode = code };

            // Act.
            var response = mediator.VerifyEmailAddress(UserName, verifyEmailModel);
            
            // Assert.
            response.Should().NotBeNull();
            response.ValidationResult.Should().BeNull();
            response.Code.Should().Be(ProviderUserMediatorCodes.VerifyEmailAddress.Ok);
        }
    }
}
