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

    [TestFixture(Category = "Integration")]
    public class AgencyUserRepositoryTests
    {
        private AgencyUserRepository _repoUnderTest;
        private readonly IMapper _mapper = new AgencyUserMappers();
        private IGetOpenConnection _connection;
        private Mock<ILogService> _logger;


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _logger = new Mock<ILogService>();

            _repoUnderTest = new AgencyUserRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        public void DoGetByUsername()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.GetByUsername(SeedData.AgencyUser1.Username);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void DoSaveWithoutRoleOrTeam()
        {
            //Arrange
            var newAgencyUser = new Domain.Entities.Raa.Users.AgencyUser
            {
                Username = Guid.NewGuid().ToString(),
                Role = null,
                Team = null
            };

            //Act
            var result = _repoUnderTest.Save(newAgencyUser);

            //Assert
            result.Should().NotBeNull();
            result.Role.Should().BeNull();
            result.Team.Should().BeNull();
        }
    }
}
