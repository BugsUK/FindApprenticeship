using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Mediators.ProviderUser;

    [TestFixture]
    public class UpdateUserTests : TestBase
    {
        [Test]
        public void ValidationFailsTest()
        {
            // Arrange.
            var mediator = GetMediator();
            var user = new ProviderUserViewModel {EmailAddress = ""};

            // Act.
            var response = mediator.UpdateUser("userName", "00001", user);

            // Assert.
            response.Should().NotBeNull();
            response.ValidationResult.IsValid.Should().BeFalse();
            response.Code.Should().Be(ProviderUserMediatorCodes.UpdateUser.FailedValidation);
        }

        [Test]
        public void ValidationSuccessfulTest()
        {
            // Arrange.
            var mediator = GetMediator();
            var user = new ProviderUserViewModel { Fullname = "Full name", EmailAddress = "asdf@asdf.com", PhoneNumber = "0321321321", DefaultProviderSiteId = 12345678};

            // Act.
            var response = mediator.UpdateUser("userName", "00001", user);

            // Assert.
            response.Should().NotBeNull();
            response.ValidationResult.Should().BeNull();
            response.Code.Should().Be(ProviderUserMediatorCodes.UpdateUser.Ok);
        }
    }
}
