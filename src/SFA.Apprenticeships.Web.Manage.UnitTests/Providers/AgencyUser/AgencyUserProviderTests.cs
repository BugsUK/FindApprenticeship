namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.AgencyUser
{
    using Application.Interfaces.Users;
    using Builders;
    using Domain.Entities.Users;
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
            userProfileService.Setup(s => s.GetAgencyUser(Username)).Returns(new AgencyUserBuilder(Username).WithTeam(TeamListFactory.GetTeam("Team1", "Team 1")).WithRole(RoleListFactory.GetRole(Role.CodeNameTechnicalAdviser, "Technical adviser")).Build);
            userProfileService.Setup(s => s.SaveUser(It.IsAny<AgencyUser>())).Returns<AgencyUser>(u => u);
            userProfileService.Setup(s => s.GetTeams()).Returns(TeamListFactory.GetTeams());
            userProfileService.Setup(s => s.GetRoles(RoleList)).Returns(RoleListFactory.GetRoleList(RoleList));
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(Username, RoleList);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(Username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Never);
            viewModel.TeamCode.Should().Be("Team1");
            viewModel.RoleCode.Should().Be(Role.CodeNameTechnicalAdviser);
            viewModel.Teams.Should().NotBeNullOrEmpty();
            viewModel.Roles.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetOrCreateAgencyUser_Create()
        {
            var userProfileService = new Mock<IUserProfileService>();
            userProfileService.Setup(s => s.GetAgencyUser(Username)).Returns((AgencyUser) null);
            userProfileService.Setup(s => s.SaveUser(It.IsAny<AgencyUser>())).Returns<AgencyUser>(u => u);
            userProfileService.Setup(s => s.GetTeams()).Returns(TeamListFactory.GetTeams());
            userProfileService.Setup(s => s.GetRoles(RoleList)).Returns(RoleListFactory.GetRoleList(RoleList));
            var provider = new AgencyUserProviderBuilder().With(userProfileService).Build();

            var viewModel = provider.GetOrCreateAgencyUser(Username, RoleList);

            viewModel.Should().NotBeNull();
            userProfileService.Verify(s => s.GetAgencyUser(Username), Times.Once);
            userProfileService.Verify(s => s.SaveUser(It.IsAny<AgencyUser>()), Times.Once);
            viewModel.TeamCode.Should().Be("All");
            viewModel.RoleCode.Should().Be(Role.CodeNameQAAdviser);
            viewModel.Teams.Should().NotBeNullOrEmpty();
            viewModel.Roles.Should().NotBeNullOrEmpty();
        }
    }
}