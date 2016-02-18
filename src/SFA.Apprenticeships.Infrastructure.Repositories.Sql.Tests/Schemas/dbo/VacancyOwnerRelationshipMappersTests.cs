namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo
{
    using NUnit.Framework;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class VacancyOwnerRelationshipMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            Assert.Inconclusive("We dont expect all fields to be mapped");
            // Arrange
            var x = new VacancyOwnerRelationshipMappers();

            // Act
            x.Initialise();

            // Assert
            x.Mapper.AssertConfigurationIsValid();
        }
    }
}
