namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Mediators.ProviderUser;
    using ViewModels.ProviderUser;

    [TestFixture]
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

        [TestCase("ABC123")]
        [TestCase("zxcv34")]
        [TestCase("123456")]
        public void ValidCodeTest(string code)
        {
            // Arrange.
            MockProviderUserProvider.Setup(x => x.ValidateEmailVerificationCode(UserName, code)).Returns(true);
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
