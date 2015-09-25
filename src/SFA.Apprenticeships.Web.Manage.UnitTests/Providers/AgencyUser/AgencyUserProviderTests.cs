namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.AgencyUser
{
    using Application.Interfaces.Users;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AgencyUserProviderTests
    {
        [Test]
        public void GetOrCreateAgencyUser_Existing()
        {
            const string username = "user@agency.com";

            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetAgencyUser(username)).Returns(new AgencyUser {Username = username});
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(username);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Never);
        }

        [Test]
        public void GetOrCreateAgencyUser_Create()
        {
            const string username = "user@agency.com";

            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetAgencyUser(username)).Returns((AgencyUser) null);
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(username);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Once);
        }
    }
}