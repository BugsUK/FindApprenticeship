namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.AgencyUser
{
    using Application.Interfaces.Users;
    using Builders;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Users;
    using Factory;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AgencyUserProviderTests
    {
        private const string Username = "user@agency.com";
        private const string RoleList = "Agency";

        [Test]
        public void GetOrCreateAgencyUser_Existing()
        {
            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetAgencyUser(Username)).Returns(new AgencyUserBuilder(Username).WithRegionalTeam(RegionalTeam.NorthWest).WithRole(RoleListFactory.GetRole("Technical_advisor", "Technical advisor")).Build);
            userProfileService.Setup(s => s.SaveUser(It.IsAny<AgencyUser>())).Returns<AgencyUser>(u => u);
            userProfileService.Setup(s => s.GetRoles()).Returns(RoleListFactory.GetRoleList(RoleList));
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(Username);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(Username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Never);
            viewModel.RegionalTeam.Should().Be(RegionalTeam.NorthWest);
            viewModel.RoleId.Should().Be("Technical_advisor");
            viewModel.RegionalTeams.Should().NotBeNullOrEmpty();
            viewModel.Roles.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetOrCreateAgencyUser_Create()
        {
            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetAgencyUser(Username)).Returns((AgencyUser) null);
            userProfileService.Setup(s => s.SaveUser(It.IsAny<AgencyUser>())).Returns<AgencyUser>(u => u);
            userProfileService.Setup(s => s.GetRoles()).Returns(RoleListFactory.GetRoleList(RoleList));
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(Username);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(Username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Once);
            viewModel.RegionalTeam.Should().Be(RegionalTeam.North);
            viewModel.RoleId.Should().Be("QA_advisor");
            viewModel.RegionalTeams.Should().NotBeNullOrEmpty();
            viewModel.Roles.Should().NotBeNullOrEmpty();
        }
    }
}