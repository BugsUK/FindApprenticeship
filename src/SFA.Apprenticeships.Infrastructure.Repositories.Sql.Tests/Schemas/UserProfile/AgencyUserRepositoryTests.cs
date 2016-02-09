namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.UserProfile
{
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
        private Team teamA;
        private Team teamB;
        private Role roleA;
        private Role roleB;
        private AgencyUser userWithNeitherRoleNorTeam;
        private AgencyUser userWithRole;
        private AgencyUser userWithTeam;
        private AgencyUser userWithBothRoleAndTeam;


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            teamA = new Team() {Id = 1, IsDefault = false, CodeName = "A", Name = "Team A"};
            teamB = new Team() {Id = 2, IsDefault = false, CodeName = "B", Name = "Team B"};
            roleA = new Role() {Id = 1, IsDefault = false, CodeName = "A", Name = "Role A"};
            roleB = new Role() {Id = 2, IsDefault = false, CodeName = "B", Name = "Role B"};
            userWithNeitherRoleNorTeam = new AgencyUser() {Username = "user"};
            userWithRole = new AgencyUser() {Username = "userWithRole", RoleId = roleA.Id};
            userWithTeam = new AgencyUser() {Username = "userTeam", TeamId = teamA.Id};
            userWithBothRoleAndTeam = new AgencyUser() {Username = "userRoleTeam", RoleId = roleB.Id, TeamId = teamB.Id};

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
            var seedObjects = new object[] {teamA, teamB, roleA, roleB, userWithBothRoleAndTeam, userWithNeitherRoleNorTeam, userWithRole, userWithTeam};

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
