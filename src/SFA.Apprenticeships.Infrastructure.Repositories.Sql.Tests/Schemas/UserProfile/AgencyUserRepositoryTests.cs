namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.UserProfile
{
    using Common;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.UserProfile;

    [TestFixture(Category = "Integration")]
    public class AgencyUserRepositoryTests
    {
        private readonly IMapper _mapper = new AgencyUserMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
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
            var seedObjects = new object[] {};

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
