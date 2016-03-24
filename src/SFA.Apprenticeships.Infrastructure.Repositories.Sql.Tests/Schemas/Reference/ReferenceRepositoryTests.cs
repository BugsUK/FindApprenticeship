namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Reference
{
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Vacancies;
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
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

        //[Test]
        //public void GetCounties()
        //{
        //    //Arrange
        //    var logger = new Mock<ILogService>();
        //    var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

        //    //Act
        //    var counties = repository.GetCounties();

        //    //Assert
        //    counties.Count.Should().Be(47);
        //    counties[0].CountyId.Should().Be(1);
        //    counties[0].CodeName.Should().Be("BED");
        //    counties[0].ShortName.Should().Be("BED");
        //    counties[0].FullName.Should().Be("Bedfordshire");
        //}

        //[Test]
        //public void GetRegions()
        //{
        //    //Arrange
        //    var logger = new Mock<ILogService>();
        //    var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

        //    //Act
        //    var counties = repository.GetRegions();

        //    //Assert
        //    counties.Count.Should().Be(10);
        //    counties[0].RegionId.Should().Be(0);
        //    counties[0].CodeName.Should().Be("NUL");
        //    counties[0].ShortName.Should().Be("NUL");
        //    counties[0].FullName.Should().Be("Unspecified");
        //}

        //[Test]
        //public void GetLocalAuthorities()
        //{
        //    //Arrange
        //    var logger = new Mock<ILogService>();
        //    var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

        //    //Act
        //    var localAuthorities = repository.GetLocalAuthorities();

        //    //Assert
        //    localAuthorities.Count.Should().Be(326);
        //    var firstLA = localAuthorities.First(la => la.LocalAuthorityId == 1);
        //    firstLA.LocalAuthorityId.Should().Be(1);
        //    firstLA.CodeName.Should().Be("45UB");
        //    firstLA.ShortName.Should().Be("45UB");
        //    firstLA.FullName.Should().Be("Adur");
        //    firstLA.County.CountyId.Should().Be(42);
        //    firstLA.County.CodeName.Should().Be("WSX");
        //    firstLA.County.ShortName.Should().Be("WSX");
        //    firstLA.County.FullName.Should().Be("West Sussex");
        //}

        [Test]
        public void GetStandards()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var standards = repository.GetStandards();

            //Assert
            standards.Should().NotBeNullOrEmpty();
            standards.Any(std => std.ApprenticeshipLevel == ApprenticeshipLevel.Unknown
                                 || std.ApprenticeshipSectorId == 0
                                 || std.Id == 0
                                 || string.IsNullOrWhiteSpace(std.Name))
                                 .Should().BeFalse();
        }

        [Test]
        public void GetSectors()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var sectors = repository.GetSectors();

            //Assert
            sectors.Should().NotBeNullOrEmpty();
            sectors.Any(std => std.Id == 0
                               || string.IsNullOrWhiteSpace(std.Name)
                               || !std.Standards.Any())
                               .Should().BeFalse();
        }

        [Test]
        public void GetFrameworks()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var frameworks = repository.GetFrameworks();

            //Assert
            frameworks.Should().NotBeNullOrEmpty();
            frameworks.Any(std => std.Id == 0
                                  || !string.IsNullOrWhiteSpace(std.CodeName)
                                  || !string.IsNullOrWhiteSpace(std.FullName)
                                  || !string.IsNullOrWhiteSpace(std.ShortName)
                                  || std.Occupation == null)
                .Should().BeFalse();
        }

        [Test]
        public void GetOccupations()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new ReferenceRepository(_connection, _mapper, logger.Object);

            //Act
            var occupations = repository.GetOccupations();

            //Assert
            occupations.Should().NotBeNullOrEmpty();
            occupations.Any(std => std.Id == 0
                                   || !string.IsNullOrWhiteSpace(std.CodeName)
                                   || !string.IsNullOrWhiteSpace(std.FullName)
                                   || !string.IsNullOrWhiteSpace(std.ShortName))
                .Should().BeFalse();
        }
    }
}