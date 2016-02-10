namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.UserProfile
{
    using System.Runtime.Remoting.Metadata.W3cXsd2001;
    using Common;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.UserProfile;
    using Sql.Schemas.UserProfile.Entities;

    [TestFixture(Category = "Integration")]
    public class AgencyUserRepositoryTests
    {
        private readonly IMapper _mapper = new AgencyUserMappers();
        private IGetOpenConnection _connection;
        private AgencyUserTeam _agencyUserTeamA;
        private AgencyUserTeam _agencyUserTeamB;
        private AgencyUserRole _agencyUserRoleA;
        private AgencyUserRole _agencyUserRoleB;
        private AgencyUser userWithNeitherRoleNorTeam;
        private AgencyUser userWithRole;
        private AgencyUser userWithTeam;
        private AgencyUser userWithBothRoleAndTeam;


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _agencyUserTeamA = new AgencyUserTeam() { AgencyUserTeamId = 1, IsDefault = 0, CodeName = "A", Name = "Team A" };
            _agencyUserTeamB = new AgencyUserTeam() { AgencyUserTeamId = 2, IsDefault = 0, CodeName = "B", Name = "Team B" };
            _agencyUserRoleA = new AgencyUserRole() { AgencyUserRoleId = 1, IsDefault = 0, CodeName = "A", Name = "Role A" };
            _agencyUserRoleB = new AgencyUserRole() { AgencyUserRoleId = 2, IsDefault = 0, CodeName = "B", Name = "Role B" };
            userWithNeitherRoleNorTeam = new AgencyUser() { Username = "user" };
            userWithRole = new AgencyUser() { Username = "userWithRole", Role = _agencyUserRoleA };
            userWithTeam = new AgencyUser() { Username = "userTeam", Team = _agencyUserTeamA };
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
        }

        private object[] GetSeedObjects()
        {
            var seedObjects = new object[] {_agencyUserTeamA, _agencyUserTeamB, _agencyUserRoleA, _agencyUserRoleB, userWithBothRoleAndTeam, userWithNeitherRoleNorTeam, userWithRole, userWithTeam};

            return seedObjects;
        }

        /// <summary>
        /// Ensure it gets the user along with role and team
        /// </summary>
        [Test]
        public void DoGetById()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Ensure it gets the user along with role and team
        /// </summary>
        [Test]
        public void DoGetByUsername()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Ensure it deletes the user, not the role or team
        /// </summary>
        [Test]
        public void DoDeleteById()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void DoSaveWithoutRoleOrTeam()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void DoSaveWithRole()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void DoSaveWithTeam()
        {
            Assert.Inconclusive();
        }
    }
}
