namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Reference
{
    using Common;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Reference;
    using Sql.Schemas.Vacancy;

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
    }
}