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
        private AgencyUser userWithBothRoleAndTeam;


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            userWithBothRoleAndTeam = new AgencyUser() { Username = "userRoleTeam"};

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
            var seedObjects = new object[] {userWithBothRoleAndTeam};

            return seedObjects;
        }

        [Test]
        public void DoGetByUsername()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.Get(userWithBothRoleAndTeam.Username);

            //Assert
            result.Should().NotBeNull();
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
    }
}
