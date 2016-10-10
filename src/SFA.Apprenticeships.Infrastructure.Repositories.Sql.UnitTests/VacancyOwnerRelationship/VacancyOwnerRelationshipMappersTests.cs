namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.VacancyOwnerRelationship
{
    using NUnit.Framework;
    using Schemas.dbo;

    [TestFixture]
    [Parallelizable]
    public class VacancyOwnerRelationshipMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var mapper = new VacancyOwnerRelationshipMappers();

            // Act
            mapper.Initialise();

            // Assert
            mapper.Mapper.AssertConfigurationIsValid();
        }
    }
}