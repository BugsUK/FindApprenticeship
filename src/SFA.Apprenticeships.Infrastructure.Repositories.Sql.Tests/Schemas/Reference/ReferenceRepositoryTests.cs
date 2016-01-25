namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Reference
{
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Reference;

    [TestFixture(Category = "Integration")]
    public class ReferenceRepositoryTests
    {
        private readonly IMapper _mapper = new ReferenceMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            dbInitialiser.Publish(true);

            _connection = dbInitialiser.GetOpenConnection();
        }

        [Test]
        public void GetCounties()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var counties = repository.GetCounties();

            //Assert
            counties.Count.Should().Be(47);
            counties[0].CountyId.Should().Be(1);
            counties[0].CodeName.Should().Be("BED");
            counties[0].ShortName.Should().Be("BED");
            counties[0].FullName.Should().Be("Bedfordshire");
        }

        [Test]
        public void GetRegions()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var counties = repository.GetRegions();

            //Assert
            counties.Count.Should().Be(9);
            counties[0].RegionId.Should().Be(1001);
            counties[0].CodeName.Should().Be("EM");
            counties[0].ShortName.Should().Be("EM");
            counties[0].FullName.Should().Be("East Midlands");
        }

        [Test]
        public void GetLocalAuthorities()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var counties = repository.GetLocalAuthorities();

            //Assert
            counties.Count.Should().Be(326);
            counties[0].LocalAuthorityId.Should().Be(1);
            counties[0].CodeName.Should().Be("45UB");
            counties[0].ShortName.Should().Be("45UB");
            counties[0].FullName.Should().Be("Adur");
        }
    }
}