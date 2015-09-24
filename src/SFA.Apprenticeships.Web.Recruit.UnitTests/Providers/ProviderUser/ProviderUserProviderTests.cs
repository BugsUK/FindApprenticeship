namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.ProviderUser
{
    using Application.Interfaces.Users;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProviderUserProviderTests
    {
        [Test]
        public void ValidateEmailVerificationCode_CaseInsensitive()
        {
            const string username = "testUser";

            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetProviderUser(username)).Returns(new ProviderUser {EmailVerificationCode = "ABC123"});
            var provider = new ProviderUserProviderBuilder().With(userProfileService).Build();

            var result = provider.ValidateEmailVerificationCode(username, "aBc123");

            result.Should().BeTrue();
        }
    }
}