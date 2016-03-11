namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.VacancyParty
{
    using NUnit.Framework;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class VacancyPartyMappersTests
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var mapper = new VacancyPartyMappers();

            // Act
            mapper.Initialise();

            // Assert
            mapper.Mapper.AssertConfigurationIsValid();
        }
    }
}