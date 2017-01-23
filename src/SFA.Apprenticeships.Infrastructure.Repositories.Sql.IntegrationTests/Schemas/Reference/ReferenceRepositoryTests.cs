namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.Reference
{
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Reference;

    [TestFixture(Category = "Integration")]
    public class ReferenceRepositoryTests
    {
        private readonly IMapper _mapper = new ReferenceMappers();
        private IGetOpenConnection _connection;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

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
                               || std.Standards.Any(x => x.ApprenticeshipSectorId != std.Id))
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
                                  || string.IsNullOrWhiteSpace(std.CodeName)
                                  || string.IsNullOrWhiteSpace(std.FullName)
                                  || string.IsNullOrWhiteSpace(std.ShortName))
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
            occupations.Any(occupation => occupation.Id == 0
                                   || string.IsNullOrWhiteSpace(occupation.CodeName)
                                   || string.IsNullOrWhiteSpace(occupation.FullName)
                                   || string.IsNullOrWhiteSpace(occupation.ShortName)
                                   || occupation.Frameworks == null
                                   || !occupation.Frameworks.Any())
                .Should().BeFalse();
        }
    }
}