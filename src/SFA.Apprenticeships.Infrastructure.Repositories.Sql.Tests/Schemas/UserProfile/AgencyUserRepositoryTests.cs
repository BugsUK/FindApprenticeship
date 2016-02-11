namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.UserProfile
{
    using System;
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.UserProfile;
    using Sql.Schemas.UserProfile.Entities;

    [TestFixture(Category = "Integration")]
    public class AgencyUserRepositoryTests
    {
        private AgencyUserRepository _repoUnderTest;
        private readonly IMapper _mapper = new AgencyUserMappers();
        private IGetOpenConnection _connection;
        private Mock<ILogService> _logger;
        private AgencyUserTeam _agencyUserTeamA;
        private AgencyUserTeam _agencyUserTeamB;
        private AgencyUserRole _agencyUserRoleA;
        private AgencyUserRole _agencyUserRoleB;
        private AgencyUser userWithBothRoleAndTeam;


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _agencyUserTeamA = new AgencyUserTeam() { AgencyUserTeamId = 1, IsDefault = 0, CodeName = "A", Name = "Team A" };
            _agencyUserTeamB = new AgencyUserTeam() { AgencyUserTeamId = 2, IsDefault = 0, CodeName = "B", Name = "Team B" };
            _agencyUserRoleA = new AgencyUserRole() { AgencyUserRoleId = 1, IsDefault = 0, CodeName = "A", Name = "Role A" };
            _agencyUserRoleB = new AgencyUserRole() { AgencyUserRoleId = 2, IsDefault = 0, CodeName = "B", Name = "Role B" };
            userWithBothRoleAndTeam = new AgencyUser() { Username = "userRoleTeam", Role = _agencyUserRoleB, Team = _agencyUserTeamB };

            var dbInitialiser = new DatabaseInitialiser();

            dbInitialiser.Publish(true);

            var seedScripts = new string[]
            {
            };
            var seedObjects = GetSeedObjects();

            dbInitialiser.Seed(seedScripts);
            dbInitialiser.Seed(seedObjects);

            _connection = dbInitialiser.GetOpenConnection();

            _logger = new Mock<ILogService>();

            _repoUnderTest = new AgencyUserRepository(_connection, _mapper, _logger.Object);
        }

        private object[] GetSeedObjects()
        {
            var seedObjects = new object[] {_agencyUserTeamA, _agencyUserTeamB, _agencyUserRoleA, _agencyUserRoleB, userWithBothRoleAndTeam};

            return seedObjects;
        }

        /// <summary>
        /// Ensure it gets the user along with role and team
        /// </summary>
        [Test]
        public void DoGetByUsername()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.Get(userWithBothRoleAndTeam.Username);

            //Assert
            result.Should().NotBeNull();
            result.Role.Should().NotBeNull();
            result.Team.Should().NotBeNull();
        }

        [Test]
        public void DoSaveWithoutRoleOrTeam()
        {
            //Arrange
            var newAgencyUser = new Domain.Entities.Users.AgencyUser();
            newAgencyUser.Username = Guid.NewGuid().ToString();
            newAgencyUser.Role = null;
            newAgencyUser.Team = null;

            //Act
            var result = _repoUnderTest.Save(newAgencyUser);

            //Assert
            result.Should().NotBeNull();
            result.Role.Should().BeNull();
            result.Team.Should().BeNull();
        }

        [Test]
        public void DoSaveWithRole()
        {
            //Arrange
            var newAgencyUser = new Domain.Entities.Users.AgencyUser();
            newAgencyUser.Username = Guid.NewGuid().ToString();
            newAgencyUser.Role = new Domain.Entities.Users.Role()
            {
                CodeName = "B",
                Id = _agencyUserRoleB.AgencyUserRoleId.ToString(),
                IsDefault = false,
                Name = "Anything you like"
            };
            newAgencyUser.Team = null;

            //Act
            var result = _repoUnderTest.Save(newAgencyUser);

            //Assert
            result.Should().NotBeNull();
            result.Role.Should().NotBeNull();
            result.Team.Should().BeNull();
            result.Role.CodeName.Should().Be("B");
            result.Role.Id.Should().Be(_agencyUserRoleB.AgencyUserRoleId.ToString());
        }

        [Test]
        public void DoSaveWithTeam()
        {
            //Arrange
            var newAgencyUser = new Domain.Entities.Users.AgencyUser();
            newAgencyUser.Username = Guid.NewGuid().ToString();
            newAgencyUser.Team = new Domain.Entities.Users.Team()
            {
                CodeName = "B",
                Id = _agencyUserTeamB.AgencyUserTeamId.ToString(),
                IsDefault = false,
                Name = "Anything you like"
            };
            newAgencyUser.Role = null;

            //Act
            var result = _repoUnderTest.Save(newAgencyUser);

            //Assert
            result.Should().NotBeNull();
            result.Team.Should().NotBeNull();
            result.Role.Should().BeNull();
            result.Team.CodeName.Should().Be("B");
            result.Team.Id.Should().Be(_agencyUserTeamB.AgencyUserTeamId.ToString());
        }
    }
}
