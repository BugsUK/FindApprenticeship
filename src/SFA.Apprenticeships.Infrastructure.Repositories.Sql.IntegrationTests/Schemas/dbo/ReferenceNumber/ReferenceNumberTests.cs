namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.ReferenceNumber
{
    using Common;
    using FluentAssertions;
    using NUnit.Framework;
    using Sql.Common;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class ReferenceNumberTests
    {
        private GetOpenConnectionFromConnectionString _connection;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

        [Test]
        public void ShouldGetNextVacancyReferenceNumbers()
        {
            // Arrange.
            var repository = new ReferenceNumberRepository(_connection);

            // Act.
            var firstReferenceNumber = repository.GetNextVacancyReferenceNumber();
            var secondReferenceNumber = repository.GetNextVacancyReferenceNumber();

            // Assert.
            firstReferenceNumber.Should().BePositive();
            secondReferenceNumber.Should().BeGreaterThan(firstReferenceNumber);
        }

        [Test]
        public void ShouldGetNextLegacyApplicationIds()
        {
            // Arrange.
            var repository = new ReferenceNumberRepository(_connection);

            // Act.
            var firstLegacyApplicationId = repository.GetNextLegacyApplicationId();
            var secondLegacyApplicationId = repository.GetNextLegacyApplicationId();

            // Assert.
            firstLegacyApplicationId.Should().BeNegative();
            secondLegacyApplicationId.Should().BeLessThan(firstLegacyApplicationId);
        }
    }
}
